using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.AI;

public class Fantom : MonoBehaviour
{
    GameObject player; // Reference to the player GameObject
    NavMeshAgent agent; // Reference to the NavMeshAgent component

    private Material defaultMaterial; // Reference to the default material of the fantom

    [SerializeField] private Material powerUpMaterial; // Reference to the power-up material of the fantom
    private Renderer renderer; // Reference to the Renderer component of the fantom

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player GameObject by its tag
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to this GameObject

        renderer = GetComponent<Renderer>(); // Get the Renderer component attached to this GameObject
        if (renderer != null)
        {
            defaultMaterial = renderer.material; // Store the default material of the fantom
        }
        else
        {
            Debug.LogWarning("Renderer not found!"); // Log a warning if the Renderer component is not found
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGameOver()) DeplacerFantom(); // Call the DeplacerFantom method to move the fantom
    }

    private void DeplacerFantom()
    {
        if (player != null)
        {
            if (GameManager.Instance.IsInPowerMode())
            {
                NavMeshPath path = new NavMeshPath(); // Create a new NavMeshPath object
                agent.CalculatePath(player.transform.position * -1, path); // Calculate the path to the player's position
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(player.transform.position*-1);
                }
                else
                {
                    if(path.corners.Length > 0)
                    {
                        agent.SetDestination(path.corners[path.corners.Length-1]); // Set the destination of the NavMeshAgent to the first corner of the path
                    }
                    else
                    {
                        Debug.LogWarning("No path found!"); // Log a warning if no path is found
                    }
                }
            }
            else
            {
                agent.SetDestination(player.transform.position); // Set the destination of the NavMeshAgent to the player's position
            }
        }
        else
        {
            Debug.LogWarning("Player not found!"); // Log a warning if the player GameObject is not found
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.IsInPowerMode()) // Check if the player is in power mode
            {
                agent.Warp(new Vector3(0, 0, 1f)); // Warp the fantom to a random position if the player is in power mode
            }
            else
            {
                GameManager.Instance.GameOver(); // Call the GameOver method in the GameManager when the fantom collides with the player
            }
        }
    }

    public void SetPowerUpMaterial()
    {
        if (renderer != null)
        {
            renderer.material = powerUpMaterial; // Set the material of the fantom to the power-up material
        }
    }
    
    public void ResetMaterial()
    {
        if (renderer != null)
        {
            renderer.material = defaultMaterial; // Reset the material of the fantom to the default material
        }
    }
}

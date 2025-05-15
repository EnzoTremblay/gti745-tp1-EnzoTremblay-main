using UnityEngine;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    public static GameManager Instance; // Singleton instance of the GameManager

    private bool isGameOver = false; // Variable to check if the game is over

    private bool isInPowerMode = false; // Reference to the game over UI GameObject

    private int score = 0; // Variable to store the score

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance to this GameManager
            DontDestroyOnLoad(gameObject); // Prevent this GameManager from being destroyed when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy this GameManager if another instance already exists
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player GameObject by its tag
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            verifierPointRestant(); // Call the verifierPointRestant method to check if there are any dots left
        }
    }

    public bool IsGameOver()
    {
        return isGameOver; // Return the game over status
    }

    public bool IsInPowerMode()
    {
        return isInPowerMode; // Return the power mode status
    }

    public void SetInPowerMode(bool isInPowerMode)
    {
        this.isInPowerMode = isInPowerMode; // Set the power mode status
        ChangerMaterialFantoms(isInPowerMode); // Call the ChangerMaterialFantom method to change the material of the fantom
        if (isInPowerMode)
        {
            SoundManager.Instance.PlayPowerUpSound(); // Play the power-up sound
        }
        else
        {
            SoundManager.Instance.StopPowerUpSound(); // Stop the power-up sound
        }
    }

    public void addScore(int newPoint)
    {
        this.score += newPoint; // Add the score to the current score

        UIManager.Instance.UpdateScore(this.score); // Update the score in the UI

    }

    public void GameOver()
    {
        isGameOver = true; // Set the game over flag to true
        UIManager.Instance.afficherFinPerdu(); // Display the game over UI
        SoundManager.Instance.PlayGameOverSound(); // Play the game over sound
        Destroy(player); // Destroy the GameManager object
    }

    public void GameWin()
    {
        isGameOver = true; // Set the game over flag to true
        UIManager.Instance.afficherFinVictoire(); // Display the victory UI
        SoundManager.Instance.PlayVictorySound(); // Play the victory sound
        Destroy(player); // Destroy the GameManager object
    }

    private void verifierPointRestant()
    {
        //find all the dots in the scene   
        GameObject[] dots = GameObject.FindGameObjectsWithTag("Dots"); // Find all Dots in the scene

        if (dots.Length == 0)
        {
            GameWin(); // Call the GameWin method if there are no dots left
        }
    }
    
    private void ChangerMaterialFantoms(bool isInPowerMode)
    {
        GameObject[] fantoms = GameObject.FindGameObjectsWithTag("Fantome"); // Find all Dots in the scene

        foreach (GameObject fantom in fantoms)
        {
            Fantom fantomScript = fantom.GetComponent<Fantom>(); // Get the Fantom component attached to the GameObject

            if (isInPowerMode)
            {
                fantomScript.SetPowerUpMaterial(); // Set the power-up material if in power mode
            }
            else
            {
                fantomScript.ResetMaterial(); // Reset the material if not in power mode
            }
        }
    }
}

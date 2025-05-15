using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 80f; // Speed of rotation

    // Update is called once per frame
    void Update()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime; // Calculate the rotation amount based on the speed and time

        transform.Rotate(0, rotationAmount, 0); // Rotate the power-up around the Y-axis
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>(); // Get the PlayerController component from the player GameObject
            if (playerController != null)
            {
                playerController.ActivatePowerUp(); // Call the ActivatePowerUp method in the PlayerController when the player collides with the power-up
            }
            
            Destroy(gameObject); // Destroy the power-up GameObject
        }
    }
}

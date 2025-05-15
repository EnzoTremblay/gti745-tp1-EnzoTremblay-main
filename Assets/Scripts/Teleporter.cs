using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination; // Reference to the teleport destination transform

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController characterController = other.GetComponent<CharacterController>(); // Get the CharacterController component from the player
            if (characterController != null)
            {
                characterController.enabled = false; // Disable the CharacterController to prevent movement during teleportation
            }
            other.transform.position = teleportDestination.position; // Teleport the player to the destination position
            if (characterController != null)
            {
                characterController.enabled = true; // Disable the CharacterController to prevent movement during teleportation
            }
        }
    }
}

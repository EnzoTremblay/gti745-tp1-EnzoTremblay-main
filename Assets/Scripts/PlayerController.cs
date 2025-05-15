using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;// Speed of the player

    [SerializeField] private float speedPowerUp = 8f; // Speed of the power-up

    [SerializeField] private float speedRotation = 10f; // Speed of rotation

    [SerializeField] private float powerDuration = 5f; // Duration of the power-up effect

    private PlayerInput playerInput; // Reference to the PlayerInput component

    private CharacterController characterController; // Reference to the CharacterController component

    private InputAction moveAction; // Reference to the move action

    private Vector2 moveInput; // Store the input from the player

    private Coroutine powerUpCoroutine; // Reference to the coroutine for the power-up effect


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        characterController = GetComponent<CharacterController>(); // Get the CharacterController component attached to the GameObject
        playerInput = GetComponent<PlayerInput>(); // Get the PlayerInput component attached to the GameObject

        moveAction = playerInput.actions["Move"]; // Get the "Move" action from the PlayerInput component 
    }

    public void OnMove()
    {
        moveInput = moveAction.ReadValue<Vector2>(); // Read the value of the move action and store it in moveInput
    }


    // Update is called once per frame
    void Update()
    {
        MovePlayer(); // Call the MovePlayer method to move the player
    }

    private void MovePlayer()
    {
        float nouvelleVitesse = speed; // Set the speed to the defined speed

        if (GameManager.Instance.IsInPowerMode()) // Check if the player is in power mode
        {
            nouvelleVitesse = speedPowerUp; // Double the speed if in power mode
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * nouvelleVitesse * Time.deltaTime; // Create a new Vector3 for movement based on the input

        characterController.Move(move); // Move the CharacterController based on the input

        // Rotate the player to face the direction of movement
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move); // Create a target rotation based on the movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speedRotation); // Smoothly rotate the player towards the target rotation
        }
    }

    public void ActivatePowerUp()
    {
        if(powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine); // Stop the previous power-up coroutine if it exists
        }
        powerUpCoroutine = StartCoroutine(PowerUpCoroutine(powerDuration)); // Start the power-up coroutine
    }

    private IEnumerator PowerUpCoroutine(float duration)
    {
        GameManager.Instance.SetInPowerMode(true); // Set the game manager to power mode
        yield return new WaitForSeconds(powerDuration); // Wait for the duration of the power-up
        GameManager.Instance.SetInPowerMode(false); // Set the game manager back to normal mode
    }      
}

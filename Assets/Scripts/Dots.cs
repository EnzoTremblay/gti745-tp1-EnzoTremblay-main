using UnityEngine;

public class Dots : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.addScore(10); // Add 1 point to the score when the player collides with the dot
            SoundManager.Instance.PlayEatingSound(); // Play the point sound
            Destroy(this.gameObject); // Destroy the dot when the player collides with it
        }
    }
}

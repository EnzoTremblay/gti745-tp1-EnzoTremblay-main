using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton instance of the SoundManager

    private AudioSource audioSourcePowerUp; // Reference to the AudioSource component

    private AudioSource audioSourcePoints; // Reference to the AudioSource component

    private AudioSource audioSourceGameOver; // Reference to the AudioSource component  

    [SerializeField] private AudioClip audioClipPowerUp; // Reference to the AudioClip for power-up sound
    [SerializeField] private AudioClip audioClipManger1; // Reference to the AudioClip for eating sound
    [SerializeField] private AudioClip audioClipManger2; // Reference to the AudioClip for eating sound
    [SerializeField] private AudioClip audioClipGameOver; // Reference to the AudioClip for game over sound
    [SerializeField] private AudioClip audioClipVictory; // Reference to the AudioClip for victory sound
    private bool jouerPremierSonDeManger = true; // Flag to check if the first eating sound has been played

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the instance to this SoundManager
            DontDestroyOnLoad(gameObject); // Prevent this SoundManager from being destroyed when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy this SoundManager if another instance already exists
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ajouter un AudioSource pour le power-up
        audioSourcePowerUp = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component for power-up sound
        audioSourcePowerUp.clip = audioClipPowerUp; // Set the AudioClip for power-up sound
        audioSourcePowerUp.loop = true; // Set the AudioSource to loop

        // Ajouter un AudioSource pour le son de manger
        audioSourcePoints = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component for eating sound

        // Ajouter un AudioSource pour le son de game over
        audioSourceGameOver = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component for game over sound

    }

    public void PlayPowerUpSound()
    {
        if (!audioSourcePowerUp.isPlaying && audioSourcePowerUp != null) // Check if the power-up sound is not already playing
        {
            audioSourcePowerUp.Play(); // Play the power-up sound
        }
    }

    public void StopPowerUpSound()
    {
        if (audioSourcePowerUp.isPlaying && audioSourcePowerUp != null) // Check if the power-up sound is playing
        {
            audioSourcePowerUp.Stop(); // Stop the power-up sound
        }
    }

    public void PlayEatingSound()
    {
        if (jouerPremierSonDeManger)
        {
            audioSourcePoints.clip = audioClipManger1; // Set the AudioClip for the first eating sound
            jouerPremierSonDeManger = false; // Set the flag to false to play the second sound next time
        }
        else
        {
            audioSourcePoints.clip = audioClipManger2; // Set the AudioClip for the second eating sound
            jouerPremierSonDeManger = true; // Set the flag to true to play the first sound next time
        }

        if (!audioSourcePoints.isPlaying && audioSourcePoints != null) // Check if the eating sound is not already playing
        {
            audioSourcePoints.Play(); // Play the eating sound
        }
    }

    public void PlayGameOverSound()
    {
        if (!audioSourceGameOver.isPlaying && audioClipGameOver != null) // Check if the game over sound is not already playing
        {
            audioSourceGameOver.PlayOneShot(audioClipGameOver); // Play the game over sound
        }
    }

    public void PlayVictorySound()
    {
        if (!audioSourceGameOver.isPlaying && audioClipVictory != null) // Check if the victory sound is not already playing
        {
            audioSourceGameOver.PlayOneShot(audioClipVictory); // Play the victory sound
        }
    }
}

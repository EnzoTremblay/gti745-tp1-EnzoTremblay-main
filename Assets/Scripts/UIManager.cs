using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance of the UIManager

    [SerializeField] private TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    [SerializeField] private TextMeshProUGUI finVictoireText; // Reference to the TextMeshProUGUI component for displaying the victory message
    [SerializeField] private TextMeshProUGUI finPerduText; // Reference to the TextMeshProUGUI component for displaying the defeat message

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

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString(); // Update the score text with the new score
    }

    public void afficherFinVictoire()
    {
        finVictoireText.gameObject.SetActive(true);
        finPerduText.gameObject.SetActive(false); 
    }
    
    public void afficherFinPerdu()
    {
        finVictoireText.gameObject.SetActive(false);
        finPerduText.gameObject.SetActive(true); 
    }
}

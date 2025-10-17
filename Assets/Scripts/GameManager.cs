using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int score;
    public static int highestScore = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI speedText; // Add a UI text for speed

    private PlayerController playerController; // Reference to PlayerController

    public static int Flag = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>(); // Find the PlayerController in the scene
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            UpdateSpeed(); // Update speed UI every frame
        }
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }
    public void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
        highestScore = Mathf.Max(score, highestScore);
    }
    void UpdateSpeed()
    {
        speedText.text = "Speed: " + playerController.GetSpeed().ToString("F1"); // Display speed with 1 decimal place
    }
}

using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;
    private GameManager gameManager;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameWinText;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "SCORE: " + GameManager.score.ToString();
        highestScoreText.text = "HIGHEST SCORE: " + GameManager.highestScore.ToString();

        int flag = PlayerPrefs.GetInt("Flag", 0); // Check flag status

        if (GameManager.Flag == 1)
        {
            gameWinText.SetActive(true);   // Show "You Win"
            gameOverText.SetActive(false); // Hide "Game Over"
            GameManager.score = 0;

            // Reset the flag AFTER showing "You Win"
            GameManager.Flag = 0;
        }
        else
        {
            gameWinText.SetActive(false);  // Hide "You Win"
            gameOverText.SetActive(true);  // Show "Game Over"
            GameManager.score = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

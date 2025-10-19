using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;

    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject gameWinText;

    void Start()
    {
        // 🧾 Hiển thị điểm
        if (scoreText != null)
            scoreText.text = "SCORE: " + GameManager.score.ToString();

        if (highestScoreText != null)
            highestScoreText.text = "HIGHEST SCORE: " + GameManager.highestScore.ToString();

        // ⚙️ Kiểm tra thắng / thua
        if (GameManager.Flag == 1)
        {
            gameWinText.SetActive(true);
            gameOverText.SetActive(false);
            GameManager.Flag = 0; // reset lại
        }
        else
        {
            gameWinText.SetActive(false);
            gameOverText.SetActive(true);
        }

        // Reset điểm để sẵn sàng cho ván sau
        GameManager.score = 0;
    }

    // 🔁 Nút Restart Game
    public void RestartGame()
    {
        // 🔄 Reset trạng thái trước khi restart
        PlayerCollision.ResetFenceHitCount();
        PlayerCollision.ResetInvincible();
        GameManager.score = 0;

        SceneManager.LoadScene("Level1");
    }



    // 🚪 Nút Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }
}

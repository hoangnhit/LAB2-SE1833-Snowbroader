using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int score;
    public static int highestScore = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI speedText;

    private PlayerController playerController;
    public static int Flag = 0;  // 0 = thua, 1 = thắng

    void Start()
    {
        // 🔄 Reset trạng thái
        PlayerCollision.ResetFenceHitCount();
        PlayerCollision.ResetInvincible();

        // 🔁 Load Highest Score từ PlayerPrefs (nếu có)
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);

        playerController = FindAnyObjectByType<PlayerController>();
        UpdateScore();
    }

    void Update()
    {
        if (playerController != null)
            UpdateSpeed();
    }

    // 🪙 Cộng điểm
    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    // 🧾 Cập nhật điểm UI (hiện dạng: Score: hiện tại / cao nhất)
    public void UpdateScore()
    {
        highestScore = Mathf.Max(score, highestScore);

        if (scoreText != null)
            scoreText.text = $"Score: {score} / {highestScore}";

        // 💾 Lưu highestScore vào PlayerPrefs
        PlayerPrefs.SetInt("HighestScore", highestScore);
        PlayerPrefs.Save();
    }

    // 🚀 Cập nhật tốc độ hiển thị UI
    void UpdateSpeed()
    {
        if (speedText != null && playerController != null)
            speedText.text = "Speed: " + playerController.GetSpeed().ToString("F1");
    }

    // 💀 GAME OVER (chặn nếu hack bất tử)
    public void GameOver()
    {
        if (PlayerCollision.IsInvincible())
        {
            Debug.Log("🛡️ Invincible active — GameOver canceled");
            return;
        }

        Debug.Log("💀 GAME OVER!");
        Time.timeScale = 1f;

        PlayerCollision.ResetFenceHitCount();
        PlayerCollision.ResetInvincible();

        SceneManager.LoadScene("GameOver");
    }

    // 🏆 GAME WIN
    public void GameWin()
    {
        Debug.Log("🏆 YOU WIN!");
        Time.timeScale = 1f;
        Flag = 1;

        PlayerCollision.ResetFenceHitCount();
} }

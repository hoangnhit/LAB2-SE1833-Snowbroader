using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;

    private static int fenceHitCount = 0;
    private static bool invincible = false;

    private float lastHitTime = 0f;
    private const float hitCooldown = 1f;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        Debug.Log($"[Scene] Loaded {s.name}. Invincible={invincible}, fenceHitCount={fenceHitCount}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bất tử: chỉ cho nhặt coin, bỏ qua còn lại
        if (invincible)
        {
            if (other.CompareTag("Coin"))
            {
                Destroy(other.gameObject);
                gameManager.AddScore(10);
            }
            return;
        }

        // Coin
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            gameManager.AddScore(10);
            return;
        }

        // Fence
        if (other.CompareTag("Fence"))
        {
            if (Time.time - lastHitTime < hitCooldown) return;
            lastHitTime = Time.time;

            fenceHitCount++;
            Debug.Log($"🧱 Fence hit #{fenceHitCount}");

            if (fenceHitCount == 1)
            {
                playerController.ReduceSpeed();
                return;
            }

            // Nếu đang úp mặt xuống đất thì không chết, chỉ coi như va lần 1
            if (transform.up.y < 0f)
            {
                Debug.Log("🤸 Upside-down → ignore GameOver, keep as 1 hit");
                playerController.ReduceSpeed();
                fenceHitCount = 1;
                return;
            }

            // Chết thật
            playerController.DisableControls();
            gameManager.GameOver();
        }
    }

    // Cheats & reset
    public static void ToggleInvincible()
    {
        invincible = !invincible;
        Debug.Log("🛡️ Invincible = " + invincible);
    }
    public static bool IsInvincible() => invincible;

    public static void ResetInvincible()
    {
        invincible = false;
        Debug.Log("🔄 Invincible reset = false");
    }

    public static void ResetFenceHitCount()
    {
        fenceHitCount = 0;
        Debug.Log("🔄 Fence hit count reset = 0");
    }
}
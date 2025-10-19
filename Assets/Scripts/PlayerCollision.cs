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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[Scene] Loaded {scene.name}. Invincible = {invincible}, fenceHitCount = {fenceHitCount}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (invincible)
        {
            if (other.CompareTag("Coin"))
            {
                Destroy(other.gameObject);
                gameManager.AddScore(10);
            }
            else
                Debug.Log($"[Invincible] Ignored {other.tag}");
            return;
        }

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            gameManager.AddScore(10);
            return;
        }

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

            // Nếu đang úp mặt xuống đất → không chết
            if (transform.up.y < 0f)
            {
                Debug.Log("🤸 Player upside-down → ignore GameOver");
                playerController.ReduceSpeed();
                fenceHitCount = 1;
                return;
            }

            // Nếu không bất tử & không úp đầu → Game Over
            Debug.Log("💀 Game Over triggered by fence hit");
            playerController.DisableControls();
            gameManager.GameOver();
        }

        if (other.CompareTag("DeathZone") || other.CompareTag("Kill"))
        {
            if (invincible)
            {
                Debug.Log($"[Invincible] Ignored {other.tag}");
                return;
            }

            Debug.Log($"☠️ Collided with {other.tag}");
            playerController.DisableControls();
            gameManager.GameOver();
        }
    }

    // ⚙️ Cheats & Reset
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

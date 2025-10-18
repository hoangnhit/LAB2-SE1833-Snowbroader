using UnityEngine;
using System.Collections;
public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerController.TakeDamage(other);
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            audioManager.PlayCoinSound();
            gameManager.AddScore(10);
        }
        if (other.CompareTag("Fence"))
        {
            gameManager.AddScore(-10);

        }
        if (other.CompareTag("Rock"))
        {
            StartCoroutine(HandleRockHit());
        }
    }

    private IEnumerator HandleRockHit()
    {
        playerController.DisableControls();
        spriteRenderer.enabled = false;
        playerController.ReduceSpeed();
        gameObject.layer = 7;
        yield return new WaitForSeconds(3.0f); //"Pause for 1.5s" at the moment
        playerController.EnableControl();
        spriteRenderer.enabled = true;
        gameObject.layer = 6;
    }
}

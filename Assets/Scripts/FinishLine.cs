// ▼ The "using" Keyword 
//      → defines the "Namespace" Directive 
//      → that "Contains" a "Class Used" in the "Code" ▼
using UnityEngine;
using UnityEngine.SceneManagement; // ◄◄ "SceneManagement" Namespace ◄◄


public class FinishLine : MonoBehaviour
{

    [SerializeField] float loadDelay = 1f; 
    [SerializeField] ParticleSystem finishEffect;

    void Start()
    {
        // Initialize flag if not set
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            GameManager.Flag = 0; // Start at Level 1 with Flag = 0
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            finishEffect.Play();

            GetComponent<AudioSource>().Play();

            Invoke("ReloadScene", loadDelay);
        }
    }

    void ReloadScene()
    {
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            // If the player started from Level 2 and finished it, show "You Win"
            if (GameManager.Flag == 1)
            {
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                // If the player starts from Level 2, ensure they go to "You Win"
                GameManager.Flag = 1;
                SceneManager.LoadScene("GameOver");
            }
            return;
        }

        if (GameManager.Flag == 0)
        {
            // Moving from Level 1 to Level 2
            GameManager.Flag = 1;
            SceneManager.LoadScene("Level2");
        }
        else
        {
            // Player dies in Level 2 → Reset flag and go to Game Over
            GameManager.Flag = 0;
            SceneManager.LoadScene("GameOver");
        }
    }
}

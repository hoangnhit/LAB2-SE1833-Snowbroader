using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroFunction : MonoBehaviour
{
    public void IntroScene()
    {
        SceneManager.LoadScene("Intro");
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Assignment 2");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}


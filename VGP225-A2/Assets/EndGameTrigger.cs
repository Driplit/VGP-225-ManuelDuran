using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the exit. Game Over!");

            // Option 1: Load an end screen
            // SceneManager.LoadScene("EndScene");

            // Option 2: Quit the game (works in a built version, not in the editor)
            Application.Quit();

            // To stop play mode in Unity Editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
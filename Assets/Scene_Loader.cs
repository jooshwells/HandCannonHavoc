using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Prototype Scene"); // Change "GameScene" to your actual game scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Makes quit work in the editor
        #endif
    }
}


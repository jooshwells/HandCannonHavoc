using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string levelSelect;
    public void LoadGameScene()
    {
        SceneManager.LoadScene(levelSelect); // Change "GameScene" to your actual game scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Makes quit work in the editor
        #endif
    }
}


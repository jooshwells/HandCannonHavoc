using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Loader : MonoBehaviour
{
   public void LoadGameScene(string levelSelect)
    {
        Debug.Log("Button was clicked!");
        SceneManager.LoadScene(levelSelect);
    }


    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Makes quit work in the editor
        #endif
    }
}

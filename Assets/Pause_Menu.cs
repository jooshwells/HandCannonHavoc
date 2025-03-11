using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false); // Ensure options are closed when pausing
        Time.timeScale = 0f;  // Freeze game time
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Unfreeze game time
        isPaused = false;
    }

    public void OpenOptions()
    {
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HH - Main Menu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time before quitting
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Allows quitting in Unity Editor
        #endif
    }
}


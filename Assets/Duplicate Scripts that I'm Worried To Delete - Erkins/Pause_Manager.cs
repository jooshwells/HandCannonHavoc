using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject confirmExitUI; // Reference to the confirmation panel
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
        optionsMenuUI.SetActive(false);
        confirmExitUI.SetActive(false); // Hide confirm exit when pausing
        Time.timeScale = 0f;  
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        confirmExitUI.SetActive(false);
        Time.timeScale = 1f;  
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

    public void PromptExit()
    {
        confirmExitUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void CancelExit()
    {
        confirmExitUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void ConfirmExit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HH - Main Menu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }
}

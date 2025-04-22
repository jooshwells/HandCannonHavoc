using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject confirmExitUI; // Reference to the confirmation panel
    private bool isPaused = false;
    private bool cantPause = false;


    void Update()
    {
        if (cantPause && isPaused)
        {
            ResumeGame();
        } else if (cantPause)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameObject.FindGameObjectWithTag("Player") == null)
            {
                return;
            }
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PausePausing()
    {
        cantPause = !cantPause;
    }


    public void PauseGame()
    {
        player.SetActive(false);
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        confirmExitUI.SetActive(false); // Hide confirm exit when pausing
        Time.timeScale = 0f;  
        isPaused = true;
    }

    public void ResumeGame()
    {
        player.SetActive(true);
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

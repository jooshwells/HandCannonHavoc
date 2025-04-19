using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    private int nextSceneLoad;
    private int finalLevelIndex;

    void Start()
    {
        // Retrieve the final level index from PlayerPrefs.
        finalLevelIndex = PlayerPrefs.GetInt("finalLevelIndex");  

        // Calculate the next scene index based on the current active scene's build index.
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void updateCurrentLevel()
    {
        // Check if the player has completed all levels.
        if (SceneManager.GetActiveScene().buildIndex == finalLevelIndex)
        {
            Debug.Log("You Completed ALL Levels");
            // You can trigger a win screen or show credits here if needed.
        }
        else
        {
            // Unlock the next level if the player is advancing and it hasn't been unlocked yet.
            if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
            {
                PlayerPrefs.SetInt("levelAt", nextSceneLoad);
                PlayerPrefs.Save();  // Save the updated progress.
            }
        }
    }
}

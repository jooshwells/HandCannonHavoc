using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* SUBSCRIBING TO MY YOUTUBE CHANNEL: 'VIN CODES' WILL HELP WITH MORE VIDEOS AND CODE SHARING IN THE FUTURE :) THANK YOU */

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;

    // Start is called before the first frame update
    void Start()
    {
        int highestUnlockedBuildIndex = PlayerPrefs.GetInt("levelAt", 2); 

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 2 > highestUnlockedBuildIndex)
            {
                lvlButtons[i].interactable = false;
                lvlButtons[i].image.enabled = false;
            }
        }
    }

}

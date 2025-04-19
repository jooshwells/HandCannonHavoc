using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject[] levelPanels; // Panels corresponding to the levels
    private Button[] buttons;
    private int currentPlayerLevel;

    void Start()
    {
        List<Button> buttonList = new List<Button>();

        for (int i = 0; i < levelPanels.Length; i++)
        {
            Button[] panelButtons = levelPanels[i].GetComponentsInChildren<Button>(true); // Include inactive
            buttonList.AddRange(panelButtons);
        }

        buttons = buttonList.ToArray();
        Debug.Log("Number of buttons found: " + buttons.Length);
    }

    /*void Update()


    {
        currentPlayerLevel = PlayerPrefs.GetInt("levelAt", 2);  // default to first unlockable level
        Debug.Log("Current Player Level: " + currentPlayerLevel);

        int firstLevelBuildIndex = 2;

        for (int i = 0; i < levelPanels.Length; i++)
        {
            if (!levelPanels[i].activeSelf)
                continue;

            Button[] panelButtons = levelPanels[i].GetComponentsInChildren<Button>(true);

            for (int j = 0; j < panelButtons.Length; j++)
            {
                Button btn = panelButtons[j];
                TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>(true);

                int thisButtonBuildIndex = firstLevelBuildIndex + j;

                bool isUnlocked = thisButtonBuildIndex <= currentPlayerLevel;

                btn.interactable = isUnlocked;

                if (text != null)
                    text.alpha = isUnlocked ? 1f : 0f;
            }
        }
    }*/

    void Update()
    {
        currentPlayerLevel = PlayerPrefs.GetInt("levelAt", 2);  // default to first unlockable level
        Debug.Log("Current Player Level: " + currentPlayerLevel);

        int firstLevelIndex = 3;
        int levelCounter = 0;

        for (int i = 0; i < levelPanels.Length; i++)
        {
            if (!levelPanels[i].activeSelf)
                continue;

            Button[] panelButtons = levelPanels[i].GetComponentsInChildren<Button>(true);

            for (int j = 0; j < panelButtons.Length; j++)
            {
                Button btn = panelButtons[j];
                TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>(true);

                int thisButtonBuildIndex = firstLevelIndex + levelCounter;
                levelCounter++;

                bool isUnlocked = thisButtonBuildIndex <= currentPlayerLevel;

                btn.interactable = isUnlocked;

                if (text != null)
                    text.alpha = isUnlocked ? 1f : 0f;

                Debug.Log($"Button {btn.name} | Build Index: {thisButtonBuildIndex} | Unlocked: {isUnlocked}");
            }
        }
    }

}

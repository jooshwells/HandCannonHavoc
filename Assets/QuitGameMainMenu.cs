using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.End)) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}

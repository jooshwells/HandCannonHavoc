using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEVMODEONLY : MonoBehaviour
{
    public void ResetProgress()
    {
        PlayerPrefs.SetInt("levelAt", 3);
        PlayerPrefs.Save();
        Debug.Log("Progress reset to Level 1");
    }

}

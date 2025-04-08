using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
    public Toggle fullScreenToggle;

    private void Start()
    {
        if (fullScreenToggle != null)
        {
            // Sync toggle with current fullscreen state
            Screen.fullScreen = fullScreenToggle.isOn;
            Debug.Log("FullScreen: " + Screen.fullScreen + " | Mode: " + Screen.fullScreenMode);
        }
    }
}

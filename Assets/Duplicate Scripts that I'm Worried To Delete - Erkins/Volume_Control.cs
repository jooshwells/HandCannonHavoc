using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider musicSlider;    // Reference to the music slider
    public Slider SFXSlider;      // Reference to the sound effect slider
    public AudioManager audioManager;

    void Start()
    {
        // Set initial values based on the current volume
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Add listeners to the sliders to call the AudioManager's methods
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Called when the music slider value changes
    void SetMusicVolume(float volume)
    {
        audioManager.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);  // Save for future sessions
        PlayerPrefs.Save();  // Ensure the value is stored immediately
    }

    // Called when the sound effects slider value changes
    void SetSFXVolume(float volume)
    {
        audioManager.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);  // Save for future sessions
        PlayerPrefs.Save();  // Ensure the value is stored immediately
    }

}

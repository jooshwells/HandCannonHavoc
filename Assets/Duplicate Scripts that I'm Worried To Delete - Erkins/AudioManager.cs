using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Scene Background Music - Pulled From Speaker System")]
    private AudioSource musicSource;

    [Header("Scene Sound Effects")]
    public AudioSource[] SFXSources;

    void Start()
    {
        // Find the background music GameObject using the "Audio" tag
        GameObject bgMusicObject = GameObject.FindGameObjectWithTag("Audio");

        if (bgMusicObject != null)
        {
            musicSource = bgMusicObject.GetComponent<AudioSource>();
        }

        // Load saved volume settings
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);
    }


    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume); // Save to PlayerPrefs
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        foreach (var sfx in SFXSources)
        {
            sfx.volume = volume;
        }
        PlayerPrefs.SetFloat("SFXVolume", volume); // Save to PlayerPrefs
        PlayerPrefs.Save();
    }
}

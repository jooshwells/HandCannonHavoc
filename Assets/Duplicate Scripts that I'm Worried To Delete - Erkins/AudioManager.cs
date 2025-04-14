using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Scene Background Music - Pulled From Speaker System")]
    private AudioSource musicSource;

    [Header("Scene Sound Effects")]
    public AudioSource[] SFXSources;

    private bool playerNotInit = false;

    void Start()
    {
        // Find the background music GameObject using the "Audio" tag
        GameObject bgMusicObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (bgMusicObject != null)
        {
            musicSource = bgMusicObject.GetComponent<AudioSource>();
        }

        if (player != null)
        {
            AudioSource[] playerSFX = player.GetComponents<AudioSource>();
            SFXSources = SFXSources.Concat(playerSFX).Distinct().ToArray(); // Merge both arrays
        } else
        {
            playerNotInit = true;
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

    private void Update()
    {
        if(playerNotInit)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
            {
                AudioSource[] playerSFX = player.GetComponents<AudioSource>();
                SFXSources = SFXSources.Concat(playerSFX).Distinct().ToArray(); // Merge both arrays
                playerNotInit= false;
            }
        }
    }
}

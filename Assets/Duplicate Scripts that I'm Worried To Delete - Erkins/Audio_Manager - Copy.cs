using UnityEngine;
public class Audio_Manager : MonoBehaviour
{
    public AudioSource[] musicSources; // Array of AudioSources for music
    public AudioSource[] sfxSources;   // Array of AudioSources for sound effects

    public void SetMusicVolume(float volume)
    {
        foreach (var source in musicSources)
        {
            source.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        foreach (var source in sfxSources)
        {
            source.volume = volume;
        }
    }
}

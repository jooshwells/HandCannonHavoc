using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public List<AudioClip> musicTracks = new List<AudioClip>(); // List for music
    public List<AudioClip> soundEffects = new List<AudioClip>(); // List for SFX

    void Start()
    {
        if (musicTracks.Count > 0)
        {
            musicSource.clip = musicTracks[0]; // Play the first track if available
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

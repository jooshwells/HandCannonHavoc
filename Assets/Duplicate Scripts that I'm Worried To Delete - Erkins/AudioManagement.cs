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
            musicSource.clip = musicTracks[0];
            Debug.Log("Playing music: " + musicSource.clip.name);
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("No music tracks found!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && soundEffects.Count > 0)
        {
            PlaySFX(soundEffects[0]); // Play first SFX as a test
        }
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            Debug.Log("Playing SFX: " + clip.name);
            SFXSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX clip is null!");
        }
    }
}

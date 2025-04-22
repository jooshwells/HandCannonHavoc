using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField] private GameObject contents;

    private GameObject player;
    private bool opened = false;
    private AudioSource asrc;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        asrc = gameObject.GetComponent<AudioSource>();
    }
    private Transform p;
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            p = player.transform;
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) {
                return;
            } else
            {
                p = player.transform;
            }
        }
        if(!opened && Vector2.Distance(transform.position, p.position) < 2f)
        {
            opened = true;
            //asrc.Play();
            StartCoroutine(OpenChest());
        }
    }

    public IEnumerator PlaySound(AudioClip clip, Transform enemy, bool isAmbient)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.parent = enemy;
        tempGO.transform.localPosition = Vector3.zero;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        aSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);

        aSource.spatialBlend = 1.0f;
        aSource.minDistance = 1f;
        aSource.maxDistance = 20f;
        aSource.rolloffMode = AudioRolloffMode.Linear;

        aSource.Play();
        Destroy(tempGO, clip.length);
        yield return new WaitForSeconds(clip.length);
        if (isAmbient)
        {
            StartCoroutine(PlaySound(clip, transform, true));
        }
        yield return null;
    }

    IEnumerator OpenChest()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.Play("Open");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.8f);
        Instantiate(contents, transform);
        StartCoroutine(PlaySound(asrc.clip, transform, false));
        //asrc.Play();

        // Spawn that grappling hook

    }
}

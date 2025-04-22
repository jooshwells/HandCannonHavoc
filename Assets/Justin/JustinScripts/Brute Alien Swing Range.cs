using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BruteAlienSwingRange : MonoBehaviour
{
    public BruteAlien bruteAlien;

    // delete later
    public bool reached = false;
    public bool attack;
    public int index;
    public int counter = 0;
    //

    public Rigidbody2D playerRb;

    public float knockback;
    [SerializeField] private AudioClip swingSound;
    private bool soundPlaying = false;
    public IEnumerator PlaySound(AudioClip clip, Transform enemy, bool isAmbient)
    {
        soundPlaying = true;
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

        soundPlaying = false;
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attack = bruteAlien.attacking;
        index = bruteAlien.i;

        if(bruteAlien.i == 3)
        {
            StartCoroutine(PlaySound(swingSound, transform, false));
        }
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        counter++;
        if (bruteAlien.attacking && bruteAlien.i == 3)
        {
            reached = true;

            if (collider.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<PlayerHealthScript>().Hit(9);

                //knockback
                if (bruteAlien.facingRight)
                {
                    knockback = Mathf.Abs(knockback);
                }
                else
                {
                    knockback = -(Mathf.Abs(knockback));
                }

                playerRb.AddForce(new Vector2(knockback, knockback), ForceMode2D.Impulse);

                //playerRb.velocity = new Vector2(knockback, Mathf.Abs(knockback));
            }
        }
    }

}

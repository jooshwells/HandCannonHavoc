using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BombScript : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    
    private GameObject player;
    private float dir;
    private bool isExploding = false;
    private GameObject instatiator;
    private Rigidbody2D rb;

    public IEnumerator PlaySound(AudioClip clip, Transform enemy)
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
        yield return null;
    }

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }

    public void setDir(float d) { dir = d; }

    public void DefineInstantiator(GameObject inst)
    {
        instatiator = inst;
    }
    public void Launch()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(12 * dir, 16);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.tag);

        if (!collision.gameObject.CompareTag(instatiator.tag)) 
        {
            if (!isExploding)
            {
                isExploding = true;
                gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                StartCoroutine(Explode());

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Trigger triggering");
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
            Vector2 dir = (transform.position - player.transform.position).normalized;
            //Debug.Log("Attempting to Force <" + (12*-dir.x) + "," + (16*-dir.y) + "> to " + rb.gameObject.ToString());
            rb.gameObject.GetComponent<PlayerControllerMk2>().KnockBack(new Vector2(12 * -dir.x, 16 * dir.y), 0.2f, 15);
        }
    }

    IEnumerator Explode()
    {
        Animator anim = gameObject.GetComponent<Animator>();

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //anim.SetBool("isExploding", true);
        anim.Play("Exploding");
        float explosionDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;

        yield return new WaitForSeconds(explosionDuration);
        //GetComponent<AudioSource>().Play();

        StartCoroutine(PlaySound(GetComponent<AudioSource>().clip, transform));

        Destroy(gameObject);
        isExploding=false;
    }
}

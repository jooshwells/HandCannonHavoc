using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BombArrow : MonoBehaviour
{
    private GameObject instantiator;
    private float attackDamage = 15f;
    private bool isExploding = false;
    [SerializeField] private AudioClip explode;
    public IEnumerator PlaySound(AudioClip clip)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create new GameObject
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add AudioSource
        aSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        aSource.clip = clip;
        aSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        aSource.Play();
        Destroy(tempGO, clip.length);
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySound(GetComponent<AudioSource>().clip));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInstantiator(GameObject inst)
    {
        instantiator = inst;
    }
    public void SetAttackDamage(float damage)
    {
        attackDamage = damage;
    }
    
    private bool explodingStarted = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (explodingStarted) { return; }
        // Health Update Event for Enemies
        if (collision.CompareTag("Enemy"))
        {
        
            if (!isExploding)
            {
                isExploding = true;

                collision.gameObject.GetComponent<EnemyHealthScript>().UpdateHealth(attackDamage);
                StartCoroutine(Explode());
            }
        }
        else if (!(collision.CompareTag(instantiator.tag))) 
        {
            StartCoroutine(Explode());
        }
        
    }

    IEnumerator Explode()
    {
        explodingStarted = true;
        Animator anim = gameObject.GetComponent<Animator>();

        StartCoroutine(PlaySound(explode));

        Debug.Log("Multiplying scale by 2");
        transform.localScale *= 2;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //anim.SetBool("isExploding", true);
        anim.Play("Exploding");
        float explosionDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        
        
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
        isExploding=false;
    }
}

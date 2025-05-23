using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGboomy : MonoBehaviour
{
    private GameObject instantiator;
    private float attackDamage = 50f;
    private bool isExploding = false;
    [SerializeField] private AudioClip boom;

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collided with: " + collision.gameObject.tag);

        if (!collision.gameObject.CompareTag(instantiator.tag)) 
        {
            if (!isExploding)
            {
                isExploding = true;
                //work on knockback later
                // gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
                // gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                // gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                StartCoroutine(Explode());

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // work on collision knockback for enemies
        // Debug.Log("Trigger triggering");
        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
        //     Vector2 dir = (transform.position - player.transform.position).normalized;
        //     Debug.Log("Attempting to Force <" + (12*-dir.x) + "," + (16*-dir.y) + "> to " + rb.gameObject.ToString());
        //     rb.gameObject.GetComponent<PlayerController>().KnockBack(new Vector2(12 * -dir.x, 16 * dir.y), 0.2f);
        // }


        // Health Update Event for Enemies
        if (collision.CompareTag("Enemy"))
        {
            GameObject par = null;

            if (!isExploding)
            {
                isExploding = true;
                StartCoroutine(Explode());

            }
            if (collision.gameObject.transform.parent != null)
            {
                par = ((collision.gameObject).transform.parent).gameObject; // this gets the parent using transform
            }

            if(par != null)
            {
                par.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // if we have a parent, check its children for a script to update the health
            } 
            else
            {
                collision.gameObject.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // else just check the collision objects children for the script
            }
            
        }
        else if (!(collision.CompareTag(instantiator.tag))) {
            {
                if (!isExploding)
                {
                isExploding = true;
                StartCoroutine(Explode());

                }
            }
        }
    }

    IEnumerator Explode()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        StartCoroutine(PlaySound(boom));
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //anim.SetBool("isExploding", true);
        transform.localScale = transform.localScale * 2;
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

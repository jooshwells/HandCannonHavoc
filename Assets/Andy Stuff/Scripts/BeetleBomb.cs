using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BeetleBomb : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    
    private GameObject player;
    private float dir;
    private bool isExploding = false;
    private GameObject instatiator;
    private Rigidbody2D rb;

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

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     //Debug.Log("Trigger triggering");
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
    //         Vector2 dir = (transform.position - player.transform.position).normalized;
    //         rb.gameObject.GetComponent<PlayerControllerMk2>().KnockBack(new Vector2(100 * -dir.x, 500 * dir.y), 0.2f, 100);
    //     }
    // }

    private void ApplyExplosionDamage()
    {
        float explosionRadius = 3f; // Adjust radius size if needed
        int explosionDamage = 50;   // Adjust damage if needed

        // Find all colliders within explosion range
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Player"))
            {
                // Apply damage to the player
                hit.GetComponent<PlayerHealthScript>().Hit(explosionDamage);

                // Apply knockback to the player
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                hit.GetComponent<PlayerControllerMk2>().KnockBack(new Vector2(200 * dir.x, 300 * dir.y), 0.2f, 50);
            }
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
        GetComponent<AudioSource>().Play();
        Destroy(gameObject);
        isExploding=false;
    }
}

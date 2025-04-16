using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArrow : MonoBehaviour
{
    private GameObject instantiator;
    private float attackDamage = 15f;
    private bool isExploding = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
                // gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
                // gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                // gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                StartCoroutine(Explode());

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        


        // Health Update Event for Enemies
        if (collision.CompareTag("Enemy"))
        {
        
            if (!isExploding)
            {
                StartCoroutine(Explode());

            }
            collision.gameObject.GetComponent<EnemyHealthScript>().UpdateHealth(attackDamage);
            StartCoroutine(Explode());
        }

        else if (!(collision.CompareTag(instantiator.tag))) 
            {
                StartCoroutine(Explode());
            }
        
    }

    IEnumerator Explode()
    {
        isExploding = true;
        Animator anim = gameObject.GetComponent<Animator>();

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

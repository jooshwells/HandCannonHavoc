using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    private GameObject instantiator;
    private float attackDamage;
    [SerializeField] int ricochetCount = 2;
    private Rigidbody2D rb; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update() { }

    public void SetInstantiator(GameObject inst)
    {
        instantiator = inst;
    }

    public void SetAttackDamage(float damage)
    {
        attackDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Health Update Event for Enemies
        if (collision.CompareTag("Enemy"))
        {
            GameObject par = null;

            // Basically this is checking whether the object we are colliding with has a parent
            // I (Josh) have some enemies with child object colliders, so essentially just make sure
            // your HealthSystem is only one object down in the hierarchy and this should work fiiine.
            // (Check my Crocogator prefab for how I did it)
            if (collision.gameObject.transform.parent != null)
            {
                par = ((collision.gameObject).transform.parent).gameObject; // This gets the parent using transform
            }

            if(par != null)
            {
                par.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // If we have a parent, check its children for a script to update the health
            } 
            else
            {
                collision.gameObject.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // Else just check the collision object's children for the script
            }

            Destroy(gameObject); // Destroy the bullet after hitting the enemy
        }
        else if (!(collision.CompareTag(instantiator.tag)))
        {
            if (ricochetCount > 0)
            {
                // Weird raycast calculation, (don't ask me I have no idea)
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity.normalized, 1f);

                if (hit.collider != null)
            {
                
            Vector2 collisionNormal = hit.normal;

            // give the bounce a small variation in angle
            float angleVariation = Random.Range(-15f, 15f);

            // bounce bullet with angle variation
            Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, collisionNormal);

            // adjust velocity using bounce 
            reflectedVelocity = RotateVector(reflectedVelocity, angleVariation);
            rb.velocity = reflectedVelocity;

            ricochetCount--; 
        }
            }
            else
            {
                // no more bounce
                Destroy(gameObject);
            }
        }
    }
    
    // helps with random bounce angle
    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;  // Convert angle to radians
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        float newX = cos * vector.x - sin * vector.y;
        float newY = sin * vector.x + cos * vector.y;
        return new Vector2(newX, newY);
    }

}
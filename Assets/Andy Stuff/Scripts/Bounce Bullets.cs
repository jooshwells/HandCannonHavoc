using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private GameObject instantiator;
    private float attackDamage;
    private int ricochetCount = 2;
    private Rigidbody2D rb;

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
                par = ((collision.gameObject).transform.parent).gameObject; // this gets the parent using transform
            }

            if(par != null)
            {
                par.GetComponentInChildren<HealthScript>().UpdateHealth(attackDamage); // if we have a parent, check its children for a script to update the health
            } 
            else
            {
                collision.gameObject.GetComponentInChildren<HealthScript>().UpdateHealth(attackDamage); // else just check the collision objects children for the script
            }

            Destroy(gameObject); // destroy the bullet
        }

        else if (!(collision.CompareTag(instantiator.tag))) 
        {
            {
                if (ricochetCount > 0)
            {
                Ricochet(collision); 
            }
            else
            {
                Destroy(gameObject); 
            }
            }  
        }
    }
    // private void Ricochet(Collider2D collision)
    // {
    //     // Reflect the velocity based on the collision normal (surface hit)
    //     //Vector2 reflection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
    //    // rb.velocity = reflection.normalized * rb.velocity.magnitude; // Maintain the bullet's speed but change direction

    //     ricochetCount--;
    // }
    private void Ricochet(Collider2D collision)
    {
        // Calculate the surface normal by using the direction from the bullet to the collider
        Vector2 normal = (collision.transform.position - transform.position).normalized;

        // Reflect the velocity based on the normal of the collision surface
        Vector2 reflection = Vector2.Reflect(rb.velocity, normal);
        rb.velocity = reflection.normalized * rb.velocity.magnitude; // Reflect the velocity while maintaining speed

        ricochetCount--; // Decrease the ricochet count
    }
}

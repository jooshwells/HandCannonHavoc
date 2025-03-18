using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

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
                // Calculate raycast hit
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity.normalized, 1f);

                if (hit.collider != null)
                {
                    Vector2 closestPoint = collision.ClosestPoint(transform.position);  // get hit location on collider
                    Vector2 normal = (transform.position - (Vector3)closestPoint).normalized;   // get normal angle for boucning

                    Vector2 oldVelocity = rb.velocity; // used for flipping sprite
                    Vector2 newVelocity = Vector2.Reflect(rb.velocity, normal);     // reflect velocity
                    Debug.Log($"Velocity before ricochet: {oldVelocity}");
                    rb.velocity = newVelocity;
                    Debug.Log($"Velocity after ricochet: {newVelocity}");
                    


                    UpdateSprite(newVelocity, oldVelocity);
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
    
    private void UpdateSprite(Vector2 newVelocity, Vector2 oldVelocity)
    {
        Vector3 newScale = transform.localScale;
        // Flip horizontally when bouncing left/right
        if (Mathf.Sign(newVelocity.x) != Mathf.Sign(oldVelocity.x))        {
            newScale.x *= -1;
        }

        // // Flip vertically when bouncing up/down
        // if ((velocity.y > 0 && newScale.y < 0) || (velocity.y < 0 && newScale.y > 0))
        // {
        //     newScale.y *= -1;
        // }
        // if (velocity.sqrMagnitude > 0) // Avoid rotation when velocity is zero
        // {
        //     float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg; // Calculate the angle
        //     transform.rotation = Quaternion.Euler(0f, 0f, angle); // Apply the rotation
        // }

        transform.localScale = newScale;
    }

}
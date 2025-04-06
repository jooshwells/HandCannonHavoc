using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    private GameObject instantiator;
    private float attackDamage;
    [SerializeField] float pullRadius = 5f;
    [SerializeField] float pullForce = 10f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, pullRadius);

            foreach (var enemy in enemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 direction = (transform.position - enemy.transform.position);
                        float distance = direction.magnitude;

                        if (distance == 0) continue;

                        float t = 1f - (distance / pullRadius); // closer = higher t
                        t = Mathf.Clamp01(t);
                        t = Mathf.Pow(t, 0.5f); // soften falloff – makes edge pull stronger

                        float forceMultiplier = Mathf.Lerp(0.5f, 1.5f, t); // min/max pull strength

                        Vector2 force = direction.normalized * pullForce * forceMultiplier;
                        rb.AddForce(force, ForceMode2D.Force);
                    }
                }
            }
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
                par.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // if we have a parent, check its children for a script to update the health
            } 
            else
            {
                collision.gameObject.GetComponentInChildren<EnemyHealthScript>().UpdateHealth(attackDamage); // else just check the collision objects children for the script
            }

                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Collider2D>().enabled = false;
        }

        else if (!(collision.CompareTag(instantiator.tag))) {
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}

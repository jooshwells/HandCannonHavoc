using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CloudyJit : MonoBehaviour
{
    public bool attacking = false;

    public Transform target;
    public Rigidbody2D rb;
    AIPath path;
    public float detectRange = 10;
    public float knockback = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        path = GetComponent<AIPath>(); 
        path.enabled = false;
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (target = null){
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        //}

        if (attacking) //if attacking
        {
            if (outofAttackingRange(detectRange)) //if further than 30 units
            {
                attacking = false;
                path.enabled = false;
            }
        }
        else //if not attacking
        {
            if (inSpottedRange(detectRange)) //if within 10 units
            {
                attacking = true;
                path.enabled = true;
            }
        }
    }

    bool inSpottedRange(float range)
    {
        return (Vector2.Distance(rb.position, target.position) < range); //start attacking
    }

    bool outofAttackingRange(float range)
    {
        return (Vector2.Distance(rb.position, target.position) > range*3); //start attacking

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(10);
            //knockback
            if (transform.localScale.x < 0)
            {
                knockback = Mathf.Abs(knockback);
            }
            else
            {
                knockback = -(Mathf.Abs(knockback));
            }

            target.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, knockback), ForceMode2D.Impulse);
        }
    }
}

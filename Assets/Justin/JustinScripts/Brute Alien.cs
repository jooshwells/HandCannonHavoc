using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Net;
using System;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;

public class BruteAlien: MonoBehaviour
{
    //
    SpriteRenderer sprite;
    public Sprite[] idleFrames;
    public Sprite[] movingFrames;
    float timeFrame = 0f;
    [NonSerialized] public int i = 0;

    [NonSerialized] public bool facingRight = false;

    [NonSerialized] public bool attacking = false;
    public float attackingRange;

    //

    public Transform target;
    public Rigidbody2D targetRb;

    public float speed = 200f;
    public float maxVelocity = 8f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        InvokeRepeating("UpdatePath", 0f, .1f);

        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!attacking)
        {
            if (inRange(attackingRange/1.5f)) //start attacking
            {
                i = 0;
                timeFrame = Time.time + 0.25f;
                sprite.sprite = movingFrames[i];
                attacking = true;
            }
            else //idle animation
            {
                attacking = false;
                if (Time.time > timeFrame)
                {
                    i++;
                    i = i % 2;
                    timeFrame = Time.time + 0.5f;
                }
                sprite.sprite = idleFrames[i];
            }
        }
        if(attacking)
        {
            if (!inRange(attackingRange)) //stop attacking
            {
                i = 0;
                attacking = false;
            }

            if (Time.time > timeFrame) //attacking animation
            {
                i++;
                i = i % 7;
                timeFrame = Time.time + 0.1f;
            }
            sprite.sprite = movingFrames[i];
        }
    }

    void UpdatePath()
    {
        if (Vector2.Distance(rb.position, target.position) >= attackingRange) return;

        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {/*
        if (playerLeft() && rb.velocity.x > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x - 0.5f, rb.velocity.y);
        }
        if (!playerLeft() && rb.velocity.x < 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x + 0.5f, rb.velocity.y);
        }
     */

        if (!attacking)
        {           
            if (rb.velocity.x > 0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x - 0.5f, rb.velocity.y);
            }
            if (rb.velocity.x < -0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x + 0.5f, rb.velocity.y);

            }
            if(-0.1 <= rb.velocity.x && rb.velocity.x <= 0.1)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            
            
        }

        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        // position of current waypoint minus our current position
        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float direction;
        if (playerLeft()) // player to the left
        {
            direction = 0.2f;
        }
        else { //player to the right
            direction = -0.2f;
        }

        if(Mathf.Abs(rb.velocity.x) < maxVelocity)
        {
            /*
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force*2);
            */

            rb.velocity = new Vector2(rb.velocity.x + direction, rb.velocity.y);
        }


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-5.8083f, 5.8083f, 1f);
            facingRight = true;
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(5.8083f, 5.8083f, 1f);
            facingRight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(25);

            //
            float knockback = 1f;

            if (!playerLeft())
            {
                knockback = -1f;
            }

            targetRb.AddForce(new Vector2(knockback, knockback), ForceMode2D.Impulse);
        }
    }

    bool inRange(float range)
    {
        return (Vector2.Distance(rb.position, target.position) < range); //start attacking
    }

    bool playerLeft()
    {
        return (targetRb.position.x - rb.position.x > 0);
    }
}

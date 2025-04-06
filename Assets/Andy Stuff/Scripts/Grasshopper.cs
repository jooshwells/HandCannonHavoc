using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Grasshopper : MonoBehaviour
{

    private Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;
    [Header("Jumping Stuff")]
    [SerializeField] private float jumpPower = 50f;
    [SerializeField] private float jumpInterval = 1f;
    [SerializeField] private float jumpDistance = 30f;

    private float lastJumpTimer = 0f;



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
        InvokeRepeating("UpdatePath", 0f, .1f);

    }
    void UpdatePath()
    {
        if (Vector2.Distance(rb.position, target.position) >= 30f) return;

        if (Time.time - lastJumpTimer >= jumpInterval)  // Check if enough time has passed for the next jump
    {
        Jump();
        lastJumpTimer = Time.time;  // Update last jump time
    }
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
    {
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
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(1);
        }
        
    }

    
    // rescrap jump for dragonfly dashing at palyer
        void Jump()
    {
        Vector2 directionToPlayer = (target.position - transform.position).normalized;
        Vector2 horizontalForce = directionToPlayer * speed * 0.5f;
        rb.AddForce(new Vector2(horizontalForce.x, jumpPower), ForceMode2D.Impulse);
    }
}

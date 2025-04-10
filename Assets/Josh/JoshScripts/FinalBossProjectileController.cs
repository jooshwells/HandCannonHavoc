using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossProjectileController : MonoBehaviour
{
    private Transform target;
    private Transform boss;
    private Transform gfx;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public int attackDamage = 25;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        gfx = GetComponentInChildren<Transform>();
        boss = GameObject.FindGameObjectWithTag("FinalBoss").transform;
        if (GameObject.FindGameObjectWithTag("Player") != null)
            target = GameObject.FindGameObjectWithTag("Player").transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

    }



    void UpdatePath()
    {
        if (Vector2.Distance(rb.position, target.position) >= 30f) return;

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
    private bool startedInvoking = false;
    private GameObject player;
    private void Update()
    {

        if (!startedInvoking && player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Debug.Log("Player Active");
                target = player.transform;
                InvokeRepeating("UpdatePath", 0f, .1f);
                startedInvoking = true;
            }
        }

        if (target == null) Destroy(gameObject); // get rid of extra projectiles when final boss dies

        Vector3 targetPos = target.transform.position + new Vector3(0, 0.1f, 0);
        Vector2 direction = (targetPos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle+180f);
        //if (player.transform.localScale.x > 0)
        //{
        //    transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
        //    //vertFlip(transform.position, angle);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //    //vertFlip(transform.position, angle);
        //}
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

        //if (rb.velocity.x >= 0.01f)
        //{
        //    enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        //}
        //else if (rb.velocity.x <= -0.01f)
        //{
        //    enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        //}
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Bullets")) != 0)
        {
            target = boss;
        } else if (collision.CompareTag("FinalBoss") && target.CompareTag("FinalBoss"))
        {
            collision.gameObject.GetComponent<EnemyHealthScript>().UpdateHealth(attackDamage);
            Destroy(gameObject);
        } else if (collision.CompareTag("Player") && target.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(attackDamage);
            Destroy(gameObject);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BigSpider : MonoBehaviour
{

    private Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;
    public Transform spawnPoint;
    public GameObject spiderlingPrefab;
    public int numSpiderlings=3;
    public float spawnRadius=1.5f;
    private EnemyHealthScript healthScript;
    private bool triggeredSpawn = false;

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
        healthScript = GetComponent<EnemyHealthScript>();
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

    void Update()
    {
        if(healthScript.GetCurrentHP() <= 0 && triggeredSpawn ==false)
        {
            spawnSpiderlings();
            triggeredSpawn=true;
            Destroy(gameObject);
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

    private void spawnSpiderlings()
    {
    
        for (int i = 0; i < numSpiderlings; i++)
        {
            // spawn spiders randomly inside upper radius, prevents spawning in ground
            Vector2 spawnPosition = (Vector2)spawnPoint.position + new Vector2(Random.Range(-spawnRadius, spawnRadius), Random.Range(0f, spawnRadius));
            GameObject newSpiderling = Instantiate(spiderlingPrefab, spawnPosition, Quaternion.identity);

            // make spiders jump out on spawn
            Rigidbody2D rb = newSpiderling.GetComponent<Rigidbody2D>();
            Vector2 playerDir = ((target.position + new Vector3(0, 20, 0)) - (Vector3)spawnPosition).normalized;

            // add some randomness to jumps
            float randomOffsetX = Random.Range(-3f, 3f);  // Random horizontal offset
            float randomOffsetY = Random.Range(-3f, 3f);  // Random vertical offset
            playerDir.x += randomOffsetX;
            playerDir.y += randomOffsetY;
            playerDir.Normalize();

            // adjusting jump physics
            float horizontal = playerDir.x * 22f;
            float vertical = playerDir.y * 40f;
            rb.AddForce(new Vector2(horizontal, vertical), ForceMode2D.Impulse);
        }
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(25);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

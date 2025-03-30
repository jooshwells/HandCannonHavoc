// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Pathfinding;

// public class BeetleScript : MonoBehaviour
// {
//     private Transform target;

//     public float speed = 200f;
//     public float nextWaypointDistance = 3f;

//     public Transform enemyGFX;
//     private EnemyHealthScript healthScript;
//     Path path;
//     int currentWaypoint = 0;
//     bool isCharging = false;
//     bool isExploding = false;
//     bool reachedEndOfPath = false;

//     Seeker seeker;
//     Rigidbody2D rb;

//     [SerializeField] public float explosionRadius = 3f; // Explosion radius
//     [SerializeField] public int explosionDamage = 50; // Explosion damage

//     // Start is called before the first frame update
//     void Start()
//     {
//         target = GameObject.FindGameObjectWithTag("Player").transform;
//         seeker = GetComponent<Seeker>();
//         rb = GetComponent<Rigidbody2D>();
//         healthScript = GetComponent<EnemyHealthScript>();
//         InvokeRepeating("UpdatePath", 0f, .1f);
//     }

//     void UpdatePath()
//     {
//         if (Vector2.Distance(rb.position, target.position) >= 30f) return;

//         if (seeker.IsDone())
//             seeker.StartPath(rb.position, target.position, OnPathComplete);
//     }

//     void OnPathComplete(Path p)
//     {
//         if (!p.error)
//         {
//             path = p;
//             currentWaypoint = 0;
//         }
//     }
//     void Update()
//     {
//         if(healthScript.GetCurrentHP() <=0)
//         {
//             StartCoroutine(Explode());
//         }
//     }
//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         if (path == null)
//         {
//             return;
//         }

//         if (currentWaypoint >= path.vectorPath.Count)
//         {
//             reachedEndOfPath = true;
//             return;
//         }
//         else
//         {
//             reachedEndOfPath = false;
//         }

//         // Check for charging state based on health
//         if (healthScript.GetCurrentHP() <= healthScript.GetMaxHP() / 2f && !isCharging)
//         {
//             TriggerBombCharge();
//         }

//         // Position of current waypoint minus our current position
//         Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
//         Vector2 force = direction * speed * Time.deltaTime;
//         rb.AddForce(force);

//         float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

//         if (distance < nextWaypointDistance)
//         {
//             currentWaypoint++;
//         }

//         // Flip sprite based on velocity
//         if (rb.velocity.x >= 0.01f)
//         {
//             enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
//         }
//         else if (rb.velocity.x <= -0.01f)
//         {
//             enemyGFX.localScale = new Vector3(1f, 1f, 1f);
//         }
//     }

//     // Trigger bomb charging state
//     private void TriggerBombCharge()
//     {
//         if (!isCharging)
//         {
//             enemyGFX.GetComponent<SpriteRenderer>().color = Color.red; // Change color to red to signify charging
//             speed *= 2; // Double the speed in charging state
//             isCharging = true;
//         }
//     }

//     // Handle collision with the player
//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (isCharging)
//         {
//             if (collision.collider.CompareTag("Player"))
//             {
//                 // Trigger explosion if charging
//                 if (!isExploding)
//                 {
//                     isExploding = true;
//                     TriggerExplosion(); // Trigger explosion on collision with the player
//                 }
//                 float damage = healthScript.GetMaxHP();
//                 healthScript.UpdateHealth(damage); // Kill the beetle after collision (automatically triggers death animation)
//             }
//         }
//         else
//         {
//             // Regular damage when not charging
//             if (collision.collider.CompareTag("Player"))
//             {
//                 collision.gameObject.GetComponent<PlayerHealthScript>().Hit(25);
//             }
//         }
//     }

//     // Handle explosion
//     private void TriggerExplosion()
//     {
//         StartCoroutine(Explode());
//         Destroy(gameObject, 0.5f); // Destroy after a brief time (to let the explosion effect play out)
//     }
//         IEnumerator Explode()
//     {
//         Animator anim = gameObject.GetComponent<Animator>();

//         anim.Play("Exploding");
//         float explosionDuration = anim.GetCurrentAnimatorStateInfo(0).length;

//         yield return new WaitForSeconds(explosionDuration);
//         GetComponent<AudioSource>().Play();
//         Destroy(gameObject);
//         isExploding=false;
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BeetleScript : MonoBehaviour
{
    private Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;
    private EnemyHealthScript healthScript;
    Path path;
    int currentWaypoint = 0;
    bool isCharging = false;
    bool isExploding = false;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    [SerializeField] public float explosionRadius = 3f; // Explosion radius
    [SerializeField] public int explosionDamage = 50; // Explosion damage

    public GameObject bombPrefab; // Reference to the bomb prefab

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<EnemyHealthScript>();
        InvokeRepeating("UpdatePath", 0f, .1f);
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
        if(healthScript.GetCurrentHP() <= 0)
        {
            StartCoroutine(Explode());
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

        // Check for charging state based on health
        if (healthScript.GetCurrentHP() <= healthScript.GetMaxHP() / 2f && !isCharging)
        {
            TriggerBombCharge();
        }

        // Position of current waypoint minus our current position
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Flip sprite based on velocity
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // Trigger bomb charging state
    private void TriggerBombCharge()
    {
        if (!isCharging)
        {
            enemyGFX.GetComponent<SpriteRenderer>().color = Color.red; // Change color to red to signify charging
            speed *= 2; // Double the speed in charging state
            isCharging = true;
            SpawnBomb(); // Spawn the bomb when charging
        }
    }

    // Spawn a bomb on top of the beetle
    private void SpawnBomb()
    {
        if (bombPrefab != null)
        {
            // Instantiate bomb at beetle's position (on top of the beetle)
            Vector3 bombSpawnPosition = transform.position + new Vector3(0f, 1f, 0f); // Adjust Y position if needed
            GameObject bomb = Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity);

            // You can set the bomb's direction if necessary, or just trigger its explosion
            bomb.GetComponent<BombScript>().DefineInstantiator(gameObject); // Optional: link beetle as the instantiator
        }
    }

    // Handle collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging)
        {
            if (collision.collider.CompareTag("Player"))
            {
                // Trigger explosion if charging
                if (!isExploding)
                {
                    isExploding = true;
                    TriggerExplosion(); // Trigger explosion on collision with the player
                }
                float damage = healthScript.GetMaxHP();
                healthScript.UpdateHealth(damage); // Kill the beetle after collision (automatically triggers death animation)
            }
        }
        else
        {
            // Regular damage when not charging
            if (collision.collider.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerHealthScript>().Hit(25);
            }
        }
    }

    // Handle explosion
    private void TriggerExplosion()
    {
        StartCoroutine(Explode());
        Destroy(gameObject, 0.5f); // Destroy after a brief time (to let the explosion effect play out)
    }

    IEnumerator Explode()
    {
        Animator anim = gameObject.GetComponent<Animator>();

        anim.Play("Exploding");
        float explosionDuration = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(explosionDuration);
        GetComponent<AudioSource>().Play();
        Destroy(gameObject);
        isExploding = false;
    }
}
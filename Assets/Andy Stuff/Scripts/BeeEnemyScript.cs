// using Pathfinding;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BeeEnemyScript : MonoBehaviour
// {
//     [SerializeField] private float wakeUpDist = 5f; // Can be removed
//     private Transform target;
//     private Animator anim;
//     private Rigidbody2D rb;

//     Path path;
//     Seeker seeker;
//     int currentWaypoint = 0;
//     bool reachedEndOfPath = false;
//     bool started = false;

//     [SerializeField] private Transform enemyGFX;
//     [SerializeField] private float nextWaypointDistance = 3f;
//     [SerializeField] private float speed = 200f;
//     [SerializeField] private float attackRange = 11f;


//     [SerializeField] private float extraHeight = 3f;

//     [SerializeField] private GameObject Sting ;

//     private bool attacking;
//     [SerializeField] private float attackCooldown = 0.75f;

//     // Start is called before the first frame update
//     void Start()
//     {
//         target = GameObject.FindGameObjectWithTag("Player").transform;
//         anim = GetComponentInChildren<Animator>();
//         rb = GetComponent<Rigidbody2D>();
//         seeker = GetComponent<Seeker>();
//         RunPathing(); // Start pathing immediately
//     }

//     // Update is called once per frame
//     // void Update()
//     // {
//     //     bool isEnemyAboveAndInXRange = (rb.position.y >= target.position.y + 4.5f) &&
//     //                            (Mathf.Abs(rb.position.x - target.position.x) <= 0.5f);

//     //     if (!attacking && isEnemyAboveAndInXRange)
//     //     {
//     //         attacking = true;
//     //         StartCoroutine(Attack());
//     //     }
//     // }

//     // Update is called once per frame
//     void Update()
//     {

//         // calculate distance from player
//         float distanceToPlayer = Vector2.Distance(rb.position, target.position);

//         // check if player is in range to attack
//         if (!attacking && distanceToPlayer <= attackRange)
//         {
//             attacking = true;
//             StartCoroutine(Attack());
//         }
//     }

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

//         // position of current waypoint minus our current position
//         Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
//         Vector2 force = direction * speed * Time.deltaTime;
//         rb.AddForce(force);

//         float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

//         if (distance < nextWaypointDistance)
//         {
//             currentWaypoint++;
//         }

//         if (rb.velocity.x >= 0.01f)
//         {
//             enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
//         }
//         else if (rb.velocity.x <= -0.01f)
//         {
//             enemyGFX.localScale = new Vector3(1f, 1f, 1f);
//         }
//     }

//     IEnumerator Attack()
//     {
//         GameObject stingAttack = Instantiate(Sting, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
//         stingAttack.GetComponent<ProjectileScript>().SetInstantiator(gameObject);
//         yield return new WaitForSeconds(attackCooldown);
//         attacking = false;
//     }

//     void RunPathing()
//     {
//         InvokeRepeating("UpdatePath", 0f, 0.5f);
//     }

//     void UpdatePath()
//     {
//         if (Vector2.Distance(rb.position, (Vector2)target.position + new Vector2(0, extraHeight)) >= 30f) return;
//         // if (Vector2.Distance(rb.position, target.position) >= 30f) return;


//         if (seeker.IsDone())
//             seeker.StartPath(rb.position, (Vector2)target.position + new Vector2(0, extraHeight), OnPathComplete);
//             // seeker.StartPath(rb.position, target.position, OnPathComplete);

//     }

//     void OnPathComplete(Path p)
//     {
//         if (!p.error)
//         {
//             path = p;
//             currentWaypoint = 0;
//         }
//     }
// }
using Pathfinding;
using System.Collections;
using UnityEngine;

public class BeeEnemyScript : MonoBehaviour
{
    [SerializeField] private float wakeUpDist = 5f; // Distance to wake up and engage with the player
    private Transform target;
    private Transform beehive;
    private Animator anim;
    private Rigidbody2D rb;

    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool started = false;

    [SerializeField] private Transform enemyGFX;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float attackRange = 15f; // Attack range from the player
    [SerializeField] private float retreatDistance = 5f; // Distance to retreat from the player
    [SerializeField] private float stopRetreatingDistance = 10f; // Distance to stop retreating

    [SerializeField] private GameObject Sting;
    private bool attacking;
    [SerializeField] private float attackCooldown = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        beehive = GameObject.FindGameObjectWithTag("BeeHive").transform;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        RunPathing();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(rb.position, target.position);
        float distanceToBeehive = Vector2.Distance(rb.position, beehive.position);

        // Retreat behavior: if the player is within retreatDistance, move towards the beehive
        if (distanceToPlayer <= retreatDistance)
        {
            target = beehive; // Start retreating towards the beehive
            RunPathing();
        }
        // Stop retreating if the player is far enough (stop retreating when the player is farther than stopRetreatingDistance)
        else if (distanceToPlayer >= stopRetreatingDistance)
        {
            target = beehive; // Keep retreating towards the beehive until it's sufficiently far
            RunPathing();
        }

        // Attack behavior: if the player is within attack range, attack
        if (!attacking && distanceToPlayer <= attackRange)
        {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Flip the enemy sprite based on movement direction
        if (rb.velocity.x >= 0.01f)
        {
            if (enemyGFX.localScale.x != -1f)
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            if (enemyGFX.localScale.x != 1f)
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    IEnumerator Attack()
    {
        GameObject stingAttack = Instantiate(Sting, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        stingAttack.GetComponent<ProjectileScript>().SetInstantiator(gameObject);
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }

    void RunPathing()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        // If the bee is sufficiently close to the target (either player or beehive), don't update path
        if (Vector2.Distance(rb.position, (Vector2)target.position + new Vector2(0, 3f)) >= 30f) return;

        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, (Vector2)target.position + new Vector2(0, 3f), OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
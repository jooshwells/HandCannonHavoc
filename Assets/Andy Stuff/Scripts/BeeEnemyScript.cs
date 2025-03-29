using Pathfinding;
using System.Collections;
using UnityEngine;

public class BeeEnemyScript : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f; // Distance to start attacking
    [SerializeField] private float retreatRange = 2f; // Distance to retreat
    private Transform target;
    private Rigidbody2D rb;

    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    [SerializeField] private Transform enemyGFX;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float speed = 200f;

    [SerializeField] private GameObject stingAttack;
    [SerializeField] private float attackCooldown = 2f;
    private bool attacking;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(target.position, transform.position);

        // Attack if within attack range
        if (!attacking && distanceToPlayer <= attackRange)
        {
            StartCoroutine(Attack());
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        float distanceToPlayer = Vector2.Distance(target.position, rb.position);

        // Retreat if the player is too close
        if (distanceToPlayer <= retreatRange)
        {
            RetreatFromPlayer();
        }
        else
        {
            MoveAlongPath();
        }
    }

    void MoveAlongPath()
    {
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
            currentWaypoint++;

        // Flip sprite direction
        if (rb.velocity.x >= 0.01f)
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        else if (rb.velocity.x <= -0.01f)
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
    }

    void RetreatFromPlayer()
    {
        Vector2 retreatDirection = (rb.position - (Vector2)target.position).normalized;
        Vector2 retreatTarget = rb.position + retreatDirection * 5f; // 5f can be adjusted for retreat distance

        // Start path to retreat target
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, retreatTarget, OnPathComplete);
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        GameObject stinger = Instantiate(stingAttack, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        stinger.GetComponent<ProjectileScript>().SetInstantiator(gameObject); // placeholder using trash fireball
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
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
}
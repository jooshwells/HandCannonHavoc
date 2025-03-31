using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemyScript : MonoBehaviour
{
    [SerializeField] private float wakeUpDist = 5f; // Can be removed
    private Transform target;
    private Transform hive;

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
    [SerializeField] private float attackRange = 11f;


    [SerializeField] private float extraHeight = 3f;

    [SerializeField] private GameObject Sting ;

    private bool attacking;
    [SerializeField] private float attackCooldown = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player")!=null)
        target = GameObject.FindGameObjectWithTag("Player").transform;    
        hive = GameObject.FindGameObjectWithTag("BeeHive").transform;    

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        RunPathing(); // Start pathing immediately
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                target = GameObject.FindGameObjectWithTag("Player").transform;

            if(target == null)
            {
                return;
            }
        }
        // calculate distance from player
        float distanceToPlayer = Vector2.Distance(rb.position, target.position);

        // check if player is in range to attack
        if (!attacking && distanceToPlayer <= attackRange)
        {
            attacking = true;
            StartCoroutine(Attack());
        }
    }

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
        if(target==null) return;
        //if (Vector2.Distance(rb.position, (Vector2)target.position + new Vector2(0, extraHeight)) >= 30f) return;

        if (Vector2.Distance(rb.position, (Vector2)target.position + new Vector2(0, extraHeight)) <= 5f)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, (Vector2)target.position + new Vector2(0, extraHeight), OnPathComplete);
        }
        else if(Vector2.Distance(rb.position, (Vector2)target.position + new Vector2(0, extraHeight)) >= 5f)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, (Vector2)hive.position, OnPathComplete);
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
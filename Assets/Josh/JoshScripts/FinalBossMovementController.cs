using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FinalBossMovementController : MonoBehaviour
{
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    [SerializeField] private Transform endLoc;    // above 50% health target
    [SerializeField] private Transform endLocTwo; // below 50% health target

    [SerializeField] private GameObject blockage;

    private Vector3 startingLocation;

    private Vector3 upper;
    private Vector3 lower;
    private float horizontalSpeed = 2.4f;
    private float verticalSpeed = 8f;

    private bool movingUp = true;
    private EnemyHealthScript hpScript;
    private Transform target;

    private bool faceRight = false;
    private bool faceLeft = false;

    //private float speed = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        target = endLoc;
        hpScript = GetComponent<EnemyHealthScript>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .1f);
    }

    void UpdatePath()
    {
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

        if(transform.position.y >= 17.85f && transform.position.x <= 25.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        } else
        {
            if ((rb.velocity.x >= 0.01f || faceLeft))
            {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            }
            else if ((rb.velocity.x <= -0.01f || faceRight))
            {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
        }


        
    }

    private bool targetUpdated = false;

    // Update is called once per frame
    void Update()
    {
        if(!targetUpdated && hpScript.GetHealthPerc() <= 0.5)
        {
            target = endLocTwo;
            targetUpdated = true;

            blockage.SetActive(false);

            Bounds updateBounds = new Bounds(new Vector3(9.2f, -4.82f, 0), new Vector3(10, 10, 1));
            GraphUpdateObject guo = new GraphUpdateObject(updateBounds);
            AstarPath.active.UpdateGraphs(guo);

        }
        //// Move horizontally at a constant speed
        //transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;

        //// Determine vertical target
        //Vector3 targetY = movingUp ? upper : lower;

        //// Move vertically toward target at a constant speed
        //Vector3 newPos = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY.y, transform.position.z), verticalSpeed * Time.deltaTime);
        //transform.position = newPos;

        //// Check if target reached, and switch direction
        //if (Mathf.Approximately(transform.position.y, targetY.y))
        //{
        //    movingUp = !movingUp;
        //    verticalSpeed = Random.Range(2f, 4f);
        //}


    }
}

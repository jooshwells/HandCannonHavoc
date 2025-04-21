using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Dragonfly : MonoBehaviour
{
    private Transform target;

    private Animator anim;
    private Rigidbody2D rb;

    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    [SerializeField] private Transform enemyGFX;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float speed = 400f;

    [Header("Dash Stuff")]
    [SerializeField] private float dashRange = 10f;
    [SerializeField] private float dashVariation = 2f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashForce = 2f;

    private float lastDashTimer;

    public IEnumerator PlaySound(AudioClip clip, Transform enemy)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.parent = enemy;
        tempGO.transform.localPosition = Vector3.zero;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        aSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);

        aSource.spatialBlend = 1.0f;
        aSource.minDistance = 1f;
        aSource.maxDistance = 20f;
        aSource.rolloffMode = AudioRolloffMode.Linear;

        aSource.Play();
        Destroy(tempGO, clip.length);
        yield return new WaitForSeconds(clip.length);
        if (clip.Equals(ambient))
        {
            StartCoroutine(PlaySound(clip, transform));
        }
        yield return null;
    }


    [SerializeField] private AudioClip ambient;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySound(ambient, transform));

        if(GameObject.FindGameObjectWithTag("Player")!=null)
        target = GameObject.FindGameObjectWithTag("Player").transform;    
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
        if (distance <= dashRange && Time.time >= lastDashTimer + dashCooldown) // if in dash range and not on cooldown
        {
            Dash();
            //return; // return to not start pathing
        }

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
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(10);
        }
        
    }
    void RunPathing()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {   

        if(target==null) return;
        
        float playerDist = Vector2.Distance(rb.position, target.position);

        //pathing
        if (playerDist >= 20f) return;

        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete); // Always go straight
        }
    }
    void Dash()
    {
        if (Time.time <lastDashTimer +dashCooldown) return;

        Vector2 directionToPlayer = (target.position - transform.position).normalized;

        // chance to miss dash slightly
        if (Random.value < 0.5f)
        {
            float heightOffset = Random.Range(-dashVariation, dashVariation);
            directionToPlayer = new Vector2(directionToPlayer.x, directionToPlayer.y + heightOffset).normalized;
        }

        rb.AddForce(directionToPlayer * dashForce, ForceMode2D.Impulse);
        lastDashTimer = Time.time;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, dashRange);
        Gizmos.color = Color.blue; 
        Gizmos.DrawWireSphere(transform.position, dashVariation);
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireSphere(transform.position, 20f);
    }
}
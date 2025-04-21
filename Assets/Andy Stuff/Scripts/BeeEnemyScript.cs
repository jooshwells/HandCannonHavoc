using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemyScript : MonoBehaviour
{
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
    [SerializeField] private float aggroRange = 11f;


    [SerializeField] private float hiveHoverRange = 3f;


    [SerializeField] private float extraHeight = 3f;

    [SerializeField] private GameObject Sting ;

    private bool attacking;
    [SerializeField]private int stingerMaxAmmo = 3;
    private int stingerAmmo;
    [SerializeField] private float attackCooldown = 0.75f;
    [SerializeField] private AudioClip ambient;
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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySound(ambient, transform));
        if (GameObject.FindGameObjectWithTag("Player")!=null)
        target = GameObject.FindGameObjectWithTag("Player").transform;    
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        stingerAmmo = stingerMaxAmmo;

        
        GameObject[] hives = GameObject.FindGameObjectsWithTag("BeeHive");
        float closestDist = Mathf.Infinity;
        Transform closestHive = null;

        foreach (GameObject h in hives)
        {
            float dist = Vector2.Distance(transform.position, h.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestHive = h.transform;
            }
        }
        hive = closestHive;

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

        // check if player is in range to attack and has stinger to shoot
        if (!attacking && distanceToPlayer <= attackRange && stingerAmmo>0)
        {
            StartCoroutine(Attack());
        }
        // reload stinger if bee is close to hive
        if(Vector2.Distance(rb.position, hive.position)<hiveHoverRange)
        {
            stingerAmmo=stingerMaxAmmo; // reload stingers
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
        attacking = true;
        stingerAmmo-=1; //use 1 ammo
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

        float playerDist = Vector2.Distance(rb.position, (Vector2)target.position + new Vector2(0, extraHeight));
        float hiveDist = Vector2.Distance(rb.position, (Vector2)hive.position);
        
        // no stinger, return to hive to "relaod"
        if (stingerAmmo==0)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, (Vector2)hive.position, OnPathComplete);
            return;
        }

        if (playerDist <= aggroRange)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, (Vector2)target.position + new Vector2(0, extraHeight), OnPathComplete);
        }

        else if (hiveDist > 2f)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, (Vector2)hive.position, OnPathComplete);
        }
        else if(hiveDist < 2f)
        {
            rb.velocity =Vector2.zero;
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hiveHoverRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
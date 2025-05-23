using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Net;
using System;
using UnityEngine.Rendering;

public class BruteAlien: MonoBehaviour
{
    //
    public SpriteRenderer sprite;
    public Sprite[] idleFrames;
    public Sprite[] movingFrames;
    float timeFrame = 0f;
    [NonSerialized] public int i = 0;

    [NonSerialized] public bool facingRight = false;

    public bool attacking = false;
    public float attackingRange = 8f;
    bool spotted = false;


    //

    public Transform target;
    public Rigidbody2D targetRb;

    public float speed = 200f;
    public float maxVelocity = 8f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    EnemyHealthScript EnemyHealthScript;

    Seeker seeker;
    public Rigidbody2D rb;
    Animator animator;
    [SerializeField] private AudioClip scream;

    public IEnumerator PlaySound(AudioClip clip, Transform enemy, bool isAmbient)
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

        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {

        //target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        InvokeRepeating("UpdatePath", 0f, .1f);

        sprite = GetComponent<SpriteRenderer>();
        EnemyHealthScript = GetComponent<EnemyHealthScript>();
        animator = GetComponent<Animator>();

    }
    public bool checker = false;
    private void Update()
    {
        if (inRange(attackingRange))
        {
            attacking = true;
        }

        if (EnemyHealthScript.GetCurrentHP() < EnemyHealthScript.GetMaxHP())
            attackingRange = 100f;

        if (EnemyHealthScript.GetCurrentHP() <= 0)
        {
            if (animator.enabled == false)
            {
                animator.enabled = true;
            }
        }

        if (!attacking)
        {
            if (inRange(attackingRange)) //start attacking
            {

                i = 0;
                timeFrame = Time.time + 0.25f;
                sprite.sprite = movingFrames[i];
                attacking = true;
            }
            else //idle animation
            {
                attacking = false;
                if (Time.time > timeFrame)
                {
                    i++;
                    i = i % 2;
                    timeFrame = Time.time + 0.5f;
                }
                sprite.sprite = idleFrames[i];
            }
        }
        //if(attacking && Mathf.Abs(rb.velocity.x) > 0.2)
        if (attacking)
        {
            /*if (!inRange(attackingRange)) //stop attacking
            {
                i = 0;
                attacking = false;
            }*/

            if (Time.time > timeFrame) //attacking animation
            {
                checker = true;

                i++;
                i = i % 7;
                timeFrame = Time.time + 0.1f;
            }
            sprite.sprite = movingFrames[i];
        }
    }

    private bool startedPathfinding = false;

    void UpdatePath()
    {
        if (Vector2.Distance(rb.position, target.position) >= attackingRange || !(EnemyHealthScript.GetCurrentHP() < EnemyHealthScript.GetMaxHP())) return;

        if(!startedPathfinding)
        {
            StartCoroutine(PlaySound(scream, transform, false));
            startedPathfinding = true;
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
    {/*
        if (playerLeft() && rb.velocity.x > 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x - 0.5f, rb.velocity.y);
        }
        if (!playerLeft() && rb.velocity.x < 0.1f)
        {
            rb.velocity = new Vector2(rb.velocity.x + 0.5f, rb.velocity.y);
        }
     */

        if (!attacking)
        {           
            if (rb.velocity.x > 0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x - 0.5f, rb.velocity.y);
            }
            if (rb.velocity.x < -0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x + 0.5f, rb.velocity.y);

            }
            if(-0.1 <= rb.velocity.x && rb.velocity.x <= 0.1)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
            
            
        }

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
        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float direction;
        if (playerLeft()) // player to the left
        {
            direction = 0.2f;
        }
        else { //player to the right
            direction = -0.2f;
        }

        if(Mathf.Abs(rb.velocity.x) <= maxVelocity)
        {
            /*
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force*2);
            */

            rb.velocity = new Vector2(rb.velocity.x + direction, rb.velocity.y);
        }
        if(Mathf.Abs(rb.velocity.x) > maxVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x - direction, rb.velocity.y);
        }


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-5.8083f, 5.8083f, 1f);
            facingRight = true;
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(5.8083f, 5.8083f, 1f);
            facingRight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //collision.gameObject.GetComponent<PlayerHealthScript>().Hit(25);

            //
            float knockback = 2f;

            if (!playerLeft())
            {
                knockback = -2f;
            }

            targetRb.AddForce(new Vector2(knockback, knockback), ForceMode2D.Impulse);
        }
    }

    bool inRange(float range)
    {
        return (Vector2.Distance(rb.position, target.position) < range || EnemyHealthScript.GetCurrentHP() < EnemyHealthScript.GetMaxHP()); //start attacking
    }

    bool playerLeft()
    {
        return (targetRb.position.x - rb.position.x > 0);
    }
}

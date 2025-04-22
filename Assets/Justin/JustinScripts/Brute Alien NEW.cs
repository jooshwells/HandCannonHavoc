using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Net;
using System;
using UnityEngine.Rendering;

public class BruteAlienNEW : MonoBehaviour
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

    AIPath path;
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
        path = GetComponent<AIPath>();
        path.enabled = false;

    }
    public bool checker = false;
    private void Update()
    {
//        if (EnemyHealthScript.GetCurrentHP() < EnemyHealthScript.GetMaxHP())
//            attackingRange = 100f;

        if (EnemyHealthScript.GetCurrentHP() <= 0)
        {
            if (animator.enabled == false)
            {
                animator.enabled = true;
            }
        }

        if(attacking)
        {
            path.enabled = true;
        }

        if (!attacking)
        {
            if (inRange()) //start attacking
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
    // Update is called once per frame
    void FixedUpdate()
    {
        if(inRange())
        {
            attacking = true;
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

    bool inRange()
    {
        return (Vector2.Distance(rb.position, target.position) < attackingRange || 
                EnemyHealthScript.GetCurrentHP() < EnemyHealthScript.GetMaxHP());
    }

    bool playerLeft()
    {
        return (targetRb.position.x - rb.position.x > 0);
    }
}

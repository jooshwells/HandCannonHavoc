using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.EventSystems;

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

    [SerializeField] public float speedMultiplier = 2.5f; 

    public GameObject bombPrefab; // Reference to the bomb prefab

    // For Animations
    //private bool isWalking = false;
    private bool isAngry = false;
    private bool transitionFinished = false;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
       if(GameObject.FindGameObjectWithTag("Player")!=null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        healthScript = GetComponent<EnemyHealthScript>();
        InvokeRepeating("UpdatePath", 0f, .1f);
    }

    void UpdatePath()
    {
        if(target == null) return;
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
        if (transitionFinished) Debug.Log("Yup");
        //if(rb.velocity.magnitude <= 0.1f && isWalking == true)
        //{
        //    isWalking = false;
        //} else if ()

        if(target == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                target = GameObject.FindGameObjectWithTag("Player").transform;

            if(target == null)
            {
                return;
            }
        }
        if(healthScript.GetCurrentHP() <= 0 && !isExploding)
        {
            if(isCharging)
            {
                isExploding = true;
                ApplyExplosionDamage();
                SpawnBomb();
            }
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
        if(isAngry && !transitionFinished) return;
        // Position of current waypoint minus our current position
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // flip sprite
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // enter charging state
    private void TriggerBombCharge()
    {
        if (!isCharging)
        {
            //enemyGFX.GetComponent<SpriteRenderer>().color = Color.red; // Change color to red to signify charging
            anim.SetBool("Angry", true);
            StartCoroutine(WaitForAnimationToFinish());
            isAngry = true;
            speed *= speedMultiplier;
            isCharging = true;
        }
    }

    IEnumerator WaitForAnimationToFinish()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Wait while the animation is still playing
        yield return new WaitForSeconds(stateInfo.length);
        transitionFinished = true;
    }

    // spawn invisble bomb to show explosion
    private void SpawnBomb()
    {
        if (bombPrefab != null)
        {
            Vector3 bombSpawnPosition = transform.position + new Vector3(0f, 0f, 0f); 
            GameObject bomb = Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity);
            bomb.GetComponent<BombScript>().DefineInstantiator(gameObject); 
        }
    }

    // Handle collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isCharging)
        {
            if (collision.collider.CompareTag("Player"))
            {
                if (!isExploding)
                {
                    SpawnBomb(); //plays explosion animation
                    ApplyExplosionDamage(); // handle collision of explosion
                    float damage = healthScript.GetMaxHP();
                    healthScript.UpdateHealth(damage); // kill beetle after explosion
                    isExploding = true;
                }
            }
        }
        else if(!isCharging)
        {
            // non charge damage
            if (collision.collider.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerHealthScript>().Hit(25);
            }
        }
    }
     private void ApplyExplosionDamage()
    {
        // Find all colliders within explosion range
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealthScript>().Hit(25); // normal hit damage

                // apply knockback and explosion damage
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                hit.GetComponent<PlayerControllerMk2>().KnockBack(new Vector2(22 * dir.x, 34 * dir.y), 0.2f, explosionDamage);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // test explosion radius
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
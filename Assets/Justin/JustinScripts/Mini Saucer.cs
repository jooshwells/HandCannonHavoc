using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class MiniSaucer : MonoBehaviour
{
    public SpriteRenderer spriterender;

    [SerializeField] private float wakeUpDist = 5f;
    private Transform target;
    //private Animator anim;
    private Rigidbody2D rb;

    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool started = false;

    [SerializeField] private Transform enemyGFX;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float speed = 400f;

    [SerializeField] private float extraHeight = 3f;

    [SerializeField] private GameObject goopAttack;

    private bool attacking = false;
    [SerializeField] private float attackCooldown = 2f;

    // Start is called before the first frame update
    void Start()
    {
        spriterender = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    public float X_Dist_Needed; 
    public float X_Dist_KeptAway; //should be smaller than X_Dist_Needed

    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            Idle();
        }

        bool isEnemyAboveAndInXRange = (rb.position.y >= target.position.y + 3f) &&
                                       (Mathf.Abs(rb.position.x - target.position.x) <= X_Dist_Needed); //&&
                               //(Mathf.Abs(rb.position.x - target.position.x) >= X_Dist_KeptAway);

        if(rb.position.y <= target.position.y + 3.5f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1f);
        }

        if (!attacking && isEnemyAboveAndInXRange)
        {
            attacking = true;
            StartCoroutine(Attack());
        }

        // If Player is within target distance
        if (!started && Vector2.Distance(target.position, gameObject.transform.position) <= wakeUpDist)
        {
            StartCoroutine(WakeUp());
            started = true;
        }

        transform.localScale = new Vector3(5.8908f, 5.8908f);
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
        GameObject curSlimeBall1 = Instantiate(goopAttack, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        curSlimeBall1.GetComponent<ProjectileScript>().SetInstantiator(gameObject);

        GameObject curSlimeBall2 = Instantiate(goopAttack, transform.Find("LaunchOrigin2").position, transform.Find("LaunchOrigin2").rotation);
        curSlimeBall2.GetComponent<ProjectileScript>().SetInstantiator(gameObject);

        GameObject curSlimeBall3 = Instantiate(goopAttack, transform.Find("LaunchOrigin3").position, transform.Find("LaunchOrigin3").rotation);
        curSlimeBall3.GetComponent<ProjectileScript>().SetInstantiator(gameObject);

        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }
    IEnumerator WakeUp()
    {
        //anim.SetBool("WakeUp", true);
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0);
        RunPathing();
    }

    void RunPathing()
    {
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (Vector2.Distance(rb.position, (Vector2)target.position + new Vector2(0, extraHeight)) >= 30f) return;

        if (seeker.IsDone())
            seeker.StartPath(rb.position, (Vector2)target.position + new Vector2(0, extraHeight), OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            //extraHeight += 0.5f;
        }
    }

    float lastTime_FlashingLights = 0;
    public Sprite[] flashingSprites;
    int flashingSpritesIndex = 0;
    void FlashingLights()
    {
        if (Time.time > lastTime_FlashingLights)
        {
            spriterender.sprite = flashingSprites[flashingSpritesIndex];
            flashingSpritesIndex = (flashingSpritesIndex + 1) % 3;
            lastTime_FlashingLights = Time.time + 0.15f;
        }
    }

    float lastTime_Idle = 0;
    float lastTime_Up = 0;
    float lastTime_Down = 0;
    bool up = true;
    void Idle()
    {
        FlashingLights();

        if (up && Time.time > lastTime_Up)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 0.1f);
            lastTime_Up = Time.time + 0.1f;
        }
        if (!up && Time.time > lastTime_Down)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.1f);
            lastTime_Down = Time.time + 0.1f;
        }

        if (Time.time > lastTime_Idle)
        {
            lastTime_Idle = Time.time + 1f;
            up = !up;
        }
    }
}

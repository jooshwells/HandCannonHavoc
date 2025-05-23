using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBagScript : MonoBehaviour
{

    [SerializeField] private float wakeUpDist = 5f;
    private Transform target;
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

    [SerializeField] private float extraHeight = 3f;

    [SerializeField] private GameObject goopAttack;

    private bool attacking;
    [SerializeField] private float attackCooldown = 0.75f;
    [SerializeField] private AudioClip ambient;
    [SerializeField] private AudioClip wakeUp;
    [SerializeField] private AudioClip fire;
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
        if(GameObject.FindGameObjectWithTag("Player")!=null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
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

        bool isEnemyAboveAndInXRange = (rb.position.y >= target.position.y + 4.5f) &&
                               (Mathf.Abs(rb.position.x - target.position.x) <= 0.5f);

        if (!attacking && isEnemyAboveAndInXRange)
        {
            attacking = true;
            StartCoroutine(Attack());
        }

        // If Player is within target distance
        if(!started && Vector2.Distance(target.position, gameObject.transform.position) <= wakeUpDist)
        {
            StartCoroutine(WakeUp());
            started = true;
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
        GameObject curSlimeBall = Instantiate(goopAttack, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        curSlimeBall.GetComponent<ProjectileScript>().SetInstantiator(gameObject);
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }
    IEnumerator WakeUp()
    {
        anim.SetBool("WakeUp", true);
        StartCoroutine(PlaySound(wakeUp, transform));
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        RunPathing();
        StartCoroutine(PlaySound(ambient, transform));
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
}

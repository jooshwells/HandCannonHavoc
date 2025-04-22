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

    private bool startmonodone = false;
    [SerializeField] List<AudioClip> ambientSoldierSounds;
    [SerializeField] private AudioClip startMonologue;
    [SerializeField] private AudioClip middleHealthMono;

    [SerializeField] private GameObject firingLoc;

    public void Pause()
    {
        startmonodone = false;
    }
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
        yield return new WaitForSeconds(clip.length+1);
        if (isAmbient)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            StartCoroutine(PlaySound(ambientSoldierSounds[Random.Range(0, 5)], transform, true));
        }
        if (clip.Equals(startMonologue) || clip.Equals(middleHealthMono)) { 
            startmonodone = true; 
            if(clip.Equals(startMonologue))
                firingLoc.SetActive(true);
            else
                firingLoc.GetComponent<FinalBossFiringLocScript>().UnPause();

        }
        yield return null;
    }

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
        if (!startmonodone) return;
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
        if (!startmonodone) return;
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
            transform.GetChild(0).localScale = new Vector3(-1f, 1f, 1f);
        } else
        {
            if ((rb.velocity.x >= 0.01f))
            {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
                transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
            }
            else if ((rb.velocity.x <= -0.01f))
            {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
                transform.GetChild(0).localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    private bool targetUpdated = false;
    [SerializeField] private GameObject player;
    private bool running = false;

    // Update is called once per frame
    void Update()
    {
        if(!running && player.activeSelf && !startmonodone)
        {
            running = true;
            StartCoroutine(PlaySound(startMonologue, transform, false));
        }
        if (!startmonodone) return;
        if (!targetUpdated && hpScript.GetHealthPerc() <= 0.5)
        {
            startmonodone = false;
            firingLoc.GetComponent<FinalBossFiringLocScript>().Pause();
            StartCoroutine(PlaySound(middleHealthMono, transform, false));


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

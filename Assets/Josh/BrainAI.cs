using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BrainAI : MonoBehaviour
{

    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public Transform enemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update

    public IEnumerator PlaySound(AudioClip clip, Vector3 position)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create new GameObject
        tempGO.transform.position = position; // set position where sound should come from

        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add AudioSource
        aSource.clip = clip;
        aSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        aSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);

        // Set up 3D sound settings
        aSource.spatialBlend = 1.0f; // 0 = 2D, 1 = 3D
        aSource.minDistance = 1f;    // full volume within this range
        aSource.maxDistance = 20f;   // silent beyond this range
        aSource.rolloffMode = AudioRolloffMode.Linear; // or Logarithmic

        aSource.Play();
        Destroy(tempGO, clip.length);
        yield return null;
    }
    void Start()
    {
        StartCoroutine(PlaySound(GetComponent<AudioSource>().clip, transform.position));
        GetComponent<EnemyHealthScript>().SetHealth(5);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .1f);
        
    }
    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
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
        } else
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
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(25);
        }
    }
}

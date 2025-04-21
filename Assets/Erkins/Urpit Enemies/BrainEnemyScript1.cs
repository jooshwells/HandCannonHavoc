using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainEnemyScript2 : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform target;
    [SerializeField] private float targetDist = 10f;
    [SerializeField] private float fireRate = 1.2f;

    // won't start attacking at player until player gets within some distance,
    // but once aware it will keep attacking until defeated.
    private bool awareOfPlayer = false;
    private bool startedFiring = false;
    private bool isFiring = false;

    [SerializeField] GameObject brain;
    public Transform fireLoc;

    public Animator anim;

    private List<GameObject> brainList = new List<GameObject>();
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        fireLoc = transform.Find("FireLoc");

        anim = GetComponent<Animator>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if(target == null && player != null)
        {
            target = player.transform;       
        }
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if(player != null)
            {
                target = player.transform;
            } else
            {
                return;
            }
        }
        //if (brainList.Count > 5)
        //{
        //    GameObject temp = brainList[0];
        //    brainList.RemoveAt(0);
        //    Destroy(temp);
        //}

        if(!awareOfPlayer && Vector2.Distance(rb.position, target.position) <= targetDist)
        {
            awareOfPlayer = true;
        }

        if (awareOfPlayer && !startedFiring)
        {
            startedFiring = true;
            InvokeRepeating("FireBrains", 0f, fireRate);
        }

        if(awareOfPlayer) FaceTarget();
    }

    void FireBrains()
    { 
        if(!isFiring) StartCoroutine(FireAnim());
    }

    IEnumerator FireAnim()
    {
        isFiring = true;
        anim.Play("Fire State");
        float animationDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration);
        if(GetComponent<EnemyHealthScript>().GetCurrentHP() > 0)Instantiate(brain, fireLoc.position, fireLoc.rotation);
        isFiring = false;
    }

    void FaceTarget()
    {
        if (target == null) return;

        Vector3 scale = transform.localScale;
        if ((target.position.x > transform.position.x && scale.x > 0) ||
            (target.position.x < transform.position.x && scale.x < 0))
        {
            scale.x *= -1; // Flip horizontally
            transform.localScale = scale;
        }
    }

}

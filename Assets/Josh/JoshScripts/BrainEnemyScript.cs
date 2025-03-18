using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainEnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    [SerializeField] private float targetDist = 8f;
    [SerializeField] private float fireRate = 1.2f;

    // won't start attacking at player until player gets within some distance,
    // but once aware it will keep attacking until defeated.
    private bool awareOfPlayer = false;
    private bool startedFiring = false;
    private bool isFiring = false;

    [SerializeField] GameObject brain;
    private Transform fireLoc;

    private Animator anim;

    private List<GameObject> brainList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        fireLoc = transform.Find("FireLoc");

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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
        Instantiate(brain, fireLoc.position, fireLoc.rotation);
        isFiring = false;
    }
}

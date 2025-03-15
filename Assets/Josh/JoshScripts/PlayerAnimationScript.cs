using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private bool isRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool currentlyRunning = Mathf.Abs(rb.velocity.x) >= 0.01f;

        if (currentlyRunning != isRunning)
        {
            anim.SetBool("Running", currentlyRunning);
            isRunning = currentlyRunning;
        }
    }
}

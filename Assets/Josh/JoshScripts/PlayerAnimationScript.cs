using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerControllerMk2 pc;

    private bool isRunning = false;
    private bool isGrounded = true;
    private bool isOnWall = false;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerControllerMk2>();
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

        if(isGrounded && !pc.IsGrounded())
        {
            isGrounded = false;
            anim.SetBool("Grounded", isGrounded);
        } else if (!isGrounded && pc.IsGrounded())
        {
            isGrounded = true;
            anim.SetBool("Grounded", isGrounded);
        }

        if (!isOnWall && pc.IsOnWall() && !pc.IsGrounded() && Input.GetAxisRaw("Horizontal") != 0)
        {
            isOnWall = true;
            anim.SetBool("WallSliding", isOnWall);
        } else if (isOnWall && (!pc.IsOnWall() || pc.IsGrounded() || Input.GetAxisRaw("Horizontal") == 0))
        {
            isOnWall= false;
            anim.SetBool("WallSliding", isOnWall);
        }
    }
}

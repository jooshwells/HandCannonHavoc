using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerController : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip ouch;


    public float wallSlide = 0.95f;

    private float horizontal;
    private float vertical;
    private float speed = 8f;
    public float jumpPower = 16f;
    private bool isFacingRight = true;

    private bool wallSliding = false;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDir;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(2f, 12f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float invulnerabilityTime;

    private bool isKnockedBack = false;
    private float knockBackTimer = 0f;
    private float invulTimer = 0f;
    private float grappleReleaseDelay = 0.5f;

    private bool grappling = false;

    public void SetGrapple(bool newG)
    {
        if (newG == false)
        {
            Jump();
        }
        grappling = newG;
    }

    public void KnockBack(Vector2 force, float timer)
    {
        gameObject.GetComponent<AudioSource>().clip = ouch;
        gameObject.GetComponent<AudioSource>().Play();
        rb.velocity = force;
        isKnockedBack = true;
        knockBackTimer = timer;
        invulTimer = invulnerabilityTime; 
    }


    // Update is called once per frame
    void Update()
    {
        if(isKnockedBack)
        {
            knockBackTimer -= Time.deltaTime;

            if(knockBackTimer <= 0f)
            {
                isKnockedBack = false;
            }

            return;
        }

        if (!grappling)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || grappling))
        {
            Jump();
        }

        if (!isWallJumping && Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    public bool getIsFacingRight()
    {
        return isFacingRight;
    }

    private void Jump()
    {
        gameObject.GetComponent<AudioSource>().clip = jump;
        gameObject.GetComponent<AudioSource>().Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }


    private void FixedUpdate()
    {
        if (!isKnockedBack && !grappling)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            //if (rb.velocity.x <= 8.1f)
            //{
            //    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            //} else
            //{
            //    rb.velocity = new Vector2(horizontal * rb.velocity.x, rb.velocity.y);
            //}
        } 
    }

    private void Flip()
    {
        // If moving left and facing right, or facing left and moving right
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight; // invert bool
            Vector3 localScale = transform.localScale; // init local scale
            localScale.x *= -1f; // flip sprite on x-axis
            transform.localScale = localScale; // update local scale
        }
    }

    private void WallSlide()
    {
        if (IsOnWall() && !IsGrounded() && horizontal != 0f)
        {
            wallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            wallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallJumping)
        {
            wallSliding = false;
            return;
        }

        if (wallSliding)
        {
            isWallJumping = false;
            wallJumpingDir = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;


            // Flip sprite if needed
            if (transform.localScale.x != wallJumpingDir)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;

            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool IsOnWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
}
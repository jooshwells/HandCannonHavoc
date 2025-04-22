using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
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
    private TrailRenderer trail;
    private bool canDash = true;
    private bool isDashing;
    private Vector2 dashDirection;
    private float dashTime;
    private float dashCooldownTime;
    [SerializeField] public float dashSpeed = 20f;
    [SerializeField] public float dashDuration = 0.2f;
    [SerializeField] public float dashCooldown = 0.5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>(); // Get the TrailRenderer component
        trail.enabled = false; // Disable it initially
    }
    // Update is called once per frame
    void Update()
    {
        if (isDashing) 
        { 
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x*5, jumpPower);
        }

        if (!isWallJumping && Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x*5, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();
        HandleDash();

        if (!isWallJumping)
        {
            Flip();
        }

    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dashDirection == Vector2.zero)
            {
                //dashDirection = Vector2.right * transform.localScale.x; // Default forward dash
                dashDirection = new Vector2(transform.localScale.x, 0f);
            }

            StartCoroutine(startDash());
        }
    }
    IEnumerator startDash()
    {
        isDashing = true;
        canDash = false;
        dashTime = Time.time + dashDuration;
        dashCooldownTime = Time.time + dashCooldown;
        rb.gravityScale = 0; // Disable gravity for the dash

        trail.enabled = true; // Enable trail effect


        while (Time.time < dashTime)
        {
            rb.velocity = dashDirection.normalized * dashSpeed;
            yield return null;
        }
        rb.gravityScale = 4; // Restore gravity
        isDashing = false;
        trail.enabled = false; // Disable trail after dash
        yield return new WaitForSeconds(dashCooldown);
        while(!IsGrounded())
        {
            yield return null;
        }
        canDash = true;
    }

        //if (grapplingHook != null && grapplingHook.getConnection())
        //{
        //    ApplySwingingForces();
        //}
        //else
        //{
        //    rb.gravityScale = 4f; // Reset gravity when not swinging
        //}

        //// Handle releasing the grapple
        //if (Input.GetKeyDown(KeyCode.Space) && grapplingHook.getConnection())
        //{
        //    ReleaseGrapple();
        //}
    

    public bool getIsFacingRight()
    {
        return isFacingRight;
    }

    //private void ReleaseGrapple()
    //{
    //    if (GetComponent<SpringJoint2D>() != null)
    //    {
    //        Destroy(GetComponent<SpringJoint2D>());
    //    }

    //    // Project velocity in the direction the player was already moving
    //    Vector2 forwardVelocity = rb.velocity.normalized * Mathf.Max(rb.velocity.magnitude, 10f);
    //    rb.velocity = forwardVelocity;

    //    rb.gravityScale = 4f; // Reset gravity to normal
    //    grapplingHook.setConnection(false);
    //}



    //private void ApplySwingingForces()
    //{
    //    float input = Input.GetAxisRaw("Horizontal");
    //    if (input == 0) return; // No input = No force applied

    //    Vector2 grapplePoint = grapplingHook.transform.position;
    //    Vector2 playerToGrapple = grapplePoint - (Vector2)transform.position;

    //    // Calculate the tangent (perpendicular) direction to the rope
    //    Vector2 swingDirection = Vector2.Perpendicular(playerToGrapple).normalized;

    //    // Determine which perpendicular direction to use based on player input
    //    swingDirection *= Mathf.Sign(input);

    //    // Prevent pulling toward grapple point by ensuring only tangential force is applied
    //    if (Vector2.Dot(rb.velocity, playerToGrapple.normalized) > -0.2f)
    //    {
    //        float swingForce = 10f; // Adjust for smooth swinging (lower value prevents erratic movement)
    //        rb.AddForce(swingDirection * swingForce, ForceMode2D.Force);
    //    }

    //    rb.gravityScale = 1.5f; // Lower gravity during swinging for more natural arcs
    //}




    private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsOnWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
}
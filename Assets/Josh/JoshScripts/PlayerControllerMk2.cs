using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PlayerControllerMk2 : MonoBehaviour
{

    // General Player Components
    private Rigidbody2D rb;

    // Components of child objects of player
    private Transform groundCheck;
    private Transform wallCheck;

    // LayerMasks for Ground and Wall
    private LayerMask wallLayer;
    private LayerMask groundLayer;
    private LayerMask dmgLayer;

    // Input directions
    private float horizontal;
    private float vertical;
    private bool isFacingRight = true;

    // Movement parameters
    [Header("Movement Parameters")]
    [SerializeField] private float jumpPower = 16f;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float accel = 32.5f;
    [SerializeField] private float decel = 25f;
    private float coyoteTime = 0.1f; // max time since grounded where jumping is allowed
    float coyoteTimer = 0f; // counter for keeping track of how long since last grounded


    // Wall Stuff
    private bool wallSliding = false;
    private bool isWallJumping;
    private float wallJumpingDir;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(2f, 12f);

    // Knockback parameters to handle getting hit
    private bool isKnockedBack = false;
    private float knockBackTimer = 0f;
    private float invulTimer = 0f;

    // Gonna be honest I don't know what this does,
    // but it was in my old script so here it is again.
    [SerializeField] private float invulnerabilityTime; 

    // Sound effects for jumping and getting hit respectively
    [Header("Sound Effects")]
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip ouch;
    [SerializeField] AudioClip dash;

    // Grappling Hook values
    private bool grappling = false;
    private bool justGrappled = false;

    // Dash Stuff
    private TrailRenderer trail;
    private bool canDash = true;
    private bool isDashing;
    private Vector2 dashDirection;
    private float dashTime;
    private float dashCooldownTime;
    [SerializeField] public float dashSpeed = 20f;
    [SerializeField] public float dashDuration = 0.2f;
    [SerializeField] public float dashCooldown = 0.5f;
    private bool dashOn = false; // Made by Josh - Used with Ability Controller

    public IEnumerator PlaySound(AudioClip clip)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create new GameObject
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add AudioSource
        aSource.volume = PlayerPrefs.GetFloat("SFXVolume");
        aSource.clip = clip;
        aSource.pitch = UnityEngine.Random.Range(1.15f, 1.25f);
        aSource.Play();
        Destroy(tempGO, clip.length);
        yield return null;
    }

    public void SetDashOn(bool d)
    {
        dashOn = d;
    }
    public float GetJumpPower()
    {
        return jumpPower;
    }

    // Returns if the player is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, 0.2f, dmgLayer);
    }

    // Returns if the player is on a wall
    public bool IsOnWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    public void KnockBack(Vector2 force, float timer, int damage)
    {
        GetComponent<PlayerHealthScript>().Hit(damage);

        // Play getting hit sound
        gameObject.GetComponent<AudioSource>().clip = ouch;
        gameObject.GetComponent<AudioSource>().Play();
        
        // Set velocity to be the force of the knockback,
        // and indicate the player is currently knocked back.
        rb.velocity = force;
        isKnockedBack = true;
        
        // Initialize knockBackTimer, also set invulTimer to the
        // serialized field for invulnerability time (still no idea
        // what this does)
        knockBackTimer = timer;
        invulTimer = invulnerabilityTime;
    }

    private void Jump()
    {
        // Play jump sound effect
        StartCoroutine(PlaySound(jump));

        // Set velocity to allow jump
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private void WallSlide()
    {
        // If on a wall, not grounded, and giving some horizontal input
        if (IsOnWall() && !IsGrounded() && horizontal != 0f)
        {
            // Enter wall sliding state, slow down y-velocity to indicate sliding.
            wallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            wallSliding = false;
        }
    }

    // This method is kind of a mess, probably could do with a rewrite.
    private void WallJump()
    {
        // If we are already wall jumping, ensure wall sliding
        // is set to false and get out of the method.
        if (isWallJumping)
        {
            wallSliding = false;
            return;
        }

        // If we are wall sliding
        if (wallSliding)
        {
            // Ensure state is not wallJumping
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

    private void Flip()
    {
        // If moving left and facing right, or facing left and moving right
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;             // invert bool
            Vector3 localScale = transform.localScale;  // init local scale
            localScale.x *= -1f;                        // flip sprite on x-axis
            transform.localScale = localScale;          // update local scale
        }
    }

    public void AbilControlResetGrapple()
    {
        justGrappled = false;
        grappling = false;
    }

    public void SetGrapple(bool newG)
    {
        if (!newG) // When detaching from the grapple
        {
            justGrappled = true;

            // Only apply a slight boost if player isn't already moving upwards
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * 5f; // Tweak this boost value if needed
            }
        }
        grappling = newG;
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

            StartCoroutine(StartDash());
        }
    }

    IEnumerator StartDash()
    {
        StartCoroutine(PlaySound(dash));

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
        while (!IsGrounded())
        {
            yield return null;
        }
        canDash = true;
    }

    private void ApplyMovement()
    {
        float targetSpeed = horizontal * speed;
        float speedDifference = targetSpeed - rb.velocity.x;

        // Choose acceleration/deceleration depending on input and current state
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : decel;
        if (!IsGrounded()) accelRate *= 0.5f;

        // If changing direction, increase acceleration for snappier turns
        if (Mathf.Sign(horizontal) != Mathf.Sign(rb.velocity.x) && horizontal != 0)
        {
            accelRate *= 2f; // Increase acceleration when reversing direction
        }
        // Apply acceleration smoothly
        float movement = Mathf.Sign(speedDifference) * Mathf.Min(Mathf.Abs(speedDifference), accelRate * Time.fixedDeltaTime);
        rb.velocity = new Vector2(rb.velocity.x + movement, rb.velocity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        // Initialize components
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        wallCheck = transform.Find("WallCheck");
        groundCheck = transform.Find("GroundCheck");
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");
        dmgLayer = LayerMask.GetMask("Damage");
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = IsGrounded();
        if (grounded)
        {
            coyoteTimer = coyoteTime;
        } else
        {
            coyoteTimer -= Time.deltaTime;
        }

        // If player is currently knocked back,
        // count down the timer until it's finished
        // at which point exit knocked back state.
        if (isKnockedBack)
        {
            knockBackTimer -= Time.deltaTime;

            if (knockBackTimer <= 0f)
            {
                isKnockedBack = false;
            }

            return; // Don''t handle movement when knocked back
        }

        // If not grappling, get directional input
        
        
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        

        // If pressing space and on the ground or on a grappling hook, jump.
        if (Input.GetKeyDown(KeyCode.Space) && (coyoteTimer > 0f || grappling))
        {
            Jump();
            coyoteTimer = 0;
        }

        // This changes the y velocity upon release of space to change jump height
        // based on how long you hold space for.
        if (!isWallJumping && Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();
        if(dashOn) HandleDash();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (!isKnockedBack && !grappling && (!justGrappled || rb.velocity.y <= 0))
        {
            ApplyMovement();
        }

        
        if (justGrappled && rb.velocity.y <= 0)
        {
            justGrappled = false;
        }
    }
}

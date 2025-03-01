using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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

    // Input directions
    private float horizontal;
    private float vertical;
    private bool isFacingRight = true;

    // Movement parameters
    [Header("Movement Parameters")]
    [SerializeField] private float jumpPower = 16f;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float accel = 0.01f;
    [SerializeField] private float decel = 4f;
    
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

    // Grappling Hook values
    private bool grappling = false;
    private bool justGrappled = false;
    
    // Returns if the player is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Returns if the player is on a wall
    public bool IsOnWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    public void KnockBack(Vector2 force, float timer)
    {
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
        gameObject.GetComponent<AudioSource>().clip = jump;
        gameObject.GetComponent<AudioSource>().Play();
        
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

    public void SetGrapple(bool newG)
    {
        if (newG == false)
        {
            Jump();
            justGrappled = true;
        }
        grappling = newG;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize components
        rb = GetComponent<Rigidbody2D>();
        wallCheck = transform.Find("WallCheck");
        groundCheck = transform.Find("GroundCheck");
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
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
        if (!grappling)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        // If pressing space and on the ground or on a grappling hook, jump.
        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || grappling))
        {
            Jump();
        }

        // This changes the y velocity upon release of space to change jump height
        // based on how long you hold space for.
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

    private void FixedUpdate()
    {
        // Only allow movement when not knocked back or grappling
        if(!isKnockedBack && !grappling && !justGrappled)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        } else if (IsGrounded())
        {
            justGrappled = false;
        }
    }
}

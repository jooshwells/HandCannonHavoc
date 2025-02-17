using System.Collections;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    private Rigidbody2D rb;
    private TrailRenderer trail;
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing;
    private Vector2 dashDirection;
    private float dashTime;
    private float dashCooldownTime;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>(); // Get the TrailRenderer component
        trail.enabled = false; // Disable it initially
    }

    void Update()
    {
        if (isDashing) return;
        if (IsGrounded())
            canDash = true; // Reset dash when grounded
        HandleMovement();
        HandleJump();
        HandleDash();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void HandleDash()
    {
        if (Time.time < dashCooldownTime || !canDash) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dashDirection == Vector2.zero)
            {
                dashDirection = Vector2.right * transform.localScale.x; // Default forward dash
            }

            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
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

        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}

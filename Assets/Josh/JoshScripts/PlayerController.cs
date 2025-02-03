using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float wallSlide = 0.95f;

    private float horizontal;
    private float speed = 8f;
    public float jumpPower = 16f;
    public float wallJumpPower = 16f;
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool wallJumped = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (IsGrounded())
        {
            isJumping = false;
            wallJumped = false;
        }
        if (!IsOnWall())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        } else if (IsOnWall() && !wallJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallSlide);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                wallJumped = true;
                if(isFacingRight) 
                { 
                    rb.velocity = new Vector2(rb.velocity.x * wallJumpPower, jumpPower);
                    Debug.Log("facing right");
                } 
                else
                {
                    rb.velocity = new Vector2(-wallJumpPower, jumpPower);
                    Debug.Log("facing LEFT");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;
        }
        
        Flip();
    }

    private void FixedUpdate()
    {
        
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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

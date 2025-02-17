using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    private copyController player;

    //motion
    public float jumpScale = 2.0f;
    private Rigidbody2D rb;

    //ability
    private bool highJumping = false;
    private bool parachuting = false;
    private float cdHighJump = 0;

    //function declarations

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<copyController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cdHighJump > 0)
        {
            cdHighJump--;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !onCooldown())
        {
            if (!(player.IsOnWall())) // highjump
            {
                rb.velocity = new Vector2(rb.velocity.x, player.jumpPower * jumpScale);
                highJumping = true;
                cdHighJump = 0; //cooldown tbd)
            }
        }

        if (isHighJumping())
        {
            rb.velocity = new Vector2((rb.velocity.x) / 4, rb.velocity.y); // jump almost straight up | need fix,
            if (getFalling()) //activate parachute
            {
                highJumping = false;
                parachuting = true;
            }
        }
        if (isParachuting())
        {
            //rb.gravityScale = 0.5f;
            rb.velocity = new Vector2(rb.velocity.x, -2f); // constant falling rather than gravity (which accelerates)
        }
        else
        {
            rb.gravityScale = 4; //default
        }
        if (player.IsGrounded() || player.IsOnWall())
        {
            parachuting = false;

        }
    } 


    // psuedocode for later

    /* if (!wallsliding)
     * {
     *      highjump and parachute activation:
     *      if (movement ability button pressed)
     *          propel player upwards like a high jump
     *          then, animation of parachute coming out
     *          parachuting = true
     * } 
     * 
     * if(parachuting)
     * {
     *      gravity of player(?) set to low to simulate parachuting
     *      (player can still move left and right while falling)
     * }
     * else
     * {
     *      gravity = (normal gravity)
     * }
     * 
     * if(grounded || wallsliding)
     * {
     *      parachuting = false
     * }
     */


    //getter functions
    public bool isHighJumping() {
        return highJumping;
    }

    public bool getFalling()
    {
        return (rb.velocity.y < 0);
    }

    public bool isParachuting()
    {
        return parachuting;
    }

    public bool onCooldown()
    {
        return (cdHighJump > 0);
    }
}

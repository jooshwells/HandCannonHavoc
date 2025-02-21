using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class HighJumpParachute : MonoBehaviour
{

    private copyController player;

    //motion
    public float jumpScale = 2.0f;
    private Rigidbody2D rb;

    //ability
    private bool highJumping = false;
    private bool parachuting = false;
    private bool parachutingToggleable = false;
    private float cdHighJump = 0;

    public int spriteFrame = 0;
    private bool spriteOpening = false;
    private bool spriteClosing = false;

    public GameObject spr;
    public SpriteRenderer render;
    public Sprite[] frames;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<copyController>();
        rb = GetComponent<Rigidbody2D>();
        spr.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (cdHighJump > 0)
        {
            cdHighJump--;
        }

        //smooth open
        if (spriteOpening && spriteFrame < 14*10 +1)
        {
            render.sprite = frames[spriteFrame/10];
            spriteFrame++;
        }

        if (spriteClosing && spriteFrame > 0)
        {
            render.sprite = frames[spriteFrame/10];
            spriteFrame--;
        }

        if(spriteClosing && spriteFrame == 0)
        {
            spriteClosing = false;
            spr.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && !onCooldown() && !isParachuting())
        {
            if (!(player.IsOnWall())) // highjump
            {
                rb.velocity = new Vector2(rb.velocity.x, player.jumpPower * jumpScale);
                highJumping = true;
                cdHighJump = 0; //cooldown tbd
            }
        }

        if (isHighJumping())
        {
            rb.velocity = new Vector2((rb.velocity.x) / 4, rb.velocity.y); // jump almost straight up | need fix, or remove.
            if (isFalling()) //activate parachute
            {
                highJumping = false;
                parachuting = true;
                parachutingToggleable = true;
            }
        } 

        if (isParachuting())
        {
            openSprite();

            //rb.gravityScale = 0.5f;
            rb.velocity = new Vector2(rb.velocity.x, -1.5f); // constant falling rather than gravity (which accelerates)
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isParachutingToggleable())
        {

            parachuting = !parachuting;

            if (parachuting)
            {
                openSprite();
            }
            else
            {
                closeSprite();
            }
        }

        if (player.IsGrounded() || player.IsOnWall())
        {
            parachuting = false;
            parachutingToggleable = false;
            closeSprite();
        }
    }

    //getter functions
    public bool isHighJumping() {
        return highJumping;
    }

    public bool isFalling()
    {
        return (rb.velocity.y < 0);
    }

    public bool isParachuting()
    {
        return parachuting;
    }

    public bool isParachutingToggleable()
    {
        return parachutingToggleable;
    }

    public bool onCooldown()
    {
        return (cdHighJump > 0);
    }

    public float getCooldown()
    {
        return cdHighJump;
    }

    public void openSprite()
    {
        spr.SetActive(true);
        spriteOpening = true;
        spriteClosing = false;
    }

    public void closeSprite()
    {
        spriteOpening = false;
        spriteClosing = true;
    }

}
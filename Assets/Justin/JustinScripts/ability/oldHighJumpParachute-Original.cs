using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class HighJumpParachute : MonoBehaviour
{

    //private PlayerController player;
    private copyController player;
    private genCooldown cooldown;

    //motion
    public float jumpScale = 2.0f;
    private Rigidbody2D rb;

    //ability
    private bool highJumping = false;
    private bool parachuting = false;
    private bool parachutingToggleable = false;
    private float cdHighJump = 0;

    public int spriteFrame = 0;
    public int spriteFrameBoost = 0;
    private bool spriteOpeningParachute = false;
    private bool spriteClosingParachute = false;
    private bool spriteOpeningBoost = false;
    private bool spriteClosingBoost = false;

    public GameObject sprParachute;
    public GameObject sprBoost;
    public SpriteRenderer render;
    public SpriteRenderer render2;

  //public Sprite[] frames;
    public Sprite[] framesParachute;
    public Sprite[] framesBoost;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = GetComponent<genCooldown>();
        cooldown.setCooldown(2f);
        //player = GetComponent<PlayerController>();
        player = GetComponent<copyController>();
        rb = GetComponent<Rigidbody2D>();
        sprParachute.SetActive(false);
        sprBoost.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if (cdHighJump > 0)
        {
            cdHighJump--;
        }

        //smooth open
        /*
        if (spriteOpening && spriteFrame < 14*10 +1)
        {
            render.sprite = frames[spriteFrame/10];
            spriteFrame++;
        }
        */

        if (spriteOpeningParachute && spriteFrame < 7 * 15 + 1)
        {
            render.sprite = framesParachute[spriteFrame / 15];
            spriteFrame++;
        }

        if (spriteClosingParachute && spriteFrame > 0)
        {
            render.sprite = framesParachute[spriteFrame/15];
            spriteFrame--;
        }

        if(spriteClosingParachute && spriteFrame == 0)
        {
            spriteClosingParachute = false;
            sprParachute.SetActive(false);
        }



        if (spriteOpeningBoost && spriteFrameBoost < 6 * 20 + 1)
        {
            render2.sprite = framesBoost[spriteFrameBoost / 20];
            spriteFrameBoost++;
        }
        else
        {
            closeSpriteBoost();
        }

        if (spriteClosingBoost && spriteFrameBoost > 0)
        {
            render2.sprite = framesBoost[spriteFrameBoost / 20];
            spriteFrameBoost--;
        }

        if (spriteClosingBoost && spriteFrameBoost == 0)
        {
            spriteClosingBoost = false;
            sprBoost.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !onCooldown() && !isParachuting()) 
        {
            if (cooldown.isActive())
            {
                // message player "ability on cooldown" or something
            }
            else
            {
                if (!(player.IsOnWall())) // highjump
                {
                    if(!parachutingToggleable)
                        openSpriteBoost();
                    rb.velocity = new Vector2(rb.velocity.x, player.jumpPower * jumpScale);
                    cooldown.enable();
                    highJumping = true;
                    cdHighJump = 0; //cooldown tbd
                }
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
            openSpriteParachute();

            //rb.gravityScale = 0.5f;
            rb.velocity = new Vector2(rb.velocity.x, -1.5f); // constant falling rather than gravity (which accelerates)
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isParachutingToggleable())
        {

            parachuting = !parachuting;

            if (parachuting)
            {
                openSpriteParachute();
            }
            else
            {
                closeSpriteParachute();
            }
        }

        if (player.IsGrounded() || player.IsOnWall()) 
        {
            parachuting = false;
            parachutingToggleable = false;
            closeSpriteParachute();
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

    public void openSpriteParachute()
    {
        sprParachute.SetActive(true);
        spriteOpeningParachute = true;
        spriteClosingParachute = false;
    }

    public void closeSpriteParachute()
    {
        spriteOpeningParachute = false;
        spriteClosingParachute = true;
    }

    public void openSpriteBoost()
    {
        sprBoost.transform.position = player.transform.position;
        sprBoost.SetActive(true);
        spriteOpeningBoost = true;
        spriteClosingBoost = false;
    }

    public void closeSpriteBoost()
    {
        spriteOpeningBoost = false;
        spriteClosingBoost = true;
        sprBoost.SetActive(false);
    }
}
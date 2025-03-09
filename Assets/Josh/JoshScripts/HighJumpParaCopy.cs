using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class HighJumpParaCopy : MonoBehaviour
{

    //private PlayerController player;
    private PlayerControllerMk2 player;
    private GenCooldownCopy cooldown;

    //motion
    [SerializeField] private float jumpScale = 2.0f;
    private Rigidbody2D rb;

    //ability
    private bool highJumping = false;
    private bool parachuting = false;
    private bool parachutingToggleable = false;
    private float cdHighJump = 0;

    [SerializeField] private int spriteFrame = 0;
    [SerializeField] private int spriteFrameBoost = 0;
    private bool spriteOpeningParachute = false;
    private bool spriteClosingParachute = false;
    private bool spriteOpeningBoost = false;
    private bool spriteClosingBoost = false;

    [SerializeField] private GameObject sprParachute;
    [SerializeField] private GameObject sprBoost;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private SpriteRenderer render2;

    //public Sprite[] frames;
    public Sprite[] framesParachute;
    public Sprite[] framesBoost;


    // Start is called before the first frame update
    void Start()
    {
        cooldown = GetComponent<GenCooldownCopy>();
        cooldown.setCooldown(2f);
        //player = GetComponent<PlayerController>();
        player = transform.parent.gameObject.GetComponent<PlayerControllerMk2>();
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
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
            render.sprite = framesParachute[spriteFrame / 15];
            spriteFrame--;
        }

        if (spriteClosingParachute && spriteFrame == 0)
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
                    if (!parachutingToggleable)
                        openSpriteBoost();
                    //openSpriteBoost();
                    rb.velocity = new Vector2(rb.velocity.x, player.GetJumpPower() * jumpScale);
                    cooldown.enable();
                    highJumping = true;
                    cdHighJump = 0; //cooldown tbd
                }
            }
        }
        if (isHighJumping())
        {
            //rb.velocity = new Vector2((rb.velocity.x) / 4, rb.velocity.y); // jump almost straight up | need fix, or remove.
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
    public bool isHighJumping()
    {
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
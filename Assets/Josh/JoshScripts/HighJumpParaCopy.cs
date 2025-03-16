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
    private bool highJumped = false;
    private bool parachuting = false;
    private bool parachutingToggleable = false;
    private float cdHighJump = 0;

    private int spriteFrame = 0;
    private int spriteFrameBoost = 0;
    private bool spriteOpeningParachute = false;
    private bool spriteClosingParachute = false;
    private bool spriteOpeningBoost = false;
    float paraTimeCurrent;
    float paraTimeEnd;
    float paraTimeCurrentBoost;
    float frameInterval = 0.05f;

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
        cooldown.setCooldown(3f); //SET COOLDOWN HERE
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

        if (spriteOpeningParachute && Time.time >= paraTimeCurrent && spriteFrame < 8)
        {
            render.sprite = framesParachute[spriteFrame];
            paraTimeCurrent = Time.time + frameInterval;
            spriteFrame++;
        }

        if (spriteClosingParachute && Time.time >= paraTimeCurrent && spriteFrame > 0)
        {
            spriteFrame--;
            render.sprite = framesParachute[spriteFrame];
            paraTimeCurrent = Time.time + frameInterval;
        }

        if (spriteClosingParachute && spriteFrame <= 0)
        {
            spriteClosingParachute = false;
            sprParachute.SetActive(false);
        }



        if (spriteOpeningBoost && Time.time >= paraTimeCurrentBoost && spriteFrameBoost < 7)
        {
            render2.sprite = framesBoost[spriteFrameBoost];
            paraTimeCurrentBoost = Time.time + frameInterval;
            spriteFrameBoost++;
        }
        else if (spriteFrameBoost == 7)
        {
            spriteOpeningBoost = false;
            sprBoost.SetActive(false);
        }




        if (Input.GetKeyDown(KeyCode.LeftShift) && !onCooldown() && !isParachuting() && player.IsGrounded())
        {
            if (cooldown.isActive())
            {
                // message player "ability on cooldown" or something
            }
            else
            {
                //if (!(player.IsOnWall())) // highjump
                {
                    if (!parachutingToggleable)
                        openSpriteBoost();
                    //openSpriteBoost();
                    rb.velocity = new Vector2(rb.velocity.x, player.GetJumpPower() * jumpScale);
                    //cooldown.enable();
                    highJumped = true;
                    cdHighJump = 0; //cooldown tbd
                }
            }
        }

        //commented out the parachute opening by default
        /*
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
        */

        //make it so anytime the player is falling they can parachute
        if (isFalling()) //activate parachute
        {
            parachutingToggleable = true;
        }

        if (isParachuting())
        {
            openSpriteParachute();

            //rb.gravityScale = 0.5f;
            rb.velocity = new Vector2(rb.velocity.x, -1.5f); // constant falling rather than gravity (which accelerates)
        }

        float timetoOpen = (8 * frameInterval) - (spriteFrame * frameInterval);
        float timetoClose = (spriteFrame * frameInterval);

        if (Input.GetKeyDown(KeyCode.LeftShift) && isParachutingToggleable())
        {

            parachuting = !parachuting;

            if (parachuting)
            {
                paraTimeCurrent = Time.time;
                openSpriteParachute();
            }
            else
            {
                paraTimeCurrent = Time.time;
                closeSpriteParachute();
            }
        }

        if (player.IsGrounded() || player.IsOnWall())
        {
            parachuting = false;
            parachutingToggleable = false;
            if (parachuting || spriteOpeningParachute)
                closeSpriteParachute();
        }

        if (player.IsGrounded())
        {
            if (fromHighJumped())
            {
                highJumped = false;
                cooldown.enable();
            }
        }
    }
    private void OnEnable(){}

    //for onDisable to work, HJP must be disabled by default
    private void OnDisable()
    {
        if(cooldown!=null)cooldown.bar.gameObject.SetActive(false);
        if(sprParachute!=null)sprParachute.SetActive(false);
        if(sprBoost!=null)sprBoost.SetActive(false);
    }

    //getter functions
    public bool fromHighJumped()
    {
        return (highJumped && rb.velocity.y + 1f < (player.GetJumpPower() * jumpScale));
    }

    public bool isFalling()
    {
        //return (rb.velocity.y < 0); 
        return (rb.velocity.y < 10f);
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
        spriteFrameBoost = 0;
        paraTimeCurrentBoost = Time.time;
        sprBoost.transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
        sprBoost.SetActive(true);
        spriteOpeningBoost = true;
    }
}
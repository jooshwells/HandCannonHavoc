using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.GraphicsBuffer;

public class Soldier : MonoBehaviour
{
    // object and numerical fields
    public float defaultScale;

    private ProjectileScript projectile;

    [SerializeField] private Transform player;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float detectDist = 10f;

    //[SerializeField] private GameObject weapon;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] spritelist;
    int frame;
    [SerializeField] float timeRate = 0.25f;


    // state checks
    private float dist;
    private float verticalDifference;
    [SerializeField] float vericalTolerance = 3f;
    public bool attacking = false;
    public bool animating = false;
    private float lastFrameTime;

    private int stanceNum;
    private bool stance = true; //true = standing, false = "crouched"


    // attacking
    [SerializeField] float FireRate = 1f;
    [SerializeField] float bulletSpeed = 1f;
    [SerializeField] float bulletDuration = 1f;

    private float lastShotTime;
    private float lastShotFrameTime = float.PositiveInfinity;

    //[SerializeField] Transform gun1;

    [SerializeField] GameObject bulletSprite;
    [SerializeField] Transform gunPos1;
    //[SerializeField] Transform gunPos2;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = spritelist[frame];

        dist = Vector2.Distance(player.position, transform.position);
        verticalDifference = Mathf.Abs(player.position.y - transform.position.y);

        if (dist < detectDist)
        {
            if (!attacking)
            {
                if (!animating)
                    lastFrameTime = Time.time;

                animating = true;

                framesForward(); // will updating animating accordingly
                if (!animating)
                {
                    lastShotTime = Time.time;
                    attacking = true;
                }

            }

            if (attacking)
            {
                if (verticalDifference < vericalTolerance)
                    attackPlayer();
            }
        }

        else
        {
            if (attacking)
            {
                attacking = false;
                stopAttacking();

                if (!animating) //should always be true if it reaches here
                    lastFrameTime = Time.time;

                animating = true;
            }

            if (animating)
            {
                framesReverse(); // will updating animating accordingly
            }
            else
            {
                Idle();
            }
        }

        if (transform.position.x - player.position.x < 0)
        {
            //flip
            transform.localScale = new Vector3(defaultScale, defaultScale, 0);
        }
        else
        {
            transform.localScale = new Vector3(-defaultScale, defaultScale, 0);
        }
    }

    private void framesForward()
    {
        if (frame < 6 && Time.time > lastFrameTime + timeRate)
        {
            frame++;
            lastFrameTime = Time.time;
        }

        else if (frame >= 6)
            animating = false;
    }

    private void framesReverse()
    {
        if (frame > 0 && Time.time > lastFrameTime + timeRate)
        {
            frame--;
            lastFrameTime = Time.time;
        }

        else if (frame <= 0)
            animating = false;

    }
    private void Idle()
    {
        /*stanceNum = (int)(Time.time * 2);

        if (stanceNum % 2 == 0)
            stance = true;
        else
            stance = false;

        if (stance == true)
            frame = 0;
        else
            frame = 1;*/

        frame = ((int)(Time.time * 4)) % 6;
        
    }

    private void attackPlayer()
    {
        if (frame < 11 && Time.time > lastShotFrameTime + 0.05)
        {
            frame++;
            lastShotFrameTime = Time.time;
        }

        if (frame <= 11 && Time.time > lastShotFrameTime + 0.05)
        {
            //frame = 4;
            frame = 6;
            lastShotFrameTime = float.PositiveInfinity;
        }

        //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        if (Time.time > lastShotTime + FireRate)
        {
            fireGun();
            lastShotTime = Time.time;
            lastShotFrameTime = Time.time;

        }
    }

    private void stopAttacking()
    {
        //weapon.SetActive(false);
    }

    public bool isAttacking()
    {
        return attacking;
    }


    private void fireGun()
    {
        GameObject bullet1 = Instantiate(bulletSprite, gunPos1.position, gunPos1.rotation);

        if(transform.position.x - player.position.x < 0)
            bullet1.transform.localScale = new Vector3(-1, 1);
        else
            bullet1.transform.localScale = new Vector3(1, 1);

        //GameObject bullet2 = Instantiate(bulletSprite, gunPos2.position, gunPos2.rotation);
        SpriteRenderer bullet1Renderer = bullet1.GetComponent<SpriteRenderer>();
        //SpriteRenderer bullet2Renderer = bullet2.GetComponent<SpriteRenderer>();

        if (bullet1Renderer != null)
        {
            bullet1Renderer.enabled = true; // Make the bullet visible
        }
        //if (bullet2Renderer != null)
        //{
        //    bullet2Renderer.enabled = true; // Make the bullet visible
        //}

        Vector2 direction1 = (player.position - gunPos1.position) + new Vector3(0, 0.5f);
        //Vector2 direction2 = (player.position - gunPos2.position) - new Vector3(0, 0.5f);

        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        //Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();

        direction1 = direction1.normalized;

        rb1.velocity = direction1 * bulletSpeed * 10;
        //rb2.velocity = direction2 * bulletSpeed;

        //flip(direction, bulletRenderer);
        Destroy(bullet1, bulletDuration);
        //Destroy(bullet2, bulletDuration);
    }
}

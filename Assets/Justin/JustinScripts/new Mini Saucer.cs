using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class newMiniSaucer : MonoBehaviour
{
    SpriteRenderer spriterender;
     
    [SerializeField] Transform transform;
    public Transform target;
    public Rigidbody2D rb;


    [SerializeField] float speed = 400f;
    [SerializeField] float extraHeight = 3f;

    public float sightRange = 20;
    public float horizontalAttackRange = 10;
    public float verticalMinimum = 3;
    EnemyHealthScript healthscript;
    bool spotted = false;

    public bool attacking = false;

    EnemyHealthScript enemyHealthScript;
    Animator animator;
    AIPath aiPath;

    [SerializeField] GameObject bulletprefab;
    [SerializeField] float attackCooldown = 2f;
    public int bulletSpeed = 3;
    public float bulletLifetime = 2;
    public Transform Origin1;
    public Transform Origin2;
    public Transform Origin3;
      public float lastShotTime;

    public bool attacktest = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyHealthScript = GetComponent<EnemyHealthScript>();
        spriterender = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        aiPath = GetComponent<AIPath>();
        aiPath.enabled = false;
        healthscript = GetComponent<EnemyHealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealthScript.GetCurrentHP() <= 0)
        {
            if (animator.enabled == false)
            {
                animator.enabled = true;
            }
        }


        FlashingLights();


        if (!attacking)
        {
            Idle();
        }

        if (Vector3.Distance(transform.position, target.position) < sightRange ||
            healthscript.GetCurrentHP() < healthscript.GetMaxHP() ||
            spotted == true)
        {
            spotted = true;
            attacking = true;
            aiPath.enabled = true;

            if (Mathf.Abs(transform.position.x - target.position.x) < horizontalAttackRange &&
                         transform.position.y - target.position.y > verticalMinimum)
            {
                attacktest = true;

                attackPlayer();
                //StartCoroutine(Attack());
            }
        }
        else
        {
            attacking = false;
            aiPath.enabled = false;
        }



/*
        bool isEnemyAboveAndInXRange = (rb.position.y >= target.position.y + 3f) &&
                                       (Mathf.Abs(rb.position.x - target.position.x) <= X_Dist_Needed);

        //if close and not too high, go up higher
        if (rb.position.y <= target.position.y + 3.5f && Mathf.Abs(rb.position.x - target.position.x) < 5)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1f);
        }

        if (!attacking && isEnemyAboveAndInXRange)
        {
            attacking = true;
            StartCoroutine(Attack());
        }

        if (!isEnemyAboveAndInXRange)
        {
            attacking = false;
        }
*/



        transform.localScale = new Vector3(5.8908f, 5.8908f);
    }

    void FixedUpdate()
    {
/*
        if (rb.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
*/
    }

    private void attackPlayer()
    {
        if (Time.time > lastShotTime + attackCooldown)
        {
            fireGun();
            lastShotTime = Time.time;

            attacktest = true;
        }
    }
    private void fireGun()
    {
        GameObject bullet1 = Instantiate(bulletprefab, Origin1.position, Origin1.rotation);
        GameObject bullet2 = Instantiate(bulletprefab, Origin2.position, Origin2.rotation);
        GameObject bullet3 = Instantiate(bulletprefab, Origin3.position, Origin3.rotation);
        SpriteRenderer bullet1Renderer = bullet1.GetComponent<SpriteRenderer>();
        SpriteRenderer bullet2Renderer = bullet2.GetComponent<SpriteRenderer>();
        SpriteRenderer bullet3Renderer = bullet3.GetComponent<SpriteRenderer>();


        if (bullet1Renderer != null)
        {
            bullet1Renderer.enabled = true; // Make the bullet visible
        }
        if (bullet2Renderer != null)
        {
            bullet2Renderer.enabled = true; // Make the bullet visible
        }
        if (bullet3Renderer != null)
        {
            bullet3Renderer.enabled = true; // Make the bullet visible
        }

        Vector3 direction1 = (target.position - Origin1.position).normalized - new Vector3(0.1f, 0f);
        Vector3 direction2 = (target.position - Origin2.position).normalized;
        Vector3 direction3 = (target.position - Origin3.position).normalized + new Vector3(0.1f, 0f); ;

        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
        Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();

        rb1.velocity = direction1 * bulletSpeed * 5;
        rb2.velocity = direction2 * bulletSpeed * 5;
        rb3.velocity = direction3 * bulletSpeed * 5;

        //flip(direction, bulletRenderer);
        Destroy(bullet1, bulletLifetime);
        Destroy(bullet2, bulletLifetime);
        Destroy(bullet3, bulletLifetime);
    }

    IEnumerator Attack()
    {
        GameObject bullet1 = Instantiate(bulletprefab, Origin1.position, Origin1.rotation);
        SpriteRenderer bullet1Renderer = bullet1.GetComponent<SpriteRenderer>();
        if (bullet1Renderer != null)
        {
            bullet1Renderer.enabled = true; // Make the bullet visible
        }
        Vector3 direction1 = (target.position - Origin1.position).normalized;
        Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
        rb1.velocity = direction1 * bulletSpeed * 5;
        //Destroy(bullet1, bulletLifetime);
        yield return new WaitForSeconds(attackCooldown);
    }


    //Animations

    float lastTime_FlashingLights = 0;
    public Sprite[] flashingSprites;
    int flashingSpritesIndex = 0;
    void FlashingLights()
    {
        if (Time.time > lastTime_FlashingLights)
        {
            spriterender.sprite = flashingSprites[flashingSpritesIndex];
            flashingSpritesIndex = (flashingSpritesIndex + 1) % 3;
            lastTime_FlashingLights = Time.time + 0.15f;
        }
    }

    float lastTime_Idle = 0;
    float lastTime_Up = 0;
    float lastTime_Down = 0;
    bool up = true;
    void Idle()
    {
        if (up && Time.time > lastTime_Up)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 0.1f);
            lastTime_Up = Time.time + 0.1f;
        }
        if (!up && Time.time > lastTime_Down)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.1f);
            lastTime_Down = Time.time + 0.1f;
        }

        if (Time.time > lastTime_Idle)
        {
            lastTime_Idle = Time.time + 1f;
            up = !up;
        }
    }
}

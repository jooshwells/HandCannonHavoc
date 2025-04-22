using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocScript : MonoBehaviour
{

    [Header("Targets")]
    [SerializeField] private Transform player;

    [Header("Animations")]
    [SerializeField] private Animator anim;

    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite standingSprite;

    [Header("Colliders")]
    [SerializeField] private BoxCollider2D quadCollider;
    [SerializeField] private BoxCollider2D standCollider;

    [Header("Attack Params")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float detectDist;


    [Header("AttackTypes")]
    //[SerializeField] private GameObject fireball;
    [SerializeField] private GameObject bird;

    private Rigidbody2D rb;
    private bool standing = false;
    private bool animFinished = false;
    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(player.position + new Vector3(0, 0.1f, 0), transform.position);

        Vector2 distanceVector = (player.position + new Vector3(0, 0.1f, 0)) - transform.position;
        //Debug.Log(distanceVector.x + " " + distanceVector.y);

        if ((distanceVector.x < 0f && transform.localScale.x > 0f) ||
            (distanceVector.x > 0f && transform.localScale.x < 0f))
        {
            Vector3 loc = transform.localScale;
            loc.x *= -1;
            transform.localScale = loc;
        }



        if (!standing && dist <= detectDist)
        {
          
            StartCoroutine(Stand());
            standing = true;
        }

        if (!attacking && animFinished && standing)
        {
            if(dist <= detectDist)
            {
                attacking = true;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        GameObject curBird = Instantiate(bird, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        curBird.GetComponent<ProjectileScript>().SetInstantiator(gameObject);
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }

    IEnumerator Stand()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        anim.SetBool("isStanding", true);
        anim.Play("Stand");

        // Wait until the animation has actually started before checking its length
        yield return new WaitForEndOfFrame();

        // Move it up a bit so its new collider won't clip into the ground
        transform.position = transform.position + new Vector3(0, 0.023f, 0);
        // Switch colliders
        quadCollider.enabled = false;
        standCollider.enabled = true;

        // Freeze position
        float g = rb.gravityScale;
        rb.gravityScale = 0;

        // Wait until the animation is finished
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        rb.gravityScale = g; // Reset gravity
        animFinished = true;
        // Change sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = standingSprite;

        
    }

}

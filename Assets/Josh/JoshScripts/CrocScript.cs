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

    private Rigidbody2D rb;
    private bool standing = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(!standing && Vector2.Distance(transform.position, player.position) < 3f)
        {
          
            standing = true;
            StartCoroutine(Stand());
            
        }
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
        
        // Freeze position
        float g = rb.gravityScale;
        rb.gravityScale = 0;

        // Wait until the animation is finished
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        rb.gravityScale = g; // Reset gravity

        // Change sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = standingSprite;

        // Switch colliders
        quadCollider.enabled = false;
        standCollider.enabled = true;
    }

}

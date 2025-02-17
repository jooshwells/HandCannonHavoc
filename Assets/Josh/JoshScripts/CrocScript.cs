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

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) < 3f)
        {
            StartCoroutine(Stand());
        }
    }

    IEnumerator Stand()
    {
        anim.SetBool("isStanding", true);
        anim.Play("Stand");

        // Wait until the animation has actually started before checking its length
        yield return new WaitForEndOfFrame();

        // Wait until the animation is finished
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        // Change sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = standingSprite;

        // Switch colliders
        quadCollider.enabled = false;
        standCollider.enabled = true;
    }

}

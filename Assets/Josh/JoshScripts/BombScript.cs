using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BombScript : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    private float dir;
    private bool isExploding = false;

    void Awake()
    {
        
    }

    public void setDir(float d) { dir = d; }
    public void Launch()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(12 * dir, 16);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isExploding && Physics2D.OverlapCircle(transform.position, 0.5f, ground))
        {
            
            isExploding = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            StartCoroutine(Explode());

        }
    }

    IEnumerator Explode()
    {
        Animator anim = gameObject.GetComponent<Animator>();

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //anim.SetBool("isExploding", true);
        anim.Play("Exploding");
        float explosionDuration = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(explosionDuration);

        Destroy(gameObject);
        isExploding=false;
    }
}

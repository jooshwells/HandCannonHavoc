using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatScript : MonoBehaviour
{
    private bool isFacingRight = true;
    private float horizontal;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float enemSpeed;
    [SerializeField] private Transform edge;
    [SerializeField] private LayerMask ground;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(isFacingRight == true ? 1 : -1 * enemSpeed, rb.velocity.y);
        if(!Physics2D.OverlapCircle(edge.position, ground)) isFacingRight = !isFacingRight;

    }

    private void Flip()
    {
        // If moving left and facing right, or facing left and moving right
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight; // invert bool
            Vector3 localScale = transform.localScale; // init local scale
            localScale.x *= -1f; // flip sprite on x-axis
            transform.localScale = localScale; // update local scale
        }
    }
}

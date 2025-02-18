using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatScript : MonoBehaviour
{
    private bool isFacingRight = true;
    private float horizontal;

    [Header("Physics")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float enemSpeed;

    [Header("Environment Checks")]
    [SerializeField] private Transform edge;
    [SerializeField] private LayerMask wall;
    [SerializeField] private LayerMask ground;

    [Header("These Induce Response")]
    [SerializeField] private GameObject player;
    [SerializeField] private float detectRange;

    [Header("Go boom")]
    [SerializeField] private GameObject bomb;
    private GameObject curbomb;

    private bool bombActive = false;

    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = player.transform;
    }

    public bool getDir()
    {
        return isFacingRight;
    }

    // Update is called once per frame
    void Update()
    { 
        if(Vector2.Distance(t.position, transform.position) < detectRange) {
            //Debug.Log("in range");
            if(!bombActive)
            {
                bombActive = true;
                curbomb = Instantiate(bomb, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
                curbomb.GetComponent<BombScript>().setDir(isFacingRight == true ? 1f : -1f);
                curbomb.GetComponent<BombScript>().Launch();
                curbomb.GetComponent<BombScript>().DefineInstantiator(gameObject);
            }
        }

        if(curbomb == null) bombActive = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(enemSpeed * (isFacingRight == true ? 1f : -1f), rb.velocity.y);

        if (!Physics2D.OverlapCircle(edge.position, 0.2f, ground) || Physics2D.OverlapCircle(edge.position, 0.2f, wall))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}

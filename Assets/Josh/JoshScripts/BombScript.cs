using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BombScript : MonoBehaviour
{
    [SerializeField] private LayerMask ground;

    void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(16,16);
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, ground))
        {
            Destroy(gameObject);
        }
    }
}

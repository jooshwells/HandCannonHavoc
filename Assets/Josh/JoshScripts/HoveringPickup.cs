using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringPickup : MonoBehaviour
{
    [SerializeField] private float hoverSpeed = 2f;  // Speed of hovering movement
    [SerializeField] private float hoverDistance = 1f; // Target hover height relative to the chest

    private Vector3 startPos;
    private Vector3 targetPos;
    private Rigidbody2D rb;
    //private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = transform.position + new Vector3(0, transform.position.y+0.75f, 0);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (0, hoverSpeed);
    }

 

    void Update()
    {
        if(transform.position.y >= targetPos.y)
        {
            rb.velocity = Vector2.zero;
        }

    }
}

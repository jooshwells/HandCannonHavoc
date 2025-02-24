using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringPickup : MonoBehaviour
{
    [SerializeField] private float hoverSpeed = 0.25f;  // Speed of hovering movement
    [SerializeField] private float hoverDistance = 0.75f; // Target hover height relative to the chest

    private Vector3 startPos;
    private Vector3 targetPos;
    private Rigidbody2D rb;
    private AbilityControllerScript ac;

    //private bool movingUp = true;

    void Start()
    {
        ac = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AbilityControllerScript>();

        startPos = transform.position;
        targetPos = transform.position + new Vector3(0, transform.position.y+hoverDistance, 0);

        transform.position = transform.parent.position;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (0, hoverSpeed);
    }

 

    void Update()
    {
        if(Vector3.Distance(transform.position, transform.parent.position) >= hoverDistance)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ac.PickUp(gameObject.tag);
            Destroy(gameObject);
        }
    }
}

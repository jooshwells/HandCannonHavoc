using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncability : MonoBehaviour
{
    [SerializeField] private float bouncy = 20f;

    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player") && Math.Abs(Vector2.Dot(collision.GetContact(0).normal, Vector2.up)) > 0.5f)
        {
            // Play bounce animation here (if needed)
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
            rb.AddForce(Vector2.up * bouncy, ForceMode2D.Impulse);
        }
    }
}

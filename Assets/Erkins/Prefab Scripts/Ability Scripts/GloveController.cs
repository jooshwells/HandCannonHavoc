using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using System.Linq;
public class GloveController : MonoBehaviour
{
    private Vector2 targetPosition;
    [SerializeField] private GameObject finalGlovePrefab;
    private bool hasLanded = false;
    private bool transformationStarted = false;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;
    private static GameObject currentFinalGlove;
    private Vector2 startPosition;

    private static List<GameObject> gloves = new List<GameObject>();
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        startPosition = transform.position;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.simulated = true;
        }
    }

    public void SetTarget(Vector2 target, GameObject finalGlove)
    {
        Debug.Log(Environment.StackTrace);
        Debug.Log($"SetTarget called with position: {target}, assigning final glove prefab: {finalGlove?.name}");
        targetPosition = target;
        finalGlovePrefab = finalGlove;

        if (finalGlovePrefab == null)
        {
            Debug.LogError("SetTarget received a NULL finalGlovePrefab!");
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected");
        int groundLayer = LayerMask.NameToLayer("Ground");
        int wallLayer = LayerMask.NameToLayer("Wall");

        // Check if the collision is with the tilemap AND if it's on the correct layer
        if (!hasLanded && collision.gameObject.layer == groundLayer || collision.gameObject.layer == wallLayer)
        {
            Debug.Log($"Glove has landed on: {collision.gameObject.name} (Layer: {LayerMask.LayerToName(collision.gameObject.layer)})");

            // Prevent multiple triggers
            hasLanded = true;

            // Adjust glove position (snap to surface)
            if (col != null)
            {
                transform.position = new Vector2(transform.position.x, collision.contacts[0].point.y);
                Debug.Log($"Glove position adjusted to: {transform.position}");
            }

            // Stop movement and freeze physics
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static;
                Debug.Log("Glove is now stuck to the surface.");
            }

            // Start transformation animation
            if (!transformationStarted)
            {
                transformationStarted = true;
                if (animator != null)
                {
                    Debug.Log("Playing inflation animation");
                    //animator.SetTrigger("Inflation");
                    SpawnFinalGlove();
                }
            }
        }
        else
        {
            Debug.LogWarning($"OnCollisionEnter2D ignored. Collision with {collision.gameObject.name} (Layer: {LayerMask.LayerToName(collision.gameObject.layer)})");
        }
    }

    public void OnInflationAnimationComplete()
    {
        //StackTraceLogType stackTrace = new StackTraceLogType();
        Debug.Log(Environment.StackTrace);

        Debug.Log("Inflation animation completed! Calling SpawnFinalGlove...");
        SpawnFinalGlove();
    }

    void SpawnFinalGlove()
    {
        Debug.Log("Attempting to spawn final glove...");
        // Destroy old final glove if it exists before spawning the new one
        if (gloves.Count >= 3)
        {
            if (gloves[0] != null)
            {
                Debug.Log("Destroying old final glove before spawning a new one: " + gloves[0].name);
                Destroy(gloves[0]);  // Destroy the GameObject
            }
            gloves.RemoveAt(0); // Remove reference from the list
        }

        // Spawn the final glove prefab at a slightly adjusted position
        if (finalGlovePrefab != null)
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y + 0.3f);
            currentFinalGlove = Instantiate(finalGlovePrefab, spawnPosition, Quaternion.Euler(0, 0, 270));
            gloves.Add(currentFinalGlove);
            Debug.Log("New final glove spawned.");
        }
        else
        {
            Debug.LogError("finalGlovePrefab is NULL! Assign it in SetTarget.");
        }

        // Destroy the glove object after spawning the final glove
        Destroy(gameObject);
    }
}

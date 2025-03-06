using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

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
    // Ensure the glove only triggers the transformation logic after landing on the tilemap
    if (!hasLanded && collision.collider.GetComponent<TilemapCollider2D>() != null)
    {
        Debug.Log("Glove has landed on tilemap!");

        // Prevent triggering more than once by marking as landed
        hasLanded = true;

        // Adjust the glove's position on landing (align with the tilemap)
        if (col != null)
        {
            transform.position = new Vector2(transform.position.x, collision.contacts[0].point.y);
            Debug.Log($"Glove position adjusted to: {transform.position}");
        }

        // Make the glove stop moving and stick to the tilemap
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop the glove from moving
            rb.bodyType = RigidbodyType2D.Static; // Make it static so it sticks to the tilemap
            Debug.Log("Glove is now stuck to the tilemap.");
        }

        // Start the transformation only once, after landing on the tilemap
        if (!transformationStarted)
        {
            transformationStarted = true;
            if (animator != null)
            {
                Debug.Log("Playing inflation animation");
                animator.SetTrigger("Inflation");

            }
        }
    }
    else
    {
        // Prevent re-triggering transformation if it's already landed
        Debug.LogWarning("OnCollisionEnter2D triggered after landing — ignoring.");
    }
}
    public void OnInflationAnimationComplete()
    {
        Debug.Log("Inflation animation completed! Calling SpawnFinalGlove...");
        SpawnFinalGlove();
    }

    void SpawnFinalGlove()
    {
        Debug.Log("Attempting to spawn final glove...");

        // Destroy old final glove if it exists before spawning the new one
        if (currentFinalGlove != null)
        {
            Debug.Log("Destroying old final glove before spawning a new one.");
            Destroy(currentFinalGlove);
            currentFinalGlove = null;
        }

        // Spawn the final glove prefab at a slightly adjusted position
        if (finalGlovePrefab != null)
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y + 0.3f);
            currentFinalGlove = Instantiate(finalGlovePrefab, spawnPosition, Quaternion.Euler(0, 0, 270));
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

using UnityEngine;
using System.Collections;

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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.simulated = true; // Ensures new gloves can move freely
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
        Debug.Log($"Glove collided with {collision.gameObject.name}");

        if (!hasLanded)
        {
            Debug.Log("Glove has landed!");
            hasLanded = true;

            if (col != null)
            {
                transform.position = new Vector2(transform.position.x, collision.contacts[0].point.y);
                Debug.Log($"Glove position adjusted to: {transform.position}");
            }

            if (!transformationStarted)
            {
                transformationStarted = true;
                if (animator != null)
                {
                    Debug.Log("Playing inflation animation");
                    animator.SetTrigger("Inflation");
                }
                else
                {
                    Debug.LogWarning("No animator found, spawning final glove immediately.");
                    StartCoroutine(DelayedSpawn());
                }
            }
        }
        else
        {
            Debug.LogWarning("OnCollisionEnter2D triggered again after landing—ignoring.");
        }
    }

    // Called at the end of the "Inflation" animation
    public void OnInflationAnimationComplete()
    {
        Debug.Log("Inflation animation completed! Calling SpawnFinalGlove...");
        StartCoroutine(DelayedSpawn());
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(0.1f); // Small delay for smoother transition
        SpawnFinalGlove();
    }

    void SpawnFinalGlove()
    {
        Debug.Log("Attempting to spawn final glove...");

        // Destroy the existing final glove if it already exists
        if (currentFinalGlove != null)
        {
            Debug.Log("Destroying old final glove before spawning a new one.");
            Destroy(currentFinalGlove);
            currentFinalGlove = null; // Clear reference
        }

        if (finalGlovePrefab != null)
        {
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y + 0.3f);
            
            // Spawn the new glove and assign it to the static reference
            currentFinalGlove = Instantiate(finalGlovePrefab, spawnPosition, Quaternion.Euler(0, 0, 270));
            
            Debug.Log("New final glove spawned.");
        }
        else
        {
            Debug.LogError("finalGlovePrefab is NULL! Assign it in SetTarget.");
        }

        Destroy(gameObject); // Remove the deflated glove
    }

    IEnumerator FadeAndDestroy()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            collider.enabled = false; // Prevent further collisions
        }

        for (float t = 1f; t > 0; t -= Time.deltaTime * 3)
        {
            sr.color = new Color(1, 1, 1, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}

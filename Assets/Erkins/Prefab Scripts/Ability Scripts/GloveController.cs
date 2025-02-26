using UnityEngine;

public class GloveController : MonoBehaviour
{
    private Vector2 targetPosition;
    private GameObject finalGlovePrefab;
    private bool hasLanded = false;
    private bool transformationStarted = false;
    private Animator animator; // Optional, if you have an animation setup
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Make sure an Animator component is attached if using animations
    }

    public void SetTarget(Vector2 target, GameObject finalGlove)
    {
        targetPosition = target;
        finalGlovePrefab = finalGlove;
    }

    // When the glove collides with the ground, stick and transform.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision with an object tagged "Ground"
        if (!hasLanded && collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;
            
            // Stop the glove's movement completely
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
                rb.simulated = false; // Stop physics simulation
            }
            
            // Adjust the glove's position to ensure it sits fully on the surface.
            // This moves the glove upward by the collider's half-height.
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                float halfHeight = col.bounds.extents.y;
                // Use the collision contact point to determine the ground level.
                Vector2 contactPoint = collision.contacts[0].point;
                transform.position = new Vector2(transform.position.x, contactPoint.y + halfHeight);
            }

            // Trigger the transformation sequence only once.
            if (!transformationStarted)
            {
                transformationStarted = true;
                if (animator != null)
                {
                    animator.SetTrigger("Inflation"); // Ensure your animation has a trigger parameter named "Inflate"
                    // Either use an Animation Event at the end of the animation to call SpawnFinalGlove,
                    // or use a delay matching the animation length:
                    Invoke("SpawnFinalGlove", 1.0f); // Adjust delay as necessary.
                }
                else
                {
                    SpawnFinalGlove();
                }
            }
        }
    }

    void SpawnFinalGlove()
    {
        Instantiate(finalGlovePrefab, transform.position, finalGlovePrefab.transform.rotation);
        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class SWAT_Health : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public bool isDying = false; // Make this public so the AI script can access it

    // This function will be used to update health when damage is taken
    public void UpdateHealth(float damage)
    {
        Debug.Log("Attempting to do " + damage + " damage to " + gameObject.name);

        // If you have any hit effects or visual damage feedback, call it here
        PlayerHitEffect hitEffect = GetComponent<PlayerHitEffect>();
        
        if (hitEffect != null)
        {
            hitEffect.TakeDamage();
        }

        // Decrease health
        health -= damage > 0 ? damage : 0;

        // Check if health reaches 0 and handle death
        if (health <= 0f && !isDying)
        {
            isDying = true; // Mark as dying to prevent repeated death triggers
            StartCoroutine(DyingAnimation());
        }
    }

    public float GetHealth()  // Get the current health value
    {
        return health;
    }

    private IEnumerator DyingAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if(animator == null) animator = GetComponentInChildren<Animator>();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false; // Stop physics
        }

        if (animator != null)
        {
            animator.Play("SWAT Dead");
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        }
        else
        {
            Debug.LogWarning("No Animator found on " + gameObject.name);
        }

        Destroy(gameObject); // Destroy the object when the death animation is over
    }
}

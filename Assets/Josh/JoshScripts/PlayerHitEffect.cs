using System.Collections;
using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.1f;
    public float invincibilityDuration = 1.5f;
    public float blinkInterval = 0.1f;

    private Color originalColor;

    private void Start()
    {
        
    }

    public void TakeDamage()
    {
        StartCoroutine(FlashAndBlink());
    }

    private IEnumerator FlashAndBlink()
    {
        // Blinking effect
        float elapsed = 0f;
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // Ensure sprite is visible after blinking
        spriteRenderer.enabled = true;
    }
}

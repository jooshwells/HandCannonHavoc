using System.Collections;
using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material whiteFlashMaterial; // Assign a white material in the inspector
    public float flashDuration = 0.1f;
    public float invincibilityDuration = 1.5f;
    public float blinkInterval = 0.1f;

    private Material defaultMaterial;
    private bool isFlashing = false; // Prevents restarting the flash effect

    private void Start()
    {
        defaultMaterial = spriteRenderer.material; // Store the original material
    }

    public bool GetFlashing() => isFlashing;

    public void TakeDamage()
    {
        if (!isFlashing) // Only start the effect if it's not already running
        {
            StartCoroutine(FlashAndBlink());
        }
    }

    private IEnumerator FlashAndBlink()
    {
        isFlashing = true; // Set flag to prevent restarting effect

        // Flash white using a material change
        spriteRenderer.material = whiteFlashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = defaultMaterial;

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
        isFlashing = false; // Reset flag when effect is done
    }
}

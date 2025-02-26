using UnityEngine;
using UnityEngine.UI; // For UI-based crosshairs

public class CrosshairController : MonoBehaviour
{
    public Sprite defaultCrosshair;
    public Sprite shrinkCrosshair;
    private SpriteRenderer spriteRenderer; // For 2D crosshairs
    private Image crosshairImage; // For UI crosshairs (if using UI canvas)

    void Awake()
    {
        // Try to get SpriteRenderer first (for world-space crosshairs)
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If no SpriteRenderer, assume it's a UI-based crosshair
        if (spriteRenderer == null)
        {
            crosshairImage = GetComponent<Image>();
        }

        ActivateCrosshair();
    }

    public void ActivateCrosshair()
    {
        Debug.Log("ActivateCrosshair() called");
        SetSprite(defaultCrosshair);
    }

    public void ShrinkCrosshair()
    {
        Debug.Log("ShrinkCrosshair() called");
        SetSprite(shrinkCrosshair);
    }

    public void ResetCrosshair()
    {
        SetSprite(defaultCrosshair);
    }

    private void SetSprite(Sprite newSprite)
    {
        Debug.Log("SetSprite() called");
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite; // For 2D world-based crosshairs
        }
        else if (crosshairImage != null)
        {
            crosshairImage.sprite = newSprite; // For UI-based crosshairs
        }
    }
}

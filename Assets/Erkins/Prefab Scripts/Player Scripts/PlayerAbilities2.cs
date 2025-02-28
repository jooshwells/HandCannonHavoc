using UnityEngine;

public class PlayerAbilities2 : MonoBehaviour
{
    public GameObject crosshairsPrefab;
    public GameObject deflatedGlovePrefab; // The "bullet" version of the glove
    public GameObject finalGlovePrefab;     // The "finished" bounce pad
    [SerializeField] private float launchForce = 20f;
    [SerializeField] private Transform handTransform; // Assign in the Unity Inspector
    private GameObject crosshairsInstance;
    private bool crosshairActive = false;
    private GameObject currentGlove; // Track the active glove so only one exists

    void Update()
    {
        // Toggle crosshairs on/off with E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleCrosshair();
        }

        if (crosshairActive)
        {
            MoveCrosshair();

            // Launch glove on left click
            if (Input.GetMouseButtonDown(0))
            {
                LaunchGlove();
            }
        }
    }

    void ToggleCrosshair()
    {
        if (crosshairActive)
        {
            if (crosshairsInstance != null)
                Destroy(crosshairsInstance);
        }
        else
        {
            crosshairsInstance = Instantiate(crosshairsPrefab);
        }
        crosshairActive = !crosshairActive;
    }

    void MoveCrosshair()
    {
        if (crosshairsInstance != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            crosshairsInstance.transform.position = mousePos;
        }
    }

    void LaunchGlove()
    {
        // Ensure only one glove is active
        if (currentGlove != null)
        {
            Destroy(currentGlove);
        }

        if (crosshairsInstance == null) return;
        Vector2 targetPosition = crosshairsInstance.transform.position;

        // Instantiate the deflated glove at player's position, using its preset rotation
        currentGlove = Instantiate(deflatedGlovePrefab, handTransform.position, deflatedGlovePrefab.transform.rotation);

        // Set the glove's velocity
        Rigidbody2D rb = currentGlove.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.velocity = direction * launchForce;
        }

        // Tell the glove where to go and what final prefab to spawn
        GloveController gc = currentGlove.GetComponent<GloveController>();
        if (gc != null)
        {
            gc.SetTarget(targetPosition, finalGlovePrefab);
        }

        // Call the crosshair's shrink function if available
        CrosshairController cc = crosshairsInstance.GetComponent<CrosshairController>();
        if (cc != null)
        {
            cc.ShrinkCrosshair();
        }

        // Optionally, destroy the crosshair after launching so you only use it once
        Destroy(crosshairsInstance);
        crosshairActive = false;
    }
}

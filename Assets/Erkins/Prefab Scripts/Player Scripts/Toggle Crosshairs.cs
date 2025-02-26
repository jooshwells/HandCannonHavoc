using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public GameObject crosshairsPrefab;
    public GameObject deflatedGlovePrefab; // The "bullet" version of the glove
    public GameObject finalGlovePrefab; // The "finished" bounce pad
    [SerializeField] private float launchForce = 20f;

    private GameObject crosshairsInstance;
    private bool crosshairActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Toggle crosshairs
        {
            ToggleCrosshair();
        }

        if (crosshairActive)
        {
            MoveCrosshair();

            if (Input.GetMouseButtonDown(0)) // Left click to launch
            {
                LaunchGlove();
            }
        }
    }

    void ToggleCrosshair()
    {
        if (crosshairActive)
        {
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshairsInstance.transform.position = mousePos;
    }

    void LaunchGlove()
    {
        Vector2 targetPosition = crosshairsInstance.transform.position;
        GameObject gloveInstance = Instantiate(deflatedGlovePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = gloveInstance.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.velocity = direction * launchForce;
        }

       /* GloveController gloveController = gloveInstance.GetComponent<GloveController>();
        if (gloveController != null)
        {
            gloveController.SetTarget(targetPosition, finalGlovePrefab);
        }*/
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class HandPointer : MonoBehaviour
{
    public RectTransform handRectTransform; // Drag your hand image here in Inspector
    private RectTransform currentTarget; // Current button being hovered
    private Vector3 originalScreenPosition; // To store the original screen position of the hand
    private float rotationVelocity = 0f; // Velocity used by SmoothDampAngle

    void Start()
    {
        // Store the original screen position of the hand
        originalScreenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, handRectTransform.position);
    }

    void Update()
    {
        if (currentTarget != null)
        {
            // Rotate the hand towards the current target (button)
            Vector3 dir = currentTarget.position - handRectTransform.position;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // Smoothly rotate the hand towards the target angle
            float targetRotation = Mathf.SmoothDampAngle(
                handRectTransform.eulerAngles.z,
                targetAngle,
                ref rotationVelocity,
                0.15f // SmoothDamp time
            );

            handRectTransform.rotation = Quaternion.Euler(0f, 0f, targetRotation);
        }
    }

    // Called when the mouse exits a button
    public void OnPointerExit(PointerEventData eventData)
    {
        // Convert the stored screen position back to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(originalScreenPosition);
        worldPosition.z = 0f; // Make sure it's on the correct plane (2D UI)

        // Reset the hand's position to the original screen position
        handRectTransform.position = worldPosition;
        currentTarget = null; // Reset the current target
    }

    // Called when the mouse enters a button
    public void OnButtonHoverEnter(RectTransform target)
    {
        currentTarget = target; // Set the current target to the hovered button
    }
}

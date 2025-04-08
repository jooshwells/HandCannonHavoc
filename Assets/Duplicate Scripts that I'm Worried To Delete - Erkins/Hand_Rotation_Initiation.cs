using UnityEngine;

public class HandPointer : MonoBehaviour
{
    public RectTransform handRectTransform; // Your hand image
    public RectTransform idleTarget; // Assign your invisible idle target here
    private RectTransform currentTarget;
    private float rotationVelocity = 0f;

    void Update()
    {
        if (currentTarget != null)
        {
            RotateHandTowards(currentTarget.position);
        }
        else if (idleTarget != null)
        {
            RotateHandTowards(idleTarget.position);
        }
    }

    void RotateHandTowards(Vector3 targetPos)
    {
        Vector3 dir = targetPos - handRectTransform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float smoothAngle = Mathf.SmoothDampAngle(
            handRectTransform.eulerAngles.z,
            targetAngle,
            ref rotationVelocity,
            0.15f
        );

        handRectTransform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
    }

    // These get called from Event Triggers on each button
    public void OnButtonHoverEnter(RectTransform target)
    {
        currentTarget = target;
    }

    public void OnButtonHoverExit()
    {
        currentTarget = null;
    }
}

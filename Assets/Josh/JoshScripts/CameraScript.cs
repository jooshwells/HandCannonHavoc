using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private float smoothSpeed = 5f; // How smoothly the camera follows the player
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from the player

    private void LateUpdate()
    {
        if (player != null)
        {
            // Desired camera position
            Vector3 targetPosition = player.position + offset;

            // Smoothly move towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}

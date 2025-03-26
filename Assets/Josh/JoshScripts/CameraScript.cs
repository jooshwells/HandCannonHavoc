using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f; // Smooth follow speed
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from player
    [SerializeField] private float followThreshold = 5f; // Min distance before unlocking
    [SerializeField] private float stopThreshold = 0.05f; // Prevents jitter when locking
    [SerializeField] private float directionOffset = 6f; // Extra offset in movement direction

    private Transform player;
    private Rigidbody2D playerRb;
    private bool camLocked = true;
    private float lastDirection = 1f;
    private Vector3 targetPosition;

    private bool movingRight = false;
    private bool movingLeft = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = player.gameObject.GetComponent<Rigidbody2D>();
    }

    private float left, right;
    private bool boundsSet;
    private void LateUpdate()
    {
        if (player.position.x <= right && player.position.x >= left) return;

        if (!boundsSet && playerRb.velocity.magnitude <= 0.1f)
        {
            //transform.position = player.position + offset;
            ////transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothSpeed * Time.deltaTime);

            //left = transform.position.x - 4;
            //right = transform.position.x + 4;

            StartCoroutine(MoveToTarget(player.position + offset));
            boundsSet = true;
        }

        if (player.position.x > right)
        {
            movingRight = true;
            movingLeft = false;
            boundsSet = false;
        }

        if (player.position.x < left)
        {
            movingLeft = true;
            movingRight = false;
            boundsSet = false;
        }

        if (movingRight)
        {
            Debug.Log("Moving right logic executing");
            Vector3 desiredPosition = player.position + new Vector3(directionOffset, offset.y, offset.z);
            targetPosition = Vector3.Lerp(targetPosition, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        else if (movingLeft)
        {
            Debug.Log("Moving left logic executing");

            Vector3 desiredPosition = player.position + new Vector3(-directionOffset, offset.y, offset.z);
            targetPosition = Vector3.Lerp(targetPosition, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        else // locked
        {
            Debug.Log("Centered logic executing");

            //Vector3 desiredPosition = player.position + new Vector3(0, offset.y, offset.z);
            //targetPosition = Vector3.Lerp(targetPosition, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, player.position+offset, smoothSpeed * Time.deltaTime);
        }
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > stopThreshold)
        {
            transform.position = Vector3.Lerp(transform.position, target, smoothSpeed * Time.deltaTime);
            yield return null;
        }

        // Update bounds only after reaching the target
        left = transform.position.x - 4;
        right = transform.position.x + 4;
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            // Draw the left threshold
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(left, transform.position.y - 5, 0), new Vector3(left, transform.position.y + 5, 0));

            // Draw the right threshold
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(right, transform.position.y - 5, 0), new Vector3(right, transform.position.y + 5, 0));
        }
    }
}

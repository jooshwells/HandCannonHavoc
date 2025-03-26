//using UnityEngine;
//using System.Collections;
//using TMPro;

//public class CameraFollow : MonoBehaviour
//{
//    [SerializeField] private float smoothSpeed = 5f; // How smoothly the camera follows the player
//    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from the player
//    private Transform player;
//    private float playerOffset = 0f;
//    private Rigidbody2D playerRb;
//    private float startingXPos;
//    private bool camLocked = true;

//    private void Start()
//    {
//        startingXPos = GetComponent<Camera>().transform.position.x;
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//        playerRb = player.gameObject.GetComponent<Rigidbody2D>();
//    }

//    bool flip = false;
//    bool coroutineRunning = false;
//    private void LateUpdate()
//    {
//        if (camLocked && Mathf.Abs(playerRb.velocity.x) >= 0.1f && Mathf.Abs(player.position.x - transform.position.x) >= 2f)
//        {
//            camLocked = false;

//            Vector3 targetPosition = new Vector3();
//            if (playerRb.velocity.x > 0)
//            {
//                targetPosition = player.position + new Vector3(6, 0, 0);
//            }
//            else
//            {
//                targetPosition = player.position + new Vector3(-6, 0, 0);
//            }


//            transform.position = Vector3.Lerp(transform.position, targetPosition + offset, smoothSpeed * Time.deltaTime);
//        }
//        else if (!camLocked)
//        {
//            camLocked = true;
//            transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothSpeed * Time.deltaTime);
//        }
//    }
//}

//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    [SerializeField] private float smoothSpeed = 5f; // Smooth follow speed
//    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from player
//    [SerializeField] private float followThreshold = 5f; // Minimum distance before the camera moves

//    private Transform player;
//    private Rigidbody2D playerRb;
//    private Vector3 velocity = Vector3.zero; // Used for SmoothDamp
//    private bool isFollowing = false; // Determines if camera should move
//    private Vector3 centeredPosition;
//    private Vector3 targetPosition;

//    private void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//        playerRb = player.GetComponent<Rigidbody2D>();
//        targetPosition = player.position + offset;
//        centeredPosition = targetPosition;
//    }

//    private void LateUpdate()
//    {
//        if (player == null) return;



//        // Check if the player has moved beyond the follow threshold
//        if (Mathf.Abs(player.position.x - transform.position.x) > followThreshold)
//        {

//            if (!isFollowing)
//            {
//                if (player.position.x > transform.position.x)
//                {
//                    targetPosition += new Vector3(6, 0, 0);
//                }
//                else
//                {
//                    targetPosition += new Vector3(-6, 0, 0);
//                }
//            }

//            isFollowing = true;


//        }

//        if (isFollowing)
//        {
//            // Smoothly follow player with damping
//            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

//            // Stop following when close enough to prevent micro-adjustments
//            if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f)
//            {
//                isFollowing = false;
//                targetPosition = centeredPosition;
//            }
//        }
//    }
//}

//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    [SerializeField] private float smoothSpeed = 5f; // How smoothly the camera follows the player
//    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from the player
//    [SerializeField] private float followThreshold = 2f; // Minimum distance before unlocking
//    [SerializeField] private float stopThreshold = 0.1f; // Prevents jitter when locking
//    [SerializeField] private float directionOffset = 6f; // Distance ahead of the player

//    private Transform player;
//    private Rigidbody2D playerRb;
//    private bool camLocked = true;
//    private float lastDirection = 1f;
//    private Vector3 targetPosition;

//    private void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//        playerRb = player.GetComponent<Rigidbody2D>();
//        targetPosition = transform.position; // Initialize target position
//    }

//    private void LateUpdate()
//    {
//        if (player == null) return;

//        float playerSpeed = playerRb.velocity.x;

//        // Update last movement direction when moving
//        if (Mathf.Abs(playerSpeed) > 0.1f)
//        {
//            lastDirection = Mathf.Sign(playerSpeed); // 1 for right, -1 for left
//        }

//        // Unlock camera if the player moves beyond followThreshold
//        if (camLocked && Mathf.Abs(player.position.x - transform.position.x) >= followThreshold)
//        {
//            camLocked = false;
//            targetPosition = player.position + new Vector3(lastDirection * directionOffset, offset.y, offset.z);
//        }

//        if (!camLocked)
//        {
//            // Move camera smoothly to target position
//            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

//            // If camera is close enough to the target, lock it again
//            if (Vector3.Distance(transform.position, targetPosition) < stopThreshold)
//            {
//                camLocked = true;
//            }
//        }
//    }
//}

using UnityEngine;

public class CameraFollow : MonoBehaviour
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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = player.GetComponent<Rigidbody2D>();
        targetPosition = transform.position; // Initialize to current position
    }

    private void LateUpdate()
    {
        if (player == null) return;

        float playerSpeed = playerRb.velocity.x;

        if(camLocked && Mathf.Abs(player.position.x - transform.position.x) <= followThreshold)
        {
            targetPosition = Vector3.Lerp(targetPosition, player.position + offset, smoothSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); ;

        }

        // Update last movement direction when moving
        if (Mathf.Abs(playerSpeed) > 0.1f)
        {
            lastDirection = Mathf.Sign(playerSpeed); // 1 for right, -1 for left
        }

        // Unlock camera if player moves past the follow threshold
        if (camLocked && Mathf.Abs(player.position.x - transform.position.x) >= followThreshold)
        {
            camLocked = false;
        }

        // Adjust target position only when camera is unlocked
        if (!camLocked)
        {
            // Gradually shift the target position instead of instantly moving it
            Vector3 desiredPosition = player.position + new Vector3(lastDirection * directionOffset, offset.y, offset.z);
            targetPosition = Vector3.Lerp(targetPosition, desiredPosition, smoothSpeed * Time.deltaTime);

            // Move camera smoothly
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            // Lock the camera again when close enough
            if (Vector3.Distance(transform.position, targetPosition) < stopThreshold)
            {
                camLocked = true;
            }
        }
    }
}


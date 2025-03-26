using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f; // How smoothly the camera follows the player
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from the player
    private Transform player;
    private float playerOffset = 6f;
    private Rigidbody2D playerRb;
    private float startingXPos;

    private void Start()
    {
        startingXPos = GetComponent<Camera>().transform.position.x;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = player.gameObject.GetComponent<Rigidbody2D>();
    }

    bool flip = false;
    bool coroutineRunning = false;
    private void LateUpdate()
    {
        if (!coroutineRunning)
        {
            if (player.localScale.x < 0 && playerOffset > 0f)
            {
                StartCoroutine(CamCheck());
            }
            else if (player.localScale.x > 0 && playerOffset < 0f)
            {
                StartCoroutine(CamCheck());
            }
        }

        if (player != null)
        {
            // Desired camera position
            Vector3 targetPosition = player.position + offset;

            // Smoothly move towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition + new Vector3(startingXPos + playerOffset, 0, 0), smoothSpeed * Time.deltaTime);
        }
    }

    IEnumerator CamCheck()
    {
        coroutineRunning = true;
        float curLocalScaleX = player.localScale.x;
        yield return new WaitForSeconds(.75f);
        float lower = curLocalScaleX - 0.5f;
        float upper = curLocalScaleX + 0.5f;
        if (player.transform.localScale.x >= lower && player.transform.localScale.x <= upper)
        {
            playerOffset *= -1;
        }
        coroutineRunning = false;
    }
}
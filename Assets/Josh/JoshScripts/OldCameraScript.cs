using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCameraScript : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f; // How smoothly the camera follows the player
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from the player
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // Desired camera position
            Vector3 targetPosition = player.position + offset;

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

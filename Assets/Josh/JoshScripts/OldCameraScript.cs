using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCameraScript : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f; // How smoothly the camera follows the player
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from the player
    private Transform player;
    private Transform background;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        background = GameObject.FindGameObjectWithTag("Background").transform;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // Desired camera position
            Vector3 targetPosition = player.position + offset;

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            background.position = Vector3.Lerp(background.position, (targetPosition-offset) + new Vector3(0,-3,0), smoothSpeed/2 * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

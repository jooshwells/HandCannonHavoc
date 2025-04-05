using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCameraScript : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f; // How smoothly the camera follows the player
    [SerializeField] private Vector3 offset = new Vector3(0, 4, -10); // Offset from the player
    private Transform player;
    private Transform background;
    private Vector3 directionalInput;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if(GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        //background = GameObject.FindGameObjectWithTag("Background").transform;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.I)) // Up-Left
            {
                directionalInput = new Vector3(-4f, 4f, 0);
            }
            else if (Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.I)) // Up-Right
            {
                directionalInput = new Vector3(4f, 4f, 0);
            }
            else if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.K)) // Down-Left
            {
                directionalInput = new Vector3(-4f, -4f, 0);
            }
            else if (Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.K)) // Down-Right
            {
                directionalInput = new Vector3(4f, -4f, 0);
            }
            else if (Input.GetKey(KeyCode.J))
            {
                directionalInput = new Vector3(-4f, 0, 0);
            }
            else if (Input.GetKey(KeyCode.L))
            {
                directionalInput = new Vector3(4f, 0, 0);
            }
            else if (Input.GetKey(KeyCode.I))
            {
                directionalInput = new Vector3(0, 4f, 0);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                directionalInput = new Vector3(0, -4f, 0);
            }
            else
            {
                directionalInput = new Vector3(0, 0, 0);
            }



            // Desired camera position
            Vector3 targetPosition = player.position + offset + directionalInput;

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            //background.position = Vector3.Lerp(background.position, (targetPosition-offset) + new Vector3(0,-3,0), smoothSpeed/2 * Time.deltaTime);
        } else
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

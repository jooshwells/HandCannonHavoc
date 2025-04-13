using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMovementController : MonoBehaviour
{
    [SerializeField] private Transform endLoc;

    private Vector3 startingLocation;

    private Vector3 upper;
    private Vector3 lower;
    private float horizontalSpeed = 1.2f;
    private float verticalSpeed = 8f;

    private bool movingUp = true;
    private float speed = 1.25f;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingLocation = transform.position;
        upper = startingLocation + new Vector3(0f, 4f, 0f); 
        lower = startingLocation + new Vector3(0f, -4f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Move horizontally at a constant speed
        transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;

        // Determine vertical target
        Vector3 targetY = movingUp ? upper : lower;

        // Move vertically toward target at a constant speed
        Vector3 newPos = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetY.y, transform.position.z), verticalSpeed * Time.deltaTime);
        transform.position = newPos;

        // Check if target reached, and switch direction
        if (Mathf.Approximately(transform.position.y, targetY.y))
        {
            movingUp = !movingUp;
            verticalSpeed = Random.Range(5f, 8f);
        }


    }
}

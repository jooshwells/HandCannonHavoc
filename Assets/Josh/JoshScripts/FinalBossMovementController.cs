using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossMovementController : MonoBehaviour
{
    private Vector3 startingLocation;

    private Vector3 upper;
    private Vector3 lower;

    private bool movingUp = true;
    private float speed = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        startingLocation = transform.position;
        upper = startingLocation + new Vector3(0f, 4f, 0f); 
        lower = startingLocation + new Vector3(0f, -4f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < upper.y - 0.3f && movingUp)
        {
            Vector3 target = Vector3.Lerp(transform.position, upper, Time.deltaTime * speed);
            transform.position = target;
        } else
        {
            speed = Random.Range(0.5f, 2f);
            movingUp = false;
        }

        if (transform.position.y > lower.y + 0.3f && !movingUp)
        {
            Vector3 target = Vector3.Lerp(transform.position, lower, Time.deltaTime * speed);
            transform.position = target;
        }
        else
        {
            speed = Random.Range(0.5f, 2f);
            movingUp = true;
        }
    }
}

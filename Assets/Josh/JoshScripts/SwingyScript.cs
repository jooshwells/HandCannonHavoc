using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private DistanceJoint2D dj;

    [SerializeField] private float g = 4f;
    [SerializeField] private float damping = 0.05f;
    private float pendulumLength;

    private Vector2 restPos;
    private float angle;
    private float angVeloc;
    [SerializeField] private float inputStrength = 8f;
    // Start is called before the first frame update
    public void SetPendLength(float len)
    {
        pendulumLength = len;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dj = GetComponent<DistanceJoint2D>();
        restPos = dj.connectedAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = rb.position - (Vector2)restPos;
        angle = Mathf.Atan2(direction.y, direction.x);

        float angularAcceleration = -g * Mathf.Sin(angle) / pendulumLength;
        angularAcceleration -= damping * angVeloc;

        float input = Input.GetAxis("Horizontal"); // Capture input from the player (e.g., arrow keys or A/D)
        angularAcceleration += input * inputStrength; // Add the input to the pendulum's angular acceleration

        angVeloc += angularAcceleration * Time.deltaTime;
        angle += angVeloc * Time.deltaTime;

        rb.AddTorque(angularAcceleration * rb.mass);

    }
}

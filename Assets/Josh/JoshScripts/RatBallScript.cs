using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddTorque(1f);
    }
}

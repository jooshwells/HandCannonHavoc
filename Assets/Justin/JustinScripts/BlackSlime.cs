using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSlime : MonoBehaviour
{
    public Transform target;
    Rigidbody2D rb;
    SlimeScript slimeScript;
    public float detectRange = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inSpottedRange(detectRange)) //if within 10 units
        {
            slimeScript.enabled = true;
        }
        else
        {
            slimeScript.enabled = false;
        }
    }

    bool inSpottedRange(float range)
    {
        return (Vector2.Distance(rb.position, target.position) < range); //start attacking
    }
}

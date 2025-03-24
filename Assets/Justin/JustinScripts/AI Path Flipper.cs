using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPathFlipper : MonoBehaviour
{
    public AIPath aiPath;
    public float defaultScale;

    // Update is called once per frame
    void Update()
    {
        if(aiPath.desiredVelocity.x > 0.1)
        {
            //transform.localScale = new Vector3(-(transform.localScale.x), (transform.localScale.y), 0);
            transform.localScale = new Vector3(-defaultScale, defaultScale, 0);
        }
        else if (aiPath.desiredVelocity.x < 0.1)
        {
            //transform.localScale = new Vector3((transform.localScale.x), (transform.localScale.y), 0);
            transform.localScale = new Vector3(defaultScale, defaultScale, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        // psuedocode for later

        /* if (!wallsliding)
         * {
         *      highjump and parachute activation:
         *      if (movement ability button pressed)
         *          propel player upwards like a high jump
         *          then, animation of parachute coming out
         *          parachuting = true
         * } 
         * 
         * if(parachuting)
         * {
         *      gravity of player(?) set to low to simulate parachuting
         *      (player can still move left and right while falling)
         * }
         * else
         * {
         *      gravity = (normal gravity)
         * }
         * 
         * if(grounded || wallsliding)
         * {
         *      parachuting = false
         * }
         */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerScript : MonoBehaviour
{
    public int grapplingHook = 0;
    public GameObject grapple;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(grapplingHook == 1 && Input.GetKeyDown(KeyCode.E))
        {
            grapple.SetActive(true);
        } else if (grapplingHook == 1 && Input.GetKeyUp(KeyCode.E))
        {
            grapple.SetActive(false);
            //grapple.GetComponent<LineRenderer>().enabled = false;
            //transform.parent.gameObject.GetComponent<DistanceJoint2D>().enabled = false;
            //transform.parent.gameObject.GetComponent<PlayerControllerMk2>().SetGrapple(false);
        }
    }

    public void PickUp(string tag)
    {
        if(tag.Equals("Grapple"))
        {
            grapplingHook = 1;
        }
    }
}

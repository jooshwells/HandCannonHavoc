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
        if(grapplingHook == 1 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            grapple.SetActive(true);
        } else if (grapplingHook == 1 && Input.GetKeyUp(KeyCode.LeftShift))
        {
            grapple.SetActive(false);
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

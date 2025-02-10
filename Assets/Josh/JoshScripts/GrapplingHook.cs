using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

        // Handle Positioning
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 parentPos = parentTransform.position;
        parentPos = new Vector2(parentPos.x, parentPos.y + 0.5f);

        Vector2 direction = mousePos - parentPos;
        direction = direction.normalized;

        Vector2 circlePosition = parentPos + direction * 2;
        transform.position = circlePosition;

        // Handle Rotations
        Vector2 dir = circlePosition - parentPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, 270f + angle);

        // Lctrl
        if(isActiveAndEnabled && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("test");
            Fire();
        }

    }

    private void Fire() 
    { 
        // extend cursor with line until collision with terrain
            
        // if terrain is grappleable
            // lock position to that terrain, create spring joint between two
            // break spring joint on button release
        // else if terrain not grappleable
            // break grapple hook and nothing happens

    }


    //private void OnDrawGizmos()
    //{
    //    if (parentTransform != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(new Vector2(parentTransform.position.x, parentTransform.position.y + 0.5f), 2);
    //    }
    //}


}

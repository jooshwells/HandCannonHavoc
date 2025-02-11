using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private Transform parentTransform;
    private bool firing = false;
    private float angle;
    private const bool compTool = true;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grappleSpeed = 8f;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle Positioning
        if (!firing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 parentPos = parentTransform.position;
            parentPos = new Vector2(parentPos.x, parentPos.y + 0.5f);

            Vector2 direction = mousePos - parentPos;
            direction = direction.normalized;

            Vector2 circlePosition = parentPos + direction * 2;
            transform.position = circlePosition;

            // Handle Rotations
            Vector2 dir = circlePosition - parentPos;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, 270f + angle);
        }

        // Lctrl
        if(isActiveAndEnabled && Input.GetMouseButtonDown(0))
        {
            Vector2 prevPos = transform.position;
            firing = true;
            //Debug.Log("firing");
            StartCoroutine(Fire());
        }


    }

    //private void Fire() 
    //{
    //    // extend cursor with line until collision with terrain
    //    while (!Physics2D.OverlapCircle(transform.position, 0.2f, grappleLayer))
    //    {
    //        Debug.Log("Doing stuff");
    //        transform.position = new Vector2(transform.position.x + (float)(Mathf.Sin(45) * 0.5), transform.position.y + (float)(Mathf.Cos(45) * 0.5));
    //    }
            
    //    // if terrain is grappleable
    //        // lock position to that terrain, create spring joint between two
    //        // break spring joint on button release
    //    // else if terrain not grappleable
    //        // break grapple hook and nothing happens

    //}

    IEnumerator Fire()
    {
        while (!Physics2D.OverlapCircle(transform.position, 0.2f, grappleLayer))
        {
            Vector2 direction = transform.up;
            transform.position += (Vector3)(direction * grappleSpeed * Time.deltaTime);

            yield return null; // Wait for the next frame
        }

        SpringJoint2D spring = player.GetComponent<SpringJoint2D>();
        spring.enabled = compTool;
        spring.connectedAnchor = transform.position;

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

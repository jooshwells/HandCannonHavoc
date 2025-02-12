using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private Transform parentTransform;
    private bool firing = false;
    private float angle;
    private const bool compTool = true;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float grappleSpeed;
    [SerializeField] private float springFrequency = 1.5f;
    [SerializeField] private float springDampingRatio = 0.7f;
    [SerializeField] private float minDistance = 0.5f;

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
            firing = true;
            //Debug.Log("firing");
            StartCoroutine(Fire());
        }

        if(player.GetComponent<SpringJoint2D>() != null && Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(player.GetComponent<SpringJoint2D>());
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            
            rb.velocity = new Vector2(rb.velocity.x, 16f);
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
        Vector2 direction = transform.up;

        while (!Physics2D.OverlapCircle(transform.position, 0.2f, grappleLayer))
        {

            transform.position += (Vector3)(direction * grappleSpeed * Time.deltaTime);

            yield return null; // Wait for the next frame
        }

        // Collided with grappleable
        AttachSpringJoint();


        firing = false;

        
    }

    private void AttachSpringJoint()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        SpringJoint2D spring = player.AddComponent<SpringJoint2D>();
        spring.connectedAnchor = transform.position;
        spring.enableCollision = true;

        spring.autoConfigureDistance = false;
        float currentDistance = Vector2.Distance(player.transform.position, transform.position);
        spring.distance = Mathf.Max(currentDistance, minDistance);

        spring.frequency = springFrequency;
        spring.dampingRatio = springDampingRatio;

        //while (true)
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        Destroy(spring);
        //        break;
        //    }
        //}
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

using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StiffGrapple : MonoBehaviour
{
    private int grappleLayer; 
    [SerializeField] private float maxDist = 10f;

    private LineRenderer lr;
    private DistanceJoint2D dj;
    private bool grappling = false;

    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        grappleLayer = LayerMask.GetMask("Ground");
        lr = GetComponent<LineRenderer>();
        dj = GetComponentInParent<DistanceJoint2D>();
        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.Raycast(transform.position, direction, maxDist, grappleLayer);
            if (hit.collider != null)
            {
                //Debug.DrawRay(transform.position, direction * maxDist, Color.red, 2f);
                
                Debug.Log("Collided at " + hit.point);
                
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hit.point);
                lr.enabled = true;

                dj.enabled = true;
                dj.connectedAnchor = hit.point;
                GetComponent<AimingCopy>().Freeze();

                grappling = true;
                GetComponentInParent<PlayerControllerMk2>().SetGrapple(true);
            }
        }

        if (grappling)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);

            GetComponentInParent<SwingyScript>().enabled = true;

            GetComponentInParent<SwingyScript>().SetPendLength(dj.distance);

        }

        if(grappling && Input.GetKeyDown(KeyCode.Space))
        {
            lr.enabled = false;
            dj.enabled = false;
            grappling = false;
            GetComponent<AimingCopy>().UnFreeze();
            GetComponentInParent<SwingyScript>().enabled = false;
            GetComponentInParent<PlayerControllerMk2>().SetGrapple(false);

        }
    }     

}

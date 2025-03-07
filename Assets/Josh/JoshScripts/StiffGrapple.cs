using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StiffGrapple : MonoBehaviour
{
    private int grappleLayer; 
    [SerializeField] private float maxDist = 100f;

    private LineRenderer lr;
    private DistanceJoint2D dj;
    private bool grappling = false;

    private RaycastHit2D[] hits;
    private RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        grappleLayer = LayerMask.GetMask("Grapple");
        lr = GetComponent<LineRenderer>();
        dj = GetComponentInParent<DistanceJoint2D>();
        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            hits = Physics2D.RaycastAll(transform.position, direction, maxDist, ~0); // ~0 indicates all possible layers are targeted
            //Debug.DrawRay(transform.position, direction * maxDist, Color.red, 2f);

            if(hits.Length != 0)
            {
                hit = hits[0];

                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Grapple"))
                {

                    //Debug.Log("Collided at " + hit.point);

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

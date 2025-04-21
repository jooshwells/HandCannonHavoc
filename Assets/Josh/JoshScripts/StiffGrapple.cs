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
    private RaycastHit2D grappleHit;
    private RaycastHit2D hit;

    [SerializeField] private AudioClip grapSFX;

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
                    transform.GetComponentInParent<AudioSource>().clip = grapSFX;
                    transform.GetComponentInParent<AudioSource>().pitch = Random.Range(1.15f, 1.25f);
                    transform.GetComponentInParent<AudioSource>().Play();
                    grappleHit = hit;
                    //Debug.Log("Collided at " + hit.point);

                    lr.SetPosition(0, transform.parent.position);
                    lr.SetPosition(1, grappleHit.point);
                    
                    lr.material.mainTextureScale = new Vector2(Vector2.Distance(grappleHit.point, transform.parent.position) / 2f, 1);
                    
                    lr.enabled = true;

                    dj.enabled = true;
                    dj.connectedAnchor = hit.point;
                    GetComponentInChildren<AimingCopy>().Freeze();

                    grappling = true;
                    GetComponentInParent<PlayerControllerMk2>().SetGrapple(true);
                }
            }
            
        }

        if (grappling)
        {
            lr.material.mainTextureScale = new Vector2(Vector2.Distance(grappleHit.point, transform.parent.position) / 2f, 1);

            lr.SetPosition(0, new Vector3(transform.parent.position.x, transform.parent.position.y, 1));
            lr.SetPosition(1, (Vector3)grappleHit.point + new Vector3(0, 0, 1f));

            GetComponentInParent<SwingyScript>().enabled = true;

            GetComponentInParent<SwingyScript>().SetPendLength(dj.distance);

        }

        if (grappling && Input.GetKeyDown(KeyCode.Space))
        {
            lr.enabled = false;
            dj.enabled = false;
            grappling = false;
            GetComponentInChildren<AimingCopy>().UnFreeze();
            GetComponentInParent<SwingyScript>().enabled = false;
            GetComponentInParent<PlayerControllerMk2>().SetGrapple(false);

        }
    }

    public void Reset()
    {
        //Debug.Log("trying to reset");
        GetComponent<LineRenderer>().enabled = false;
        transform.parent.gameObject.GetComponent<DistanceJoint2D>().enabled = false;
        if(GetComponentInChildren<AimingCopy>().IsFrozen()) GetComponent<AimingCopy>().UnFreeze();
        if(grappling == true) grappling = false;
        GetComponentInParent<SwingyScript>().enabled = false;

        transform.parent.GetComponent<PlayerControllerMk2>().AbilControlResetGrapple();
    }

}

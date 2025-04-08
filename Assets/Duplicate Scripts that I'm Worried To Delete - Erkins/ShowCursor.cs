using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    [SerializeField] private Material regularGrappleMaterial;
    [SerializeField] private Material aimingMaterial;
    private SpriteRenderer sr;

    private Vector3 startPos;
    private Vector3 endPos;
    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        startPos = lr.GetPosition(0);
        endPos = lr.GetPosition(1);
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        float lineLength = 10f;  // Example distance


        if (Input.GetMouseButtonDown(1))
        {
            sr.enabled = true;

            lr.material = aimingMaterial;
            Color c = new Color(0.7f, 0f, 0f, 0.5f); // dimmer red + more transparent

            //c.a = 0.5f;

            lr.startColor = c;
            lr.endColor = c;
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 endPoint = (Vector2)transform.position + direction * lineLength;
            lr.SetPosition(0, transform.position); // Start point
            lr.SetPosition(1, endPoint); // End point
        }

        if (Input.GetMouseButtonUp(1))
        {
            //lr.material = regularGrappleMaterial;
            //Color c = Color.white;
            //lr.startColor = c;
            //lr.endColor = c;

            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);

            sr.enabled = false;
        }
    }


}

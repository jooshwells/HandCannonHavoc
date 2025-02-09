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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 parentPos = parentTransform.position;
        parentPos = new Vector2(parentPos.x, parentPos.y + 0.5f);

        Vector2 direction = mousePos - parentPos;
        direction = direction.normalized;

        Vector2 circlePosition = parentPos + direction * 2;
        transform.position = circlePosition;
    }

    private void OnDrawGizmos()
    {
        if (parentTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector2(parentTransform.position.x, parentTransform.position.y + 0.5f), 2);
        }
    }

}

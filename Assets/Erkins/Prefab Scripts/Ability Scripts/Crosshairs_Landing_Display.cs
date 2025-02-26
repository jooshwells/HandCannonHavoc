using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    public GameObject orbPrefab;

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0)) // Left-click to shoot orb
        {
            ShootOrb();
        }
    }

    void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }

    void ShootOrb()
    {
        Instantiate(orbPrefab, transform.position, Quaternion.identity); // Spawn orb
        Destroy(gameObject); // Destroy crosshairs
    }
}

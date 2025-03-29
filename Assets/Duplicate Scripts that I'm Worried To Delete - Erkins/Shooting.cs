using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject[] gunPrefabs; // Store full gun prefabs
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeAlive;
    [SerializeField] public Transform muzzlePoint; // Assign in Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            ShootGun();
        }
    }

    void ShootGun()
    {
        // Pick a random gun prefab
        GameObject chosenGunPrefab = gunPrefabs[Random.Range(0, gunPrefabs.Length)];

        // Instantiate the prefab
        GameObject gunInstance = Instantiate(chosenGunPrefab, muzzlePoint.position, Quaternion.identity);

        // Get the Rigidbody2D component
        Rigidbody2D gunBullet = gunInstance.GetComponent<Rigidbody2D>();

        if (gunBullet != null)
        {
            // Calculate direction from muzzle to mouse
            Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - muzzlePoint.position).normalized;

            // Apply the calculated direction to the bullet's velocity
            gunBullet.velocity = direction * bulletSpeed;
        }

        Destroy(gunInstance, timeAlive);
    }
}

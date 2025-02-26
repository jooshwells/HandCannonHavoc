using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcGunScript : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform gun;
    public float fireRate = 1f; // Fire every 0.5 seconds

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 0f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {

        Instantiate(projectile, gun.position, gun.rotation);

    }
}

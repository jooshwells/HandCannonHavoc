using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcGunScript : MonoBehaviour
{
    public npcScript npc;

    //[SerializeField] private GameObject projectile;
    [SerializeField] private Transform gun;

    [SerializeField] private Transform player;
    [SerializeField] GameObject bulletSprite;
    [SerializeField] Transform gunPos;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float bulletDuration = 1f;
    [SerializeField] float fireRate = 1f;

    private float nextBullet = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (npc.isAttacking() && Time.time >= nextBullet)
        {
            fireGun();
            nextBullet = Time.time + fireRate;
        }
    }
    private void fireGun()
    {
        GameObject bullet = Instantiate(bulletSprite, gunPos.position, gunPos.rotation);
        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.enabled = true; // Make the bullet visible
        }

        Vector2 direction = (player.position - gun.position);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = direction * bulletSpeed;
        //flip(direction, bulletRenderer);
        Destroy(bullet, bulletDuration);
    }
}

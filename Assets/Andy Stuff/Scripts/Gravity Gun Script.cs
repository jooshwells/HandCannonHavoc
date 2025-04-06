using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityGunScript : MonoBehaviour 
{

    private Transform player;
    [SerializeField] GameObject bulletSprite;

    [SerializeField] Transform gunPos;
    [SerializeField] Transform gunBarrel;

    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float bulletDuration = 10f;
    [SerializeField] float fireRate = .5f;
    [SerializeField] float attackDamage = 5f;

    private float nextBullet = 0f;

    [SerializeField] int magSize = 10;
    [SerializeField] float reloadSpeed = 2f;
    private int currentAmmo;
    private bool isReloading = false;


  
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magSize;
        player =transform.parent.parent;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Bullets")); // prevent bullet collision
    }

    // reset ammo when swapping between guns
    void OnEnable()
    {
        currentAmmo = magSize;
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading) return;

        if(currentAmmo <=0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < magSize))
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetMouseButton(0) && Time.time >= nextBullet) // left click
        {
            shoot();
            nextBullet = Time.time +fireRate;
        }
    }
    void shoot()
    {
        if(currentAmmo <=0) return;
        currentAmmo--;


        GameObject bullet = Instantiate(bulletSprite, gunBarrel.position, gunPos.rotation);    
        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.enabled = true; // Make the bullet visible
        }
        BlackHole bulletScript = bullet.GetComponent<BlackHole>();
        bulletScript.SetInstantiator(gameObject);
        bulletScript.SetAttackDamage(attackDamage);


        Vector2 direction = (gunPos.right).normalized;

        // deal with gun firing inwards
        if(player.transform.localScale.x <0) 
        {
            direction = -(gunPos.right).normalized;
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;


        rb.velocity = direction * bulletSpeed;
        flip(direction, bulletRenderer);
        Destroy(bullet, bulletDuration);
    }
    
    // spagetthi code for handling weird sprite flipping
    void flip(Vector2 dir, SpriteRenderer bullet)
    {
        Vector3 cur = bullet.transform.localScale;

        if(dir.x > 0 && gunPos.localScale.y >0)
        {
            bullet.transform.localScale = new Vector3(cur.x, cur.y, cur.z);
        }
        if(dir.x<0 && gunPos.localScale.y >0) 
        {
            bullet.transform.localScale = new Vector3(-1*cur.x, cur.y, cur.z);
        }
        if(dir.x > 0 && gunPos.localScale.y <0)
        {
            bullet.transform.localScale = new Vector3(-1*cur.x,-1* cur.y, cur.z);
        }
        if(dir.x < 0 && gunPos.localScale.y <0)
        {
            bullet.transform.localScale = new Vector3(cur.x, -1*cur.y, cur.z);
        }
       
    }

    IEnumerator Reload()
    {
        isReloading = true;
    

        yield return new WaitForSeconds(reloadSpeed);

        currentAmmo = magSize;
        isReloading = false;
    }

}

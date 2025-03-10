using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    private Transform player;
    [SerializeField] GameObject bulletSprite;
    [SerializeField] Transform gunPos;
    [SerializeField] Transform gunBarrel;

    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float bulletDuration = 1f;
    [SerializeField] float fireRate = .5f;
    [SerializeField] float attackDamage = 5f;

    private float nextBullet = 0f;

    [SerializeField] int magSize = 10;
    [SerializeField] float reloadSpeed = 2f;
    [SerializeField] int pellets = 5;
    [SerializeField] float spreadAngle = 30;
    [SerializeField] float recoil = 5f;



    private int currentAmmo;
    private bool isReloading = false;
    private Rigidbody2D playerRb;



  
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magSize;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Bullets")); // prevent bullet collision
        player =transform.parent.parent;
        playerRb = transform.parent.parent.GetComponent<Rigidbody2D>();  //used for recoil

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
    if(currentAmmo <= 0) return;
    currentAmmo--;


    // loop to shoot pellets
    for (int i = 0; i < pellets; i++)
    {
        // spread calculations
        float spreadStep = spreadAngle / pellets; // even spread
        float spread = spreadStep * (i - (pellets - 1) / 2f); // offset by index of i
        Vector2 direction = (gunPos.right).normalized;

        // hanlde firing inwards issues
        if(player.transform.localScale.x <0)
        {
            direction = -(gunPos.right).normalized;
        }

        // apply spread
        direction = Quaternion.Euler(0, 0, spread) * direction;

        // instantiate bullet 
        GameObject bullet = Instantiate(bulletSprite, gunBarrel.position, gunPos.rotation);
        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.enabled = true; 
        }
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetInstantiator(gameObject);
        bulletScript.SetAttackDamage(attackDamage);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
        flip(direction, bulletRenderer);
        Destroy(bullet, bulletDuration);
        
        if(i==0) Recoil(direction); // apply recoil only once
    }
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
    void Recoil(Vector2 shotDirection)
    {
        float horizontalRecoil = -shotDirection.x * recoil *.6f;
        float verticalRecoil = -shotDirection.y * recoil;

        playerRb.AddForce(new Vector2(horizontalRecoil,verticalRecoil), ForceMode2D.Impulse);
    
    }
}

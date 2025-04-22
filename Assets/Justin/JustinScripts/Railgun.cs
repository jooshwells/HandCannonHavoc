/*

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Railgun : customShoot
{
    public Image bar;

    private void Awake()
    {
        bulletSpeed = 500f;
        //bulletDuration default
        fireRate = 1f;
        //attackDamage = 30f;
        magSize = 4;
        reloadSpeed = 4f;
    }

    protected override void Update()
    {
        base.Update();
        if 
    }

    protected override void shoot()
    {
        base.shoot();

    }



    protected override bool shootingInput() //shoot based on holding rather than press
    {
        genHoldButton hold = gameObject.GetComponent<genHoldButton>();
        hold.setHoldNeeded(1f); // # seconds to hold to fire
        hold.isHolding(Input.GetMouseButton(0) && Time.time >= nextBullet); // left click
        return hold.chargeExecute();
    }
}

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun2 : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] GameObject bulletSprite;

    [SerializeField] Transform gunPos;
    [SerializeField] Transform gunBarrel;

    [SerializeField] float bulletSpeed = 10f; //
    [SerializeField] float bulletDuration = 10f;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float attackDamage = 30f; //

    private float nextBullet = 0f;

    [SerializeField] int magSize = 10;
    [SerializeField] float reloadSpeed = 2f;
    [SerializeField] float recoil = 5f;

    private int currentAmmo;
    private bool isReloading = false;
    private Rigidbody2D playerRb;

    //RAILGUN COMPONENTS
    genHoldButton hold;
    [SerializeField] SpriteRenderer render;
    [SerializeField] Sprite[] frames;
    int frame_index;

    // Start is called before the first frame update
    void Start()
    {
        hold = gameObject.GetComponent<genHoldButton>();

        currentAmmo = magSize;
        //player = transform.parent.parent;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Bullets")); // prevent bullet collision
        playerRb = player.GetComponent<Rigidbody2D>();  //used for recoil
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
        if (isReloading) return;

        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo < magSize))
        {
            StartCoroutine(Reload());
            return;
        }
        if (shootingInput())
        {
            shoot();
            nextBullet = Time.time + fireRate;
        }

        //Railgun Frames
        if (hold.progress == 0)
            frame_index = 0;
        else
            frame_index = (int)((hold.progress * 7) + 1);

        if (frame_index >= 8) 
            frame_index = 8;

        render.sprite = frames[frame_index];
    }

    bool shootingInput() //shoot based on holding rather than press
    {
        hold.setHoldNeeded(1f); // # seconds to hold to fire
        hold.isHolding(Input.GetMouseButton(0) && Time.time >= nextBullet); // left click
        return hold.chargeExecute();
    }

    void shoot()
    {
        if (currentAmmo <= 0) return;
        currentAmmo--;

        Vector2 direction = (gunPos.right).normalized;

        // hanlde firing inwards issues
        if (player.transform.localScale.x < 0)
        {
            direction = -(gunPos.right).normalized;
        }


        GameObject bullet = Instantiate(bulletSprite, gunBarrel.position, gunPos.rotation);
        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.enabled = true; // Make the bullet visible
        }
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetInstantiator(gameObject);
        bulletScript.SetAttackDamage(attackDamage);


        //Vector2 direction = (gunPos.right).normalized;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;


        rb.velocity = direction * bulletSpeed;
        flip(direction, bulletRenderer);
        Destroy(bullet, bulletDuration);

        Recoil(direction); // apply recoil only once

    }

    // spagetthi code for handling weird sprite flipping
    void flip(Vector2 dir, SpriteRenderer bullet)
    {
        Vector3 cur = bullet.transform.localScale;

        if (dir.x > 0 && gunPos.localScale.y > 0)
        {
            bullet.transform.localScale = new Vector3(cur.x, cur.y, cur.z);
        }
        if (dir.x < 0 && gunPos.localScale.y > 0)
        {
            bullet.transform.localScale = new Vector3(-1 * cur.x, cur.y, cur.z);
        }
        if (dir.x > 0 && gunPos.localScale.y < 0)
        {
            bullet.transform.localScale = new Vector3(-1 * cur.x, -1 * cur.y, cur.z);
        }
        if (dir.x < 0 && gunPos.localScale.y < 0)
        {
            bullet.transform.localScale = new Vector3(cur.x, -1 * cur.y, cur.z);
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
        float horizontalRecoil = -shotDirection.x * recoil * .6f;
        float verticalRecoil = -shotDirection.y * recoil;

        playerRb.AddForce(new Vector2(horizontalRecoil, verticalRecoil), ForceMode2D.Impulse);

    }

}

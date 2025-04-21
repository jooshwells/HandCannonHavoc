using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BurstGun : MonoBehaviour 
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
    [SerializeField] int burstCount = 4;
    [SerializeField] float burstDelay = .1f;
    [SerializeField] private GameObject reloadUIObject;
    [SerializeField] private Image reloadBar;
    private RectTransform barRect;
    private float originalWidth;
    //ammo bar stuff
    [SerializeField] private Image ammoBar;


    private int currentAmmo;
    private bool isReloading = false;


  
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magSize;
        player =transform.parent.parent;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Bullets")); // prevent bullet collision
        if (reloadBar != null)
        {
            barRect = reloadBar.rectTransform;
            originalWidth = barRect.sizeDelta.x;
            barRect.sizeDelta = new Vector2(0, barRect.sizeDelta.y);
            reloadUIObject.SetActive(false);
        }
    }

    // reset ammo when swapping between guns
     void OnEnable()
    {
        ResetBars();
        UpdateAmmoBar();
    }
    void OnDisable()
    {
        StopAllCoroutines(); //cancel reload
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
            StartCoroutine(burstShoot());
            nextBullet = Time.time +fireRate;
        }
    }

    IEnumerator burstShoot()
    {
        if(currentAmmo <=0) yield break;
        int shotsFired =0;
        while (shotsFired < burstCount && currentAmmo > 0)
        {
            shoot();
            shotsFired++;
            yield return new WaitForSeconds(burstDelay);
        }


    }
    void shoot()
    {
        if(currentAmmo <=0) return;
        currentAmmo--;
        UpdateAmmoBar();


        GameObject bullet = Instantiate(bulletSprite, gunBarrel.position, gunPos.rotation);    
        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.enabled = true; // Make the bullet visible
        }
        Bullet bulletScript = bullet.GetComponent<Bullet>();
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
        reloadUIObject.SetActive(true);
        float elapsed = 0f;
        originalWidth = 200f;

        while (elapsed < reloadSpeed)
        {
            float width = Mathf.Lerp(0, originalWidth, elapsed / reloadSpeed);
            barRect.sizeDelta = new Vector2(width, barRect.sizeDelta.y);
            elapsed += Time.deltaTime;
            yield return null;
        }
        barRect.sizeDelta = new Vector2(originalWidth, barRect.sizeDelta.y);
        reloadUIObject.SetActive(false);

        currentAmmo = magSize;
        isReloading = false;
        UpdateAmmoBar();

    }
     void UpdateAmmoBar()
    {
        float ammoPercent = Mathf.Clamp((float)currentAmmo / magSize, 0f, 1f);
        ammoBar.fillAmount = ammoPercent;
    }
    public void ResetBars()
    {
        ammoBar.fillAmount = (float)currentAmmo / magSize;
        reloadUIObject.SetActive(false);
        reloadBar.fillAmount = 0f; 
    }

}

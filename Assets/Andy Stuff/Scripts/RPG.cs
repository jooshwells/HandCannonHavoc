using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPG : MonoBehaviour 
{

    private Transform player;
    [SerializeField] GameObject bulletSprite;

    [SerializeField] Transform gunPos;
    [SerializeField] Transform gunBarrel;

    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float bulletDuration = 1f;
    [SerializeField] float fireRate = .5f;
    [SerializeField] float attackDamage = 50f;

    private float nextBullet = 0f;

    [SerializeField] int magSize = 1;
    [SerializeField] float reloadSpeed = 2f;
    [SerializeField] float recoil = 20f;
    [SerializeField] private GameObject reloadUIObject;
    [SerializeField] private Image reloadBar;
    private RectTransform barRect;
    private float originalWidth;
    //ammo bar stuff
    [SerializeField] private Image ammoBar;

    private int currentAmmo;
    private bool isReloading = false;
    private Rigidbody2D playerRb;



  
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magSize;
        UpdateAmmoBar();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Bullets"), LayerMask.NameToLayer("Bullets")); // prevent bullet collision
        player =transform.parent.parent;
        playerRb = transform.parent.parent.GetComponent<Rigidbody2D>();  //used for recoil
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
            shoot();
            nextBullet = Time.time +fireRate;
        }
    }
    void shoot()
    {
        if(currentAmmo <=0) return;
        currentAmmo--;
        UpdateAmmoBar();


        GameObject rpg = Instantiate(bulletSprite, gunBarrel.position, gunPos.rotation);    
        SpriteRenderer bulletRenderer = rpg.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.enabled = true; // Make the bullet visible
        }
        RPGboomy bulletScript = rpg.GetComponent<RPGboomy>();
        bulletScript.SetInstantiator(gameObject);
        bulletScript.SetAttackDamage(attackDamage);


        Vector2 direction = (gunPos.right).normalized;

        // deal with gun firing inwards
        if(player.transform.localScale.x <0) 
        {
            direction = -(gunPos.right).normalized;
        }

        Rigidbody2D rb = rpg.GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;


        rb.velocity = direction * bulletSpeed;
        flip(direction, bulletRenderer);
        Recoil(direction);
        Destroy(rpg, bulletDuration);
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
    void Recoil(Vector2 shotDirection)
    {
        float horizontalRecoil = -shotDirection.x * recoil *.6f;
        float verticalRecoil = -shotDirection.y * recoil;

        playerRb.AddForce(new Vector2(horizontalRecoil,verticalRecoil), ForceMode2D.Impulse);
    
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

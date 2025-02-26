using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class copyShoot : MonoBehaviour
{


    [SerializeField] GameObject bulletSprite;
    [SerializeField] GameObject reloadSprite;

    [SerializeField] Transform gunPos;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float bulletDuration = 1f;
    [SerializeField] float fireRate = .5f;
    private float nextBullet = 0f;

    [SerializeField] int magSize = 10;
    [SerializeField] float reloadSpeed = 2f;
    private int currentAmmo;
    private bool isReloading = false;



    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magSize;
        reloadSprite.SetActive(false);

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
        if (Input.GetMouseButton(0) && Time.time >= nextBullet) // left click
        {
            shoot();
            nextBullet = Time.time + fireRate;
        }
    }
    void shoot()
    {
        if (currentAmmo <= 0) return;
        currentAmmo--;


        GameObject bullet = Instantiate(bulletSprite, gunPos.position + new Vector3(1.5f, -0.5f, 0), gunPos.rotation);
        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();
        if (bulletRenderer != null)
        {
            bulletRenderer.enabled = true; // Make the bullet visible
        }

        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - gunPos.position).normalized;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = direction * bulletSpeed;
        flip(direction, bulletRenderer);
        Destroy(bullet, bulletDuration);
    }

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
        reloadSprite.SetActive(true);

        yield return new WaitForSeconds(reloadSpeed);

        currentAmmo = magSize;
        isReloading = false;
        reloadSprite.SetActive(false);
    }
}

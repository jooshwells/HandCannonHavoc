using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copyProjectileScript : MonoBehaviour
{
    [Header("AttackParams")]
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float xAttackPower;
    [SerializeField] private float yAttackPower;

    private GameObject target;
    private Rigidbody2D rb;
    private GameObject instantiator;


    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<AudioSource>().Play();

    }

    public void SetInstantiator(GameObject inst)
    {
        instantiator = inst;
    }

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        Transform targetPos = target.transform;
        Vector2 dist = (targetPos.position + new Vector3(0, 0.1f, 0)) - transform.position;
        float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        //Debug.Log("Angle = " + (angle+360f));
        if (angle + (angle < 0f ? 360 : 0) >= 90 && angle + (angle < 0f ? 360 : 0) <= 270)
        {
            Vector3 locScale = transform.localScale;
            locScale.y *= -1;
            transform.localScale = locScale;
        }


        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 dir = dist.normalized;
        rb.velocity = dir * new Vector2(xSpeed, ySpeed);
    }

    public int damage = 20;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle the player being hit…
            target.GetComponent<PlayerHealthScript>().Hit(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy")) // Make bullet pass through enemies
        {
            Collider2D projectileCollider = GetComponent<Collider2D>();
            // Use 'collision' directly because it is the enemy collider.
            Physics2D.IgnoreCollision(projectileCollider, collision);
        }
        else if (instantiator == null || (!collision.CompareTag(instantiator.tag)))
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

}

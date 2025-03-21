using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
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
        if(angle+ (angle < 0f ? 360 : 0) >= 90 && angle+ (angle < 0f ? 360 : 0) <= 270)
        {
            Vector3 locScale = transform.localScale;
            locScale.y *= -1;
            transform.localScale = locScale;
        } 
        

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 dir = dist.normalized;
        rb.velocity = dir * new Vector2 (xSpeed, ySpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Player Hit");

            /*
             * Play Exploding Bird Animation Through Coroutine
            */
            

            Vector2 dist = transform.position - (target.transform.position + new Vector3(0, 1f, 0));
            Vector2 dir = dist.normalized;

            target.GetComponent<PlayerControllerMk2>().KnockBack(new Vector2(xAttackPower*-dir.x, yAttackPower), 0.2f, 20); // last parameter is damage, set to 5 arbitrarily
            Destroy(gameObject);
        }
        else if (instantiator == null || !(collision.CompareTag(instantiator.tag))) {
            {
                Destroy(gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
     
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    [Header("AttackSpeed")]
    [SerializeField] private float x;
    [SerializeField] private float y;

    private GameObject target;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();

        Transform targetPos = target.transform;
        Vector2 dist = targetPos.position - transform.position;
        float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 dir = dist.normalized;
        rb.velocity = dir * new Vector2 (x, y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

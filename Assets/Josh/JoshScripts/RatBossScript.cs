using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBossScript : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Attack Types")]
    [SerializeField] private GameObject fireball;

    [Header("Attack Params")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float detectDist;


    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector2.Distance(player.position + new Vector3(0, 0.5f, 0), transform.position);

        Vector2 distanceVector = (player.position + new Vector3(0, 0.5f, 0)) - transform.position;
        //Debug.Log(distanceVector.x + " " + distanceVector.y);

        if ((distanceVector.x < 0f && transform.localScale.x > 0f) ||
            (distanceVector.x > 0f && transform.localScale.x < 0f))
        {
            Vector3 loc = transform.localScale;
            loc.x *= -1;
            transform.localScale = loc;
        }

        if (!attacking)
        {
            if (dist <= detectDist)
            {
                Debug.Log("trying to attack");
                attacking = true;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        GameObject curFireball = Instantiate(fireball, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        curFireball.GetComponent<ProjectileScript>().SetInstantiator(gameObject);
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }
}

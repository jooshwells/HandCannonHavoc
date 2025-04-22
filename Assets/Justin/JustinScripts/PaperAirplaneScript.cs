using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperAirplaneScript : MonoBehaviour
{
    Transform transform;
    Transform target;
    [SerializeField] GameObject bomb;

    [SerializeField] float attackCooldown =2f;
    bool dropped = false;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            target = GameObject.FindGameObjectWithTag("Player").transform;


        
        if (Mathf.Abs(target.position.x - transform.position.x) < 10f)
        {
            if(!dropped)
            {
                dropped = true;
                StartCoroutine(Attack());
            }
        }
    }
    IEnumerator Attack()
    {
        GameObject instBomb = Instantiate(bomb, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        //instBomb.GetComponent<ProjectileScript>().SetInstantiator(gameObject);
        yield return new WaitForSeconds(attackCooldown);
        dropped = false;
    }
}

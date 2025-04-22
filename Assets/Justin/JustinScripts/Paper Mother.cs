using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PaperMother : MonoBehaviour
{
    [SerializeField] GameObject paperPlane;
    [SerializeField] GameObject paperMinion;
    [SerializeField] float planeSpeed = 5;
    [SerializeField] float attackCooldown = 8f;
    [SerializeField] float randomAttackRange = 2f;

    public bool dropped = false;
    public bool dropped2 = false;

    [SerializeField] Transform target;
    bool close = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (Vector3.Distance(target.position, gameObject.transform.position) < 12f)
        {
            close = true;
        }
       
        if (!dropped && !close)
        {
            dropped = true;
            StartCoroutine(Attack());
        }

        if (!dropped2 && close)
        {
            dropped2 = true;
            StartCoroutine(Attack2());
        }
    }
    IEnumerator Attack()
    {
        GameObject plane = Instantiate(paperPlane, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        plane.GetComponent<Rigidbody2D>().velocity = new Vector3(-planeSpeed,0);
        yield return new WaitForSeconds(Random.Range(attackCooldown - (randomAttackRange / 2), attackCooldown + (randomAttackRange/2)));
        dropped = false;
    }

    IEnumerator Attack2()
    {
        GameObject minion = Instantiate(paperMinion, transform.Find("LaunchOrigin2").position, transform.Find("LaunchOrigin2").rotation);
        minion.GetComponent<CloudyJit>().target = GameObject.FindGameObjectWithTag("Player").transform;
        minion.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
        yield return new WaitForSeconds( Random.Range(attackCooldown - (randomAttackRange / 2), attackCooldown + (randomAttackRange / 2)) / 10 );
        dropped = false;
    }
}

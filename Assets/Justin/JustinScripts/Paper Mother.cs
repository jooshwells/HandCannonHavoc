using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PaperMother : MonoBehaviour
{
    [SerializeField] GameObject paperPlane;
    [SerializeField] float planeSpeed = 5;
    [SerializeField] float attackCooldown = 8f;
    [SerializeField] float randomAttackRange = 2f;

    bool dropped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if condition?
       
        if (!dropped)
        {
            dropped = true;
            StartCoroutine(Attack());
        }
      
    }
    IEnumerator Attack()
    {
        GameObject plane = Instantiate(paperPlane, transform.Find("LaunchOrigin").position, transform.Find("LaunchOrigin").rotation);
        plane.GetComponent<Rigidbody2D>().velocity = new Vector3(-planeSpeed,0);
        yield return new WaitForSeconds(Random.Range(attackCooldown - (randomAttackRange / 2), attackCooldown + (randomAttackRange/2)));
        dropped = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BeeHiveScript : MonoBehaviour
{

    [SerializeField] private float spawnRadius = 5f; // spawn bees when player is in range
    [SerializeField] private int totalBees = 3;
    [SerializeField] private float spawnDelay = 1.5f;

    [SerializeField] private GameObject beeEnemy;
    [SerializeField] private Transform spawnPoint;
    private Transform target;

    private bool isAlive = true;
    private bool spawnTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player")!=null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;    
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                target = GameObject.FindGameObjectWithTag("Player").transform;

            if(target == null)
            {
                return;
            }
        }
        playerCheck();
    }
    void playerCheck()
    {

        if(spawnTrigger || target == null)
        {
            return;
        } 

        float playerDist = Vector2.Distance(transform.position, target.position);
        if (playerDist <= spawnRadius)
        {
            StartCoroutine(SpawnBees());
        }
    }

    IEnumerator SpawnBees()
    {
        spawnTrigger = true;
        for(int i=0; i<totalBees; i++)
        {
            Instantiate(beeEnemy,spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }


}

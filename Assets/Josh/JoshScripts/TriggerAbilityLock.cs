using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAbilityLock : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Variant");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player.GetComponentInChildren<AbilityControllerScript>().TurnEverythingOffBut(4);
        }
    }
}

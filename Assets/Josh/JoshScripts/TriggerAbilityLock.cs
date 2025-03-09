using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAbilityLock : MonoBehaviour
{
    private GameObject player;
    private AbilityControllerScript ac;
    [SerializeField] private enum Selection
    {
        Dash = 1,
        Grapple = 2,
        HighJump_Para = 3,
        Bounce_Pad = 4
    }
    [SerializeField] private Selection selection;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Variant");
        ac = player.GetComponentInChildren<AbilityControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Selection was " + (int)selection);
            ac.TurnEverythingOffBut((int)selection);
            if (!ac.IsLocked())
            {
                ac.LockAllBut((int)selection);
            } else
            {
                ac.LockAllBut(0);
            }

        }
    }
}

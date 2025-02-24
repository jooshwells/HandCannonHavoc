using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    private GameObject g;
    private RoomManager rm;
    private bool passedThrough;

    private void Start()
    {
        g = GameObject.Find("RoomManager");
        rm = g.GetComponent<RoomManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!passedThrough)
            {
                rm.GoToRoom(rm.GetCurrentRoom() + 1);
                passedThrough = true;
            } 
            else
            {
                rm.GoToRoom(rm.GetCurrentRoom() - 1);
                passedThrough = false;
            }
        }
        
    }
}

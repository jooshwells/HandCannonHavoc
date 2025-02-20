using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<Camera> rooms;

    private Camera currentCam;
    [SerializeField] private Tutorial_GrapplingGun gp;

    private void Start()
    {
        // Set current room to starting room
        currentCam = rooms[0];
        currentCam.gameObject.SetActive(true);

        //// Get Gun Pivot Changing Script
        //gp = GameObject.Find("Player").GetComponentInChildren<Tutorial_GrapplingGun>();
        gp.SetCam(currentCam);
    }
    
    // Switches camera to room newRoom - 1 (zero-based indexing)
    public void GoToRoom(int newRoom)
    {
        Camera newCam = rooms[newRoom-1];

        currentCam.gameObject.SetActive(false);
        newCam.gameObject.SetActive(true);
        currentCam = newCam;

        gp.SetCam(currentCam);
    }

    public int GetCurrentRoom()
    {
        return rooms.IndexOf(currentCam)+1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] int zoom;
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 3, zoom);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Script : MonoBehaviour
{
    [SerializeField] private Transform player;

    public float dist;
    public float detectDist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         dist = Vector2.Distance(player.position + new Vector3(0, 0.5f, 0), transform.position);
        
    }
}

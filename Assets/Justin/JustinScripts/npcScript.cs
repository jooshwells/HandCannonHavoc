using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPC_Script : MonoBehaviour
{
    private ProjectileScript projectile;

    [SerializeField] private Transform player;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float detectDist;
    [SerializeField] private float speed = 4f;

    [SerializeField] private GameObject weapon;

    private float dist;
    private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector2.Distance(player.position + new Vector3(0, 0.5f, 0), transform.position);
        if (dist < detectDist)
        {
            attacking = true;
            attackPlayer();
        }
        else
        {
            attacking = false;
            stopAttacking();
        }
    }

    private void attackPlayer()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        
        //attacking animation, pull out gun
        weapon.SetActive(true); //for now, just activate the gun object
    }

    private void stopAttacking()
    {
        weapon.SetActive(false);
    }
}

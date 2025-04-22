using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Gun_Aiming : MonoBehaviour
{
    public Transform player;
    [SerializeField] float gunDistance = 2f;

    void Update()
    {
        gunPosition();
    }

    void gunPosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 playerPos = player.position + new Vector3(-0.01f, 0.81f, 0);
        Vector2 direction = (mousePos - playerPos).normalized;

        // set gun a fixed radius away
        Vector2 gunPosition = playerPos + direction * gunDistance;

        transform.position = gunPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ensure gun faces correct direction and sprite is flipped correctly
        if (player.localScale.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
            vertFlip(mousePos,angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            vertFlip(mousePos,angle);
        }
    }

    void vertFlip(Vector2 mousePos, float angle)
    {
        if(player.localScale.x > 0)
        {
            if(angle<90 && angle>0 || angle<0 && angle >-90)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z)); // Normal scale
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), -1*Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z)); // Flip vertically
            }
        }
        else if(player.localScale.x < 0)
        {   
            //transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
            if(angle<-90 && angle>-180 || angle<180 && angle >90)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z)); // Normal scale
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), -1*Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z)); // Flip vertically
            }
        }
    }
}


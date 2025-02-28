using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AimingCopy : MonoBehaviour
{
    public Transform player;
    [SerializeField] float gunDistance = 2f;
    private bool frozen = false;
    public void Freeze()
    {
        frozen = true;
    }

    public void UnFreeze()
    {
        frozen = !frozen;
    }

    void Update()
    {
        if(!frozen) gunPosition();
    }

    void gunPosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 playerPos = player.position + new Vector3(0, 0.5f, 0);
        Vector2 direction = (mousePos - playerPos).normalized;

        // set gun a fixed radius away
        Vector2 gunPosition = playerPos + direction * gunDistance;

        transform.position = gunPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ensure gun faces correct direction and sprite is flipped correctly
        if (player.localScale.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
            vertFlip(-mousePos, -playerPos);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            vertFlip(mousePos, playerPos);
        }
    }
    void vertFlip(Vector2 mousePos, Vector2 playerPos)
    {
        if (mousePos.x < playerPos.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), -1 * Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z)); // Flip vertically
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z)); // Normal scale
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDamage : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(3);
        }
    }
}

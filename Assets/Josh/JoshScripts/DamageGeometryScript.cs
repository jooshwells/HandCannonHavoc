using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGeometryScript : MonoBehaviour
{
    private bool playerTakingDamage = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Change to OnCollisionStay to ensure player takes DOT? 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTakingDamage = true;
            StartCoroutine(DamageOverTime(collision));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTakingDamage = false;
        }
    }

    private IEnumerator DamageOverTime(Collision2D collision)
    {
        while (playerTakingDamage)
        {
            collision.gameObject.GetComponent<PlayerHealthScript>().Hit(10);
            yield return new WaitForSeconds(1f);
        }
    }
}

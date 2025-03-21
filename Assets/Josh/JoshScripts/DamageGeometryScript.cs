using System.Collections;
using UnityEngine;

public class DamageGeometryScript : MonoBehaviour
{
    private int playerCollisionCount = 0; // Tracks player presence

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollisionCount++;

            // Start DOT only if it's the first collision with the player
            if (playerCollisionCount == 1)
            {
                StartCoroutine(DamageOverTime(collision.gameObject));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollisionCount--;

            // Ensure counter does not go negative
            playerCollisionCount = Mathf.Max(0, playerCollisionCount);
        }
    }

    private IEnumerator DamageOverTime(GameObject player)
    {
        while (playerCollisionCount > 0)
        {
            player.GetComponent<PlayerHealthScript>().Hit(10);
            yield return new WaitForSeconds(1f);
        }
    }
}

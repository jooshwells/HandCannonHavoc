using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BruteAlienSwingRange : MonoBehaviour
{
    public BruteAlien bruteAlien;

    // delete later
    public bool reached = false;
    public bool attack;
    public int index;
    public int counter = 0;
    //

    public Rigidbody2D playerRb;

    public float knockback;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attack = bruteAlien.attacking;
        index = bruteAlien.i;
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        counter++;
        if (bruteAlien.attacking && bruteAlien.i == 3)
        {
            reached = true;

            if (collider.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<PlayerHealthScript>().Hit(25);

                //knockback
                if (bruteAlien.facingRight)
                {
                    knockback = Mathf.Abs(knockback);
                }
                else
                {
                    knockback = -(Mathf.Abs(knockback));
                }

                //playerRb.velocity = new Vector2(knockback, Mathf.Abs(knockback));
                playerRb.AddForce(new Vector2(knockback, knockback), ForceMode2D.Impulse);
            }
        }
    }

}

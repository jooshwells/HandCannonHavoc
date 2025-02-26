using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject instantiator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInstantiator(GameObject inst)
    {
        instantiator = inst;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //handle reducing enemy hp
        }

        else if (!(collision.CompareTag(instantiator.tag))) {
            {
                Destroy(gameObject);
            }
        }
    }
}

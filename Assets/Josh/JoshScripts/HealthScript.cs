using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // health value, initialized to 100
    private float health = 100f;

    public void UpdateHealth(float damage)
    {
        // just in case someone passes in a negative value for damage
        Debug.Log("Attempting to do " + damage + " damage to " + gameObject.name);
        health -= damage > 0 ? damage : 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            Debug.Log(((transform.parent).gameObject).tag + " has died.");
            health = 100f;
        }
    }
}

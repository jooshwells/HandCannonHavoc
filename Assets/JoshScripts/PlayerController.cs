using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private int jump_speed = 10  ;
    // Start is called before the first frame update
    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jump_speed / 2);
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            body.velocity = new Vector2(-5, body.velocity.y);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            body.velocity = new Vector2(5, body.velocity.y);
        }

    }
}

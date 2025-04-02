using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextGate : MonoBehaviour
{
    [SerializeField] private GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(text.activeSelf)
            {
                text.SetActive(false);
            } else
            {
                text.SetActive(true);
            }
        }
    }
}

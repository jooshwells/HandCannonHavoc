using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextGate : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private bool turnOn = true;
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
            text.SetActive(turnOn); // disable or activate text based on serialized field
        }
    }
}

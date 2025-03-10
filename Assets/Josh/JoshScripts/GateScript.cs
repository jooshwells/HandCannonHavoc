using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [SerializeField] private GameObject finishGate;
    [SerializeField] private GameObject startGate;
    [SerializeField] private GameObject stopGate;
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
        if (collision.CompareTag("Player"))
        {
            if (!stopGate.activeSelf)
            {
                stopGate.SetActive(true);
            } else if (stopGate.activeSelf && gameObject.name.Equals("FinishGate"))
            {
                stopGate.SetActive(false);
            }
        }
    }
}

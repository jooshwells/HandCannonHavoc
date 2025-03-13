using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [SerializeField] private GameObject finishGate;
    [SerializeField] private GameObject startGate;
    [SerializeField] private GameObject stopGate;
    [SerializeField] private FlagScript fs;
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
                fs.SetPassedGate(false);
            } else if (stopGate.activeSelf && gameObject.name.Equals("FinishGate"))
            {
                stopGate.SetActive(false);
                fs.SetPassedGate(true);
            }
        }
    }
}

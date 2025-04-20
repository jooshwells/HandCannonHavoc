using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudActivateThing : MonoBehaviour
{
    [SerializeField] GameObject hud;

    // Start is called before the first frame update
    void Start()
    {
        if (hud.active == false)
        {
            hud.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

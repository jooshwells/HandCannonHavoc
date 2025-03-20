using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBagScript : MonoBehaviour
{

    [SerializeField] private float wakeUpDist = 5f;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

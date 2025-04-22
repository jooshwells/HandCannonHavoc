using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraPaperMinion : MonoBehaviour
{
    Animator animator;
    EnemyHealthScript enemyHealthScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        enemyHealthScript = GetComponent<EnemyHealthScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealthScript.GetCurrentHP() <= 0)
        {
            animator.enabled = true;
        }
    }
}

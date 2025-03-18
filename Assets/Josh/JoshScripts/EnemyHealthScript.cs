using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthScript : MonoBehaviour
{
    // health value, initialized to 100
    [SerializeField] private float health = 100f;

    public void UpdateHealth(float damage)
    {
        // just in case someone passes in a negative value for damage
        Debug.Log("Attempting to do " + damage + " damage to " + gameObject.name);
        GetComponent<PlayerHitEffect>().TakeDamage();
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
            StartCoroutine(DyingAnimation());
        }
    }

    IEnumerator DyingAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if(animator == null) animator = GetComponentInChildren<Animator>();

        GetComponent<Rigidbody2D>().simulated = false;

        animator.Play("Die State");
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        Destroy(gameObject);
    }
}

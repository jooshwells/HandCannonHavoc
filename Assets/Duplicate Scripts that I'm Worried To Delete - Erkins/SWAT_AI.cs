using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SWAT_AI : MonoBehaviour
{
    private int movementSpeed;
    private bool playerInSight;
    private bool isReloading;
    private int bulletCount;
    private bool inCombat;
    private Vector3 originalScale;

    public Animator animator;
    public AIPath aiPath;
    public Transform player;

    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private SWAT_Health swatHealth;

    void Start()
    {
        animator.SetInteger("movementSpeed", 1);
        playerInSight = false;
        isReloading = false;
        bulletCount = 16;
        inCombat = false;
        originalScale = transform.localScale;

        // Get the SWAT_Health component
        swatHealth = GetComponent<SWAT_Health>();
    }

    void Update()
    {
        // If the SWAT is dead, skip any actions
        if (swatHealth == null || swatHealth.isDying)
        {
            animator.SetBool("isDead", true);  // Trigger death animation in Animator
            return;
        }

        // Set health parameter to animator
        animator.SetInteger("health", (int)swatHealth.GetHealth());

        spriteFlip();    

        float playerDistance = Vector3.Distance(transform.position, player.position);

        animator.SetInteger("movementSpeed", (int) aiPath.desiredVelocity.magnitude);
        animator.SetInteger("playerDistance", (int) playerDistance);

        if (playerDistance < 10f)
        {
            playerInSight = true;
            animator.SetBool("playerInSight", true);

            if (playerDistance < 7f) // Shoot only when close enough
            {
                inCombat = true;
                animator.SetBool("inCombat", true);
                Shoot();
            }
        }
        else
        {
            playerInSight = false;
            animator.SetBool("playerInSight", false);
        }

        if (bulletCount <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void spriteFlip()
    {
        if (aiPath.desiredVelocity.x < -0.01f) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }   
        else if (aiPath.desiredVelocity.x > 0.01f) 
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(2f); // Wait for reload to complete

        bulletCount = 16;
        animator.SetBool("isReloading", false);
        isReloading = false;
    }

    // Method to take damage when the AI is hit
    public void TakeDamage(int damage)
    {
        // Call the UpdateHealth method on the SWAT_Health script to handle damage
        if (swatHealth != null)
        {
            swatHealth.UpdateHealth(damage);
            animator.SetBool("wasShot", true); // Trigger hit animation if necessary
        }
    }

    public void Shoot()
    {
        if (bulletCount > 0 && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            bulletCount--;
            animator.SetTrigger("shoot");
            Debug.Log("Enemy shot! Remaining bullets: " + bulletCount);
        }
    }
}

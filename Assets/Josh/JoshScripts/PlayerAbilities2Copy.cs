using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAbilities2Copy : MonoBehaviour
{
    public GameObject crosshairsPrefab;
    public GameObject deflatedGlovePrefab; // The "bullet" version of the glove
    public GameObject finalGlovePrefab;     // The "finished" bounce pad
    [SerializeField] private float launchForce = 30f;
    [SerializeField] private float cooldownTime = 2f;
    private float cdTimer = 0f;
    //[SerializeField] private Transform handTransform; // Assign in the Unity Inspector
    private GameObject crosshairsInstance;
    private bool crosshairActive = false;
    private GameObject currentGlove; // Track the active glove so only one exists

    private bool keyPressed = false;
    private List<GameObject> gloves = new List<GameObject>();

    void Update()
    {
        
        if (cdTimer > 0f)
        {
            cdTimer -= Time.deltaTime;
        }
        if (!keyPressed && Input.GetKeyDown(KeyCode.LeftShift) && cdTimer <=0f)
        {
            gameObject.GetComponent<AimingCopy>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            keyPressed = true;
        }
        
        // Launch glove on left shift
        if (keyPressed && Input.GetKeyUp(KeyCode.LeftShift))
        {
            LaunchGlove();
            gameObject.GetComponent<AimingCopy>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            keyPressed =false;

            cdTimer = cooldownTime;
        }
        
    }

    //void ToggleCrosshair()
    //{
    //    if (crosshairActive)
    //    {
    //        if (crosshairsInstance != null)
    //            Destroy(crosshairsInstance);
    //    }
    //    else
    //    {
    //        crosshairsInstance = Instantiate(crosshairsPrefab);
    //    }
    //    crosshairActive = !crosshairActive;
    //}
    
    //void MoveCrosshair()
    //{
    //    if (crosshairsInstance != null)
    //    {
    //        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        crosshairsInstance.transform.position = mousePos;
    //    }
    //}

    void LaunchGlove()
    {
        // Ensure only one glove is active
        //if (gloves.Count > 3)
        //{
        //    Debug.Log("gloves.Count = " + gloves.Count);    
        //    Destroy(gloves.First());
        //    gloves.RemoveAt(0);
        //}

        //if (crosshairsInstance == null) return;
        Vector2 targetPosition = transform.parent.position;

        // Instantiate the deflated glove at player's position, using its preset rotation
        currentGlove = Instantiate(deflatedGlovePrefab, transform.position, deflatedGlovePrefab.transform.rotation);
        gloves.Add(currentGlove);
        // Set the glove's velocity
        Rigidbody2D rb = currentGlove.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = ((Vector2)transform.position - targetPosition).normalized;
            rb.velocity = direction * launchForce;
        } 

        // Tell the glove where to go and what final prefab to spawn
        GloveController gc = currentGlove.GetComponent<GloveController>();
        if (gc != null)
        {
            gc.SetTarget(targetPosition, finalGlovePrefab);
        }

        //// Call the crosshair's shrink function if available
        //CrosshairController cc = crosshairsInstance.GetComponent<CrosshairController>();
        //if (cc != null)
        //{
        //    cc.ShrinkCrosshair();
        //}

        // Optionally, destroy the crosshair after launching so you only use it once
        //Destroy(crosshairsInstance);
        //crosshairActive = false;
    }
}

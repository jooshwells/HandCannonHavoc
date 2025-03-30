using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpPlayerLevelStart : MonoBehaviour
{
    private bool scaleUp = false;
    private bool scaleDown = false;
    private bool playerSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        scaleUp = true;
    }

    public void SetScale(bool up)
    {
        if (up) { scaleUp = true; scaleDown = false; }
        if (!up) { scaleUp = false; scaleDown = true; }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSpawned) return;
        if (scaleUp)
        {
            if (transform.localScale.x <= 1)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 1, 1), 1.25f * Time.deltaTime);
            }
            else
            {
                scaleUp = false;
            }
        }

        if(scaleDown)
        {
            if (transform.localScale.x > 0.1f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0.1f, 0.1f, 1), 1.25f * Time.deltaTime);
            }
            else
            {
                Debug.Log("Running");
                scaleDown = false;

                // Tell Intro Controller Where to spawn player
                GameObject.FindGameObjectWithTag("IntroController").GetComponent<IntroController>().SpawnPlayer();
                playerSpawned = true;
            }
        }
    }
}

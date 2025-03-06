using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControllerScript : MonoBehaviour
{
    // Is the ability available?
    [SerializeField] private int grapplingHookAvail = 1;
    [SerializeField] private int dashAvail = 1;
    [SerializeField] private int bouncePadAvail = 1;
    [SerializeField] private int highJumpParaAvail = 1;

    // references to abilities
    private PlayerControllerMk2 dash; // Not really just dash, but that's where it's handled so we need it
    private GameObject grapple;
    private GameObject highJumpParachute;
    private GameObject bouncePad;

    // Is the ability active? (Different from available, as all can be available but only one may be active)
    private bool grapOn = false;
    private bool highJumpParaOn = false;
    private bool bouncePadOn = false;
    private bool dashOn = false;

    // Start is called before the first frame update
    void Start()
    {
        //Get all of our ability stuff
        GameObject parent = transform.parent.gameObject;
        dash = parent.GetComponent<PlayerControllerMk2>();
        grapple = parent.transform.Find("AltGrapple").gameObject;
        highJumpParachute = parent.transform.Find("HighJumpPara").gameObject;
        bouncePad = parent.transform.Find("BouncePad").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Turn Everything But Dash Off
            TurnEverythingOffBut(1);

            // Dash Active
            dash.SetDashOn(true);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Turn Everything But Grapple Off
            TurnEverythingOffBut(2);

            // Grapple Active
            grapple.SetActive(true);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Turn Everything But High Jump Off
            TurnEverythingOffBut(3);

            // High Jump Active
            highJumpParachute.SetActive(true);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Turn Everything But Bounce Pad Off
            TurnEverythingOffBut(4);

            // Bounce Pad Active
            bouncePad.SetActive(true);
        }

    }

    private void TurnEverythingOffBut(int except)
    {
        switch (except)
        {
            case 1:
                Debug.Log("Dash On");
                grapple.SetActive(false);
                highJumpParachute.SetActive(false);
                bouncePad.SetActive(false);
                break;
            case 2:
                Debug.Log("Grapple On");
                dash.SetDashOn(false);
                highJumpParachute.SetActive(false);
                bouncePad.SetActive(false);
                break;
            case 3:
                Debug.Log("High Jump On");
                grapple.SetActive(false);
                dash.SetDashOn(false);
                bouncePad.SetActive(false);
                break;
            case 4:
                Debug.Log("Bounce Pad On");
                dash.SetDashOn(false);
                highJumpParachute.SetActive(false);
                grapple.SetActive(false);
                break;
        }
    }

    public void PickUp(string tag)
    {
        if(tag.Equals("Grapple"))
        {
            grapplingHookAvail = 1;
        }
    }
}

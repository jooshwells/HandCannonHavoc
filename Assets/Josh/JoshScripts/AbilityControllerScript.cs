using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityControllerScript : MonoBehaviour
{
    // UI Stuff
    [SerializeField] private GameObject abilityIndicator1;
    [SerializeField] private GameObject abilityIndicator2;
    [SerializeField] private GameObject abilityIndicator3;
    [SerializeField] private GameObject abilityIndicator4;

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

    private bool abilityLock = false;
    private bool first = false;
    private int lastAbil = 0;


    // Start is called before the first frame update
    void Start()
    {
        //Get all of our ability stuff
        GameObject parent = transform.parent.gameObject;
        dash = parent.GetComponent<PlayerControllerMk2>();
        grapple = parent.transform.Find("AltGrapple").gameObject;
        highJumpParachute = parent.transform.Find("HighJumpPara").gameObject;
        bouncePad = parent.transform.Find("BouncePad").gameObject;

        TurnEverythingOffBut(0);
    }

    private void DashOn()
    {
        // Turn Everything But Dash Off
        TurnEverythingOffBut(1);

        // Dash Active
        dash.SetDashOn(true);
        abilityIndicator1.SetActive(true);
    }

    private void GrappleOn()
    {
        // Turn Everything But Grapple Off
        TurnEverythingOffBut(2);

        // Grapple Active
        grapple.SetActive(true);
        abilityIndicator2.SetActive(true);
    }

    private void HighJumpParaOn()
    {
        // Turn Everything But High Jump Off
        TurnEverythingOffBut(3);

        // High Jump Active
        highJumpParachute.SetActive(true);
        abilityIndicator3.SetActive(true);
    }

    private void BouncePadOn()
    {
        // Turn Everything But Bounce Pad Off
        TurnEverythingOffBut(4);

        // Bounce Pad Active
        bouncePad.SetActive(true);
        abilityIndicator4.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {  
        if (Input.GetKeyDown(KeyCode.Alpha1) && dashAvail==1)
        {
            DashOn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && grapplingHookAvail==1)
        {
            GrappleOn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && highJumpParaAvail==1)
        {
            HighJumpParaOn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && bouncePadAvail==1)
        {
            BouncePadOn();
        }
    }

    public void TurnEverythingOffBut(int except)
    {
        switch (except)
        {
            case 0:
                dash.SetDashOn(false);
                abilityIndicator1.SetActive(false);

                grapple.SetActive(false);
                abilityIndicator2.SetActive(false);

                highJumpParachute.SetActive(false);
                abilityIndicator3.SetActive(false);

                bouncePad.SetActive(false);
                abilityIndicator4.SetActive(false);

                break;
            case 1: // dash
                //Debug.Log("LastAbil = " + lastAbil);
                if (lastAbil == 0) lastAbil = 1;
                if (!first) first = true;

                if (lastAbil == 2) grapple.GetComponent<StiffGrapple>().Reset();

                grapple.SetActive(false);
                abilityIndicator2.SetActive(false);

                highJumpParachute.SetActive(false);
                abilityIndicator3.SetActive(false);

                bouncePad.SetActive(false);
                abilityIndicator4.SetActive(false);
                lastAbil = 1;

                break;
            case 2: // grapple
                //Debug.Log("LastAbil = " + lastAbil);
                if (lastAbil == 0) lastAbil = 2;

                //if (first)
                //{
                //    Debug.Log("trying reset");
                //    grapple.GetComponent<StiffGrapple>().Reset();
                //    //first = true;
                //} else
                //{
                //    first = true;
                //    grapple.GetComponent<StiffGrapple>().Reset();

                //}
                //grapple.GetComponent<AimingCopy>().Freeze();

                dash.SetDashOn(false);
                abilityIndicator1.SetActive(false);

                highJumpParachute.SetActive(false);
                abilityIndicator3.SetActive(false);

                bouncePad.SetActive(false);
                abilityIndicator4.SetActive(false);
                lastAbil = 2;


                break;
            case 3: // high jump
                //Debug.Log("LastAbil = " + lastAbil);
                if (lastAbil == 0) lastAbil = 3;
                if (!first) first = true;

                if (lastAbil == 2) grapple.GetComponent<StiffGrapple>().Reset();


                grapple.SetActive(false);
                abilityIndicator2.SetActive(false);

                dash.SetDashOn(false);
                abilityIndicator1.SetActive(false);

                bouncePad.SetActive(false);
                abilityIndicator4.SetActive(false);
                lastAbil = 3;
                break;
            case 4: // bounce pad
                //Debug.Log("LastAbil = " + lastAbil);
                if (lastAbil == 0) lastAbil = 4;
                if (!first) first = true;

                if(lastAbil == 2)grapple.GetComponent<StiffGrapple>().Reset();


                dash.SetDashOn(false);
                abilityIndicator1.SetActive(false);

                highJumpParachute.SetActive(false);
                abilityIndicator3.SetActive(false);

                grapple.SetActive(false);
                abilityIndicator2.SetActive(false);
                lastAbil = 4;
                break;
        }
    }

    public void PickUp(string tag)
    {
        if(tag.Equals("Grapple"))
        {
            grapplingHookAvail = 1;
        }
        if(tag.Equals("NewWeapon"))
        {
            transform.parent.GetComponent<WeaponSwitcher>().UnlockWeapon();
        }

    }

    public void LockAllBut(int keep)
    {
        //Debug.Log("Locking");
        abilityLock = !abilityLock;
        switch (keep)
        {
            case 0:
                dashAvail = 1;
                grapplingHookAvail = 1;
                highJumpParaAvail = 1;
                bouncePadAvail = 1;
                break;
            case 1:
                grapplingHookAvail = 0;
                highJumpParaAvail = 0;
                bouncePadAvail = 0;

                DashOn();

                break;
            case 2:
                dashAvail = 0;
                highJumpParaAvail = 0;
                bouncePadAvail = 0;

                GrappleOn();

                break;
            case 3:
                dashAvail = 0;
                grapplingHookAvail = 0;
                bouncePadAvail = 0;

                HighJumpParaOn();

                break;
            case 4:
                dashAvail = 0;
                grapplingHookAvail = 0;
                highJumpParaAvail = 0;

                BouncePadOn();

                break;
        }
    }

    public bool IsLocked() => abilityLock;
}

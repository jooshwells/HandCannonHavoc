using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class genHoldButton : MonoBehaviour
{
    public Image bar;
    float holdNeeded;
    public float progress = 0f;
    float startTime;
    float currTime;
    float endTime;
    bool holding = false;
    bool startedHolding = false;
    bool charged = false;
    bool execute = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = progress;
        if (holding) //button is HELD DOWN
        {
            if (!startedHolding) //executes if user JUST starts holding the button
            {
                startedHolding = true;
                //display charge-up bar
                startTime = Time.time;
                endTime = Time.time + holdNeeded;
            }
            else
            {
                currTime = Time.time - startTime;
                progress = currTime / holdNeeded; //progress is a % (0.00 to 1.00) of time held so far, out of hold needed
                                                                //visible charge-up-bar will use this value
                if (progress >= 1f)
                {
                    charged = true;
                }
            }
        }
        else //button is LET GO
        {
            startedHolding = false;
            if (charged)
            {
                execute = true;
                progress = 0f;
                charged = false;
            }
            else
            {
                execute = false;
                progress = 0f;
            }
        }
    }

    public void setHoldNeeded(float x_secs)
    {
        holdNeeded = x_secs;
    }
    public void isHolding(bool tf)
    {
        holding = tf;
    }
    public bool chargeExecute()
    {
        return execute;
    }
}

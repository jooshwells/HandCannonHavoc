using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagScript : MonoBehaviour
{
    private SpeedrunTimer tc;
    private bool passedGate = false;
    // Start is called before the first frame update
    void Start()
    {
        tc = GameObject.Find("TimerController").GetComponent<SpeedrunTimer>();
    }

    public void SetPassedGate(bool p)
    {
        passedGate = p;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!tc.IsRunning())
            {
                tc.ResetTimer();
                tc.StartTimer();
            }
            else
            {
                if (passedGate)
                {
                    tc.StopTimer();
                    //float currentBestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue); // Use float.MaxValue to ensure first run works
                    //if (tc.GetTime() < currentBestTime)
                    //{
                    //    PlayerPrefs.SetFloat("BestTime", tc.GetTime());
                    //    PlayerPrefs.Save(); // Ensure it gets stored permanently
                    //    GameObject.Find("TimerController").GetComponent<SpeedrunTimer>().UpdateBestTimerDisplay(PlayerPrefs.GetFloat("BestTime", 0f));
                    //}
                }
            }
        }
    }
}

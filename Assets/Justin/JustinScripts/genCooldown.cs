using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class genCooldown : MonoBehaviour
{
    private float readyTime;
    private float duration;

    private float startTime;
    public float timeRange;
    public float progress;
    [SerializeField] public Image bar;
    void Start()
    {

    }

    void Update()
    {
        //cooldown bar ~ visual only
        if (isActive())
        {
            bar.gameObject.SetActive(true);
            timeRange = readyTime - startTime;
            progress = 1 - ((Time.time - startTime) / timeRange);
            bar.fillAmount = progress;
        }
        else
        {
            bar.gameObject.SetActive(false);
        }
    }

    public void setCooldown(float cooldown)
    {
        duration = cooldown;
    }
    public void enable()
    {
        startTime = Time.time;
        readyTime = Time.time + duration;
    }

    public bool isActive()
    {
        if (Time.time < readyTime)
            return true;
        return false;
    }
}

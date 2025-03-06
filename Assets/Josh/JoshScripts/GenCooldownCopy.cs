using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenCooldownCopy : MonoBehaviour
{
    private float readyTime;
    private float duration;

    void Start()
    {

    }

    void Update()
    {

    }

    public void setCooldown(float cooldown)
    {
        duration = cooldown;
    }
    public void enable()
    {
        readyTime = Time.time + duration;
    }

    public bool isActive()
    {
        if (Time.time < readyTime)
            return true;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Railgun : customShoot
{
    public Image bar;

    private void Awake()
    {
        bulletSpeed = 10000f;
        //bulletDuration default
        fireRate = 0.5f;
        //attackDamage = 30f;
        magSize = 50;
        reloadSpeed = 4f;
    }
    
    protected override bool shootingInput() //shoot based on holding rather than press
    {
        genHoldButton hold = gameObject.GetComponent<genHoldButton>();
        hold.setHoldNeeded(0.5f); //2 seconds to hold to fire
        hold.isHolding(Input.GetMouseButton(0) && Time.time >= nextBullet); // left click
        return hold.chargeExecute();
    }
}

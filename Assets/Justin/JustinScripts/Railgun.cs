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
        bulletSpeed = 500f;
        //bulletDuration default
        fireRate = 1f;
        //attackDamage = 30f;
        magSize = 4;
        reloadSpeed = 4f;
    }

    protected override void shoot()
    {
        base.shoot();

    }

    protected override bool shootingInput() //shoot based on holding rather than press
    {
        genHoldButton hold = gameObject.GetComponent<genHoldButton>();
        hold.setHoldNeeded(1f); // # seconds to hold to fire
        hold.isHolding(Input.GetMouseButton(0) && Time.time >= nextBullet); // left click
        return hold.chargeExecute();
    }
}

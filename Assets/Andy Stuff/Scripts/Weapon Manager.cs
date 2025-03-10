using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponList;  
    private int curIdx = 0;  // tracks current weapon in list
    private int idx = 0; // increments index, use mod weaponList.Count

    private void Start()
    {
        // activate first weapon
        UpdateWeaponVisibility();
    }

    void Update()
    {
        // cycle weapons with q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            idx++;
            SwitchWeapon();
            
        }
    }

    void SwitchWeapon()
    {
        // disable current weapon
        DeactivateWeapon(curIdx);

        // keep idx in bounds
        curIdx = idx % weaponList.Count;

        // activate next weapon
        UpdateWeaponVisibility();
    }

    void DeactivateWeapon(int index)
    {
        // get weapon in list and disable it
        GameObject weapon = weaponList[index];
        weapon.SetActive(false);
    }

    void UpdateWeaponVisibility()
    {
        // active weapon from list
        GameObject weapon = weaponList[curIdx];
        weapon.SetActive(true);
    }
}
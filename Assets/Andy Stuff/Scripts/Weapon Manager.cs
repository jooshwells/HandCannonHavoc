using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponList;  // List to hold weapons (pistol, shotgun, etc.)
    private int curIdx = 0;  // Index to track the current active weapon
    private int idx = 0; // increments index, use mod weaponList.Count

    private void Start()
    {
        // Initialize by setting the first weapon as active
        UpdateWeaponVisibility();
    }

    void Update()
    {
        // Switch weapons with Q key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            idx++;
            SwitchWeapon();
            
        }
    }

    void SwitchWeapon()
    {
        // Disable current weapon's components
        DeactivateWeapon(curIdx);

        // Update the index and ensure it's within bounds
        curIdx = idx % weaponList.Count;

        // Activate the new weapon's components
        UpdateWeaponVisibility();
    }

    void DeactivateWeapon(int index)
    {
        // Get the weapon object at the specified index and disable all necessary components
        GameObject weapon = weaponList[index];
        weapon.SetActive(false);
    }

    void UpdateWeaponVisibility()
    {
        // Activate the current weapon's components
        GameObject weapon = weaponList[curIdx];
        weapon.SetActive(true);
    }
}
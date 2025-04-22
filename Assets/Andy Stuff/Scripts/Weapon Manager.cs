using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponList;
    [Header("Game Object with Indicators as Children")]
    [SerializeField] private GameObject weaponIndicatorList;
    private int curIdx = 0;  // tracks current weapon in list
    private int idx = 0; // increments index, use mod weaponList.Count
    private int weaponsAvailable=0;
    private int totalWeaponsAvailable = 7;
    private bool firstWeaponDisplayed = false;

    public void UnlockWeapon()
    {
        if(PlayerPrefs.GetInt("WeaponsUnlocked", 0) < totalWeaponsAvailable)
        {
            weaponsAvailable += 1;
            PlayerPrefs.SetInt("WeaponsUnlocked", weaponsAvailable);
        } else
        {
            return;
        }
        
        if(weaponsAvailable >= 1 && !firstWeaponDisplayed)
        {
            UpdateWeaponVisibility();
            firstWeaponDisplayed = true;
        }
    }
    
    private void Start()
    {
        // activate first weapon if unlocked
        weaponsAvailable = PlayerPrefs.GetInt("WeaponsUnlocked", 0);

        if (weaponsAvailable >= 1 && !firstWeaponDisplayed)
        {
            UpdateWeaponVisibility();
            firstWeaponDisplayed = true;
        }
    }

    void Update()
    {
        // cycle weapons with q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (idx - 1 >= 0)
            {
                idx--;
            } else
            {
                idx = weaponList.Count - 1;
            }
            SwitchWeapon();
            
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            idx++;
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        if (weaponsAvailable == 0) return;

        // disable current weapon
        DeactivateWeapon(curIdx);

        // keep idx in bounds
        curIdx = idx % weaponsAvailable;

        // activate next weapon
        UpdateWeaponVisibility();
    }

    void DeactivateWeapon(int index)
    {
        // get weapon in list and disable it
        GameObject weapon = weaponList[index];
        GameObject weaponInd = weaponIndicatorList.transform.GetChild(curIdx).gameObject;
        weapon.SetActive(false);
        weaponInd.SetActive(false);

    }

    void UpdateWeaponVisibility()
    {
        // active weapon from list
        GameObject weapon = weaponList[curIdx];
        GameObject weaponInd = weaponIndicatorList.transform.GetChild(curIdx).gameObject;
        weaponInd.SetActive(true);
        weapon.SetActive(true);
    }
}
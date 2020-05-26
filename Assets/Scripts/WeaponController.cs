using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private Weapon[] weapons_;

    [SerializeField]
    private Transform weaponPosition_;

    private int currentWeaponIndex_;

    private void Awake()
    {
        InitiateWeapons();
        currentWeaponIndex_ = 0;
    }

    private void InitiateWeapons()
    {
        for ( int i = 0; i < weapons_.Length; i++ )
        {
            //swap prefabs with cloned objects
            weapons_[i] = Instantiate( weapons_[i] );
            weapons_[i].transform.SetParent( weaponPosition_ );
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        EquipCurrentWeapon();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            weapons_[currentWeaponIndex_].Fire();
        }

        if ( Input.GetKeyDown( KeyCode.W ) )
        {
            EquipNextWeapon();
        }

        if ( Input.GetKeyDown( KeyCode.Q ) )
        {
            EquipPreviousWeapon();
        }
    }

    private void EquipCurrentWeapon()
    {
        weapons_[currentWeaponIndex_].Equip( weaponPosition_ );
    }

    private void UnequipCurrentWeapon()
    {
        weapons_[currentWeaponIndex_].Unequip();
    }

    private void EquipNextWeapon()
    {
        UnequipCurrentWeapon();
        currentWeaponIndex_ = ( currentWeaponIndex_ + 1 ) % weapons_.Length;
        EquipCurrentWeapon();
    }

    private void EquipPreviousWeapon()
    {
        UnequipCurrentWeapon();
        currentWeaponIndex_ = (currentWeaponIndex_ + weapons_.Length - 1) % weapons_.Length;
        EquipCurrentWeapon();
    }
}

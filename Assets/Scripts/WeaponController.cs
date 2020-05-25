using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private Weapon[] weapons_;

    [SerializeField]
    private Transform weaponPosition_;

    private int currentWeapon;

    private void Awake()
    {
        InitiateWeapons();
        currentWeapon = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        weapons_[currentWeapon].Equip( weaponPosition_ );
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            weapons_[currentWeapon].Fire();
        }
    }

    private void InitiateWeapons()
    {
        for( int i = 0; i < weapons_.Length; i++ )
        {
            //swap prefabs with cloned objects
            weapons_[i] = Instantiate( weapons_[i] );
        }
    }
}

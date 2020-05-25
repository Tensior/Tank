using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float fireRate_ = 10f;

    //[SerializeField]
    //private Rigidbody shellPrefab_;

    [SerializeField]
    private Transform shellSpawnPosition_;

    //[SerializeField]
    //private int poolSize_ = 10;

    //private Queue<Rigidbody> shellPool_;

    private bool isEquipped_ = false;
    private float timeSinceLastFire_ = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        Unequip();

        //shellPool_ = new Queue<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( gameObject.activeSelf )
        {
            timeSinceLastFire_ += Time.deltaTime;
        }
    }

    public void Equip( Transform weaponPosition )
    {
        gameObject.SetActive( true );

        transform.SetParent( weaponPosition );
        transform.localPosition = Vector3.zero;

        isEquipped_ = true;
    }

    public void Unequip()
    {
        transform.SetParent( null );
        gameObject.SetActive( false );

        isEquipped_ = false;
    }

    public void Fire()
    {
        if ( timeSinceLastFire_ < 1f / fireRate_) //can't fire faster then fire rate
        {
            return;
        }

        var newShell = CannonShellPool.Instance.GetFromPool().gameObject.GetComponent<Rigidbody>();
        newShell.gameObject.SetActive( true );

        //if ( shellPool_.Count < poolSize_ )
        //{
        //    newShell = Instantiate( shellPrefab_ );
        //}
        //else
        //{
        //    newShell = shellPool_.Dequeue();
        //}
        //shellPool_.Enqueue( newShell );
        newShell.position = shellSpawnPosition_.position;
        newShell.rotation = shellSpawnPosition_.rotation;

        newShell.velocity = transform.localToWorldMatrix * Vector3.up * 10f;
        timeSinceLastFire_ = 0;
    }
}

using UnityEngine;

[RequireComponent( typeof( ShellPool ) )]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float fireRate_ = 10f;

    [SerializeField]
    private Transform shellSpawnPosition_;

    private float timeSinceLastFire_ = 0f;

    private ShellPool shellPool_;

    private void Awake()
    {
        Unequip();
        shellPool_ = GetComponent<ShellPool>();
    }

    private void Update()
    {
        if ( gameObject.activeSelf )
        {
            timeSinceLastFire_ += Time.deltaTime;
        }
    }

    public void Equip()
    {
        gameObject.SetActive( true );

        transform.localPosition = Vector3.zero;
    }

    public void Unequip()
    {
        gameObject.SetActive( false );
    }

    public void Fire()
    {
        if ( timeSinceLastFire_ < 1f / fireRate_) //can't fire faster then fire rate
        {
            return;
        }

        var newShell = shellPool_.GetFromPool();
        newShell.gameObject.SetActive( true );
        var newShellRigidBody = newShell.gameObject.GetComponent<Rigidbody>();
        newShellRigidBody.position = shellSpawnPosition_.position;
        newShellRigidBody.rotation = shellSpawnPosition_.rotation;

        newShellRigidBody.velocity = transform.localToWorldMatrix * Vector3.up * newShell.Speed;
        timeSinceLastFire_ = 0;
    }
}

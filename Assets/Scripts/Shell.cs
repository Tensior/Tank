using UnityEngine;

public class Shell : MonoBehaviour, IGenericPoolableObject<Shell>
{
    [SerializeField]
    private float speed_ = 10f;
    public float Speed { get => speed_; private set => speed_ = value; }

    [SerializeField]
    private float damage_ = 1f;

    public GenericObjectPool<Shell> Pool { get; set; }

    private float maxLifeTime_ = 0f;

    private void Awake()
    {
        maxLifeTime_ = 100f / speed_; //100 units is an approximate size of the scene
    }

    private void OnEnable()
    {
        Invoke( "ReturnShellToPool", maxLifeTime_ );
    }

    private void OnTriggerEnter( Collider other )
    {
        if (other.gameObject.GetComponent<Weapon>())
        {
            return; //just ignore firing events
        }

        // Collision with an enemy
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage( damage_ );
        }

        //Destroy shell on any collision except other shells
        if ( !other.gameObject.GetComponent<Shell>() )
        {
            ReturnShellToPool();
        }
    }

    private void ReturnShellToPool()
    {
        Pool.ReturnToPool( this );
        CancelInvoke();
    }
}

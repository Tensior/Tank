using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent( typeof( NavMeshAgent ) )]
public class Enemy : CombatController, IGenericPoolableObject<Enemy>
{ 
    private Player target_;
    private NavMeshAgent navMeshAgent_;

    [SerializeField]
    private float damage_ = 10f;

    public GenericObjectPool<Enemy> Pool { get; set; }

    private void Awake()
    {
        target_ = FindObjectOfType<Player>();
        navMeshAgent_ = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if ( target_ != null ) // may be null if this enemy was awakened after player death and deactivation
        {
            navMeshAgent_.SetDestination( target_.transform.position );
        }
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject == target_.gameObject )
        {
            target_.TakeDamage( damage_ );
            AllHealthLost();
        }
    }

    protected override void AllHealthLost()
    {
        Pool.ReturnToPool( this );
        base.AllHealthLost();
    }
}

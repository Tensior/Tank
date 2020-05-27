using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{
    public UnityAction OnDeath;

    [SerializeField]
    private float maxHealth_ = 100f;

    [SerializeField, Range(0f, 1f)]
    private float armor_ = 0.5f;

    private float currentHealth_;

    private void OnEnable()
    {
        currentHealth_ = maxHealth_;
    }

    public void TakeDamage( float damage )
    {
        currentHealth_ -= damage * ( 1f - armor_ );
        if ( currentHealth_ <= 0f )
        {
            AllHealthLost();
        }
    }

    protected virtual void AllHealthLost()
    {
        OnDeath?.Invoke();
        OnDeath = null; //unsubscribe all listeners
    }
}

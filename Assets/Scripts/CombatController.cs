using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth_ = 100f;

    [SerializeField, Range(0f, 1f)]
    private float armor_ = 0.5f;

    private float currentHealth_;
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth_ = maxHealth_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

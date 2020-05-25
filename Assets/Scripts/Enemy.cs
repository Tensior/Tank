using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject target_;
    private NavMeshAgent navMeshAgent_;

    void Awake()
    {
        target_ = FindObjectOfType<MovementController>().gameObject; //target object which is controlled by player
        navMeshAgent_ = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        navMeshAgent_.SetDestination( target_.transform.position );
    }
}

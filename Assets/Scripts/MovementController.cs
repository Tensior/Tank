using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float speed_ = 6.0f; // [unit/sec]

    [SerializeField]
    private float rotateSpeed_ = 90.0f;

    [SerializeField]
    private MeshRenderer allowedArea_;

    private Rigidbody rigidBody_;
    private Vector3 moveInput_ = Vector3.zero; //position difference between fixed updates
    private Quaternion rotateInput_ = Quaternion.identity; //rotation difference between fixed updates

    private void Awake()
    {
        rigidBody_ = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        moveInput_ = speed_ * Time.fixedDeltaTime * Input.GetAxis( "Vertical" ) * transform.TransformDirection( Vector3.forward );

        //for proper rotation when moving backwards
        var rotationDir = Input.GetAxis( "Vertical" ) < 0 ? -Input.GetAxis( "Horizontal" ) : Input.GetAxis( "Horizontal" );

        rotateInput_ = Quaternion.AngleAxis( rotateSpeed_ * Time.fixedDeltaTime * rotationDir, Vector3.up );
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if ( moveInput_ != Vector3.zero )
        {
            var newPosition = rigidBody_.position + moveInput_;

            // Constrain movement if allowedArea_ was provided
            if ( !allowedArea_ || allowedArea_.bounds.Contains( new Vector3( newPosition.x, allowedArea_.bounds.center.y, newPosition.z ) ) )
            {
                rigidBody_.MovePosition( newPosition );
            }
            else
            {
                Debug.Log( "constraining..." );
            }
        }
    }

    private void Rotate()
    {
        if ( rotateInput_ != Quaternion.identity )
        {
            rigidBody_.MoveRotation( rigidBody_.rotation * rotateInput_ );
        }
    }

    //private void Update()
    //{
    //    ReadInput();
    //}

    //private void ReadInput()
    //{
    //    moveDirection_ = new Vector3( Input.GetAxis( "Horizontal" ), 0.0f, Input.GetAxis( "Vertical" ) ).normalized;
    //}

    //void FixedUpdate()
    //{
    //    if ( moveDirection_ != Vector3.zero )
    //    {
    //        // Rotate so that move direction becomes local forward
    //        var localForward = transform.TransformDirection( Vector3.forward );
    //        if ( moveDirection_ != localForward )
    //        {
    //            //Rotating
    //            Debug.Log( "rotating..." );
    //            /*transform.rotation*/
    //            rigidBody_.MoveRotation( Quaternion.RotateTowards( rigidBody_.rotation, 
    //                                                               Quaternion.LookRotation( moveDirection_, Vector3.up ), 
    //                                                               rotateSpeed_ * Time.fixedDeltaTime ) );
    //        }

    //        var newPosition = transform.position + moveDirection_ * speed_ * Time.fixedDeltaTime;

    //        // Constrain movement if allowedArea_ was provided
    //        if ( !allowedArea_ || allowedArea_.bounds.Contains( new Vector3( newPosition.x, allowedArea_.bounds.center.y, newPosition.z ) ) )
    //        {
    //            rigidBody_.MovePosition( newPosition );
    //        }
    //        else
    //        {
    //            Debug.Log( "constraining..." );
    //        }
    //    }
    //}
}

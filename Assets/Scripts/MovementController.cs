using UnityEngine;

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
            var areaBounds = allowedArea_.bounds;
            areaBounds.Expand( new Vector3(-1f, 0f, -1f ) ); //shrink allowed area a bit

            // Constrain movement
            if ( areaBounds.Contains( new Vector3( newPosition.x, areaBounds.center.y, newPosition.z ) ) )
            {
                rigidBody_.MovePosition( newPosition );
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
}

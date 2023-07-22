using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Motor")]
    [SerializeField] private Rigidbody motor;

    private float moveInput;
    private float moveSpeed;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float backwardSpeed;

    private float turnInput;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float minVelocityToTurn;

    [SerializeField] private float rotateToGroundNormalSpeed;

    [Header("Rigidbody settings")]
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float extraGravityForce;
    [SerializeField] private float airDrag;
    private float normalDrag;
    private bool _isGrounded;
    public bool isGrounded => _isGrounded;

    private void Start()
    {
        motor.transform.parent = null;
        carRigidbody.transform.parent = null;

        normalDrag = motor.drag;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        Move();
        Rotate();

        _isGrounded = Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1f, groundLayer);

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, fromToRotation, rotateToGroundNormalSpeed * Time.deltaTime);

        motor.drag = _isGrounded ? normalDrag : airDrag;
    }
    
    private void FixedUpdate()
    {
        if (_isGrounded)
        {
            motor.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            motor.AddForce(Vector3.down * extraGravityForce);
        }

        carRigidbody.MoveRotation(transform.rotation);
    }

    private void Move()
    {
        moveSpeed = moveInput > 0 ? moveInput * forwardSpeed : moveInput * backwardSpeed;
        transform.position = motor.transform.position;
    }

    private void Rotate()
    {
        if (motor.velocity.magnitude > minVelocityToTurn)
        {
            float newRotation = turnInput * turnSpeed * Time.deltaTime * Mathf.Sign(moveInput);
            transform.Rotate(0, newRotation, 0, Space.World);
        }
    }
}

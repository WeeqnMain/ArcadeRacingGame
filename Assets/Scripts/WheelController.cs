using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CarController))]
public class WheelController : MonoBehaviour
{
    [SerializeField] private Transform[] wheels;
    [SerializeField] private TrailRenderer[] trails;

    private float moveInput;
    private float turnInput;

    private Animator animator;
    private CarController carController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(moveInput * 1000f * Time.deltaTime, 0, 0);
        }

        if (turnInput > 0)
        {
            animator.SetBool("isGoingLeft", false);
            animator.SetBool("isGoingRight", true);
        }
        else if (turnInput < 0)
        {
            animator.SetBool("isGoingLeft", true);
            animator.SetBool("isGoingRight", false);
        }
        else
        {
            animator.SetBool("isGoingLeft", false);
            animator.SetBool("isGoingRight", false);
        }

        if (carController.isGrounded && moveInput != 0f)
            SetTrailsEmit(true);
        else
            SetTrailsEmit(false);
    }

    private void SetTrailsEmit(bool state)
    {
        foreach (var trail in trails)
        {
            trail.emitting = state;
        }
    }
}

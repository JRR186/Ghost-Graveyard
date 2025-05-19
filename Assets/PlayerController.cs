using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public CinemachineCamera cam;
    public float speed, sensitivity, maxForce, jumpHeight;
    private Vector2 move, look;
    private float lookRotation;

    [SerializeField]private Transform groundCheck;
    [SerializeField]private LayerMask groundMask;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump();
        }
    }

    void Move()
    {
        Vector3 currentVelocity = rb.linearVelocity;


        Vector3 targetDirection = cam.transform.right * move.x + cam.transform.forward * move.y;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        Vector3 targetVelocity = targetDirection * speed;

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);

        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Jump()
    {
        bool grounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
        if (grounded)
        {
            Vector3 jumpForce = Vector3.up * jumpHeight;
            rb.AddForce(jumpForce, ForceMode.VelocityChange);

        }
    }

    

    private void FixedUpdate()
    {
        Move();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public CinemachineCamera cam;
    public float speed, sensitivity, maxForce, jumpHeight, playerHealth;
    private Vector2 move, look;
    private float lookRotation;

    [SerializeField]private Transform groundCheck;
    [SerializeField]private LayerMask groundMask;

    public TextMeshProUGUI healthText;
    public Image blood1, blood2, blood3;
    public GameObject Blood1, Blood2, Blood3;

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        BloodScreen();

        if (playerHealth <= 0)
        {
            StartCoroutine(LoadStart());
        }
    }
    public void BloodScreen()
    {
        healthText.SetText(playerHealth.ToString());
        float transparency = 1f - ((playerHealth / 100f)*2);
        Color color = Color.white;
        color.a = transparency;

        if (playerHealth > 90)
        {
            Blood1.SetActive(true);
            blood1.color = color;
        }
        if (playerHealth > 40)
        {
            Blood2.SetActive(true);
            Blood1.SetActive(false);
            blood2.color = color;
        }
        if (playerHealth > 10)
        {
            Blood3.SetActive(true);
            Blood2.SetActive(false);
            blood3.color = color;
        }
    }
    public IEnumerator LoadStart()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Start");
    }
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

    private void Update()
    {
        BloodScreen();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}

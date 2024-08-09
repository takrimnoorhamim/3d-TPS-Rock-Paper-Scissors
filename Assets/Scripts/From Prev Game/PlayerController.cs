using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 2f;
    public float turnSmoothTime = 0.1f;
    public Transform cameraTransform;
    public float cameraDistance = 5f;
    public Vector3 cameraOffset = new Vector3(0, 2, 0);
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    private Rigidbody rb;
    private float verticalRotation = 0f;
    public Animator animator;
    private bool isGrounded = true;
    private float turnSmoothVelocity;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Update movement state
        UpdateMovementState();

        // Handle camera and player rotation based on mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        // Rotate both player and camera horizontally based on mouse input
        transform.Rotate(Vector3.up * mouseX);
        cameraTransform.rotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, 0);
    }

    void LateUpdate()
    {
        // Update camera position to follow the player
        Vector3 desiredPosition = transform.position + transform.TransformDirection(cameraOffset) - cameraTransform.forward * cameraDistance;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * 5f);
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        if (input.magnitude >= 0.1f)
        {
            // Calculate the movement direction in world space
            moveDirection = transform.right * input.x + transform.forward * input.z;

            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Move the player
            rb.velocity = moveDirection * currentSpeed + new Vector3(0, rb.velocity.y, 0);

            // Only rotate the player when not running
            if (!isRunning && moveDirection != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
        else
        {
            // Stop horizontal movement if no input
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            moveDirection = Vector3.zero;
        }
    }

    void UpdateMovementState()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        bool isMoving = movement.magnitude > 0.1f;
        bool isRunning = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && isMoving;

        if (isRunning)
        {
            animator.ResetTrigger("Walk");
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Run");
        }
        else if (isMoving)
        {
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Idle");
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        animator.SetTrigger("Jump");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Idle");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.ResetTrigger("Jump");
            // The next animation will be set in the next Update frame
        }
    }
}
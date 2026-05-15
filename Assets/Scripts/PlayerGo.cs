using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Camera Settings")]
    public Transform cameraTransform; // Перетащите сюда камеру (или её родительский объект)
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f; // Максимальный угол взгляда вверх/вниз

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    // Для бега
    private bool isRunning;
    private float currentSpeed;

    private Animator animator;
    public GameObject pistol;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Блокируем курсор в центре экрана
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Если камера не назначена, ищем её в дочерних объектах
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        ApplyGravity();

        //анимация прицеливания
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetTrigger("IsPressedRightMouseButton");
            pistol.SetActive(true);
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.ResetTrigger("IsPressedRightMouseButton");
            pistol.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1))
        {
            animator.Play("HumanM@Gun_Aim01_Shoot01");
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Поворот персонажа по горизонтали
        transform.Rotate(Vector3.up * mouseX);

        // Поворот камеры по вертикали
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        // Проверка на бег (зажат Shift)
        isRunning = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Движение относительно поворота персонажа
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * currentSpeed * Time.deltaTime);

        //анимация ходьбы, бега
        if (move != new Vector3(0,0,0))
        {
            if (isRunning)
            {
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
            
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Опционально: для принудительного освобождения курсора (например, в меню)
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
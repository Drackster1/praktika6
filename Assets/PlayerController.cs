using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Скорости для ходьбы, бега и приседания
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;

    // Параметры прыжка и гравитации
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    // Параметры для обзора с помощью мыши
    public float mouseSensitivity = 2f;
    public Transform cameraTransform; // Ссылка на камеру
    private float cameraPitch = 0f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalVelocity = 0f;

    public KeyCode escapeKey = KeyCode.Escape;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Если камера не назначена в Inspector, попытаемся найти Main Camera
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(escapeKey))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Обработка обзора с помощью мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Поворот персонажа по горизонтали
        transform.Rotate(0, mouseX, 0);

        // Поворот камеры по вертикали
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0, 0);

        // Получение ввода для движения
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        Vector3 move = transform.forward * moveZ + transform.right * moveX;

        // Выбор текущей скорости (бег, приседание или ходьба)
        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            currentSpeed = crouchSpeed;
        }

        // Если персонаж стоит на земле
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // Небольшое отрицательное значение, чтобы персонаж прилипал к земле

            // Прыжок по нажатию пробела
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // Применение гравитации
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection = move * currentSpeed;
        moveDirection.y = verticalVelocity;

        // Движение через CharacterController!!!!!!
        controller.Move(moveDirection * Time.deltaTime);

        // Перезапуск уровня, если персонаж падает ниже определённой точки (например, -10 по Y)
        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
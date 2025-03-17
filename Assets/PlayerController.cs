using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // �������� ��� ������, ���� � ����������
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;

    // ��������� ������ � ����������
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    // ��������� ��� ������ � ������� ����
    public float mouseSensitivity = 2f;
    public Transform cameraTransform; // ������ �� ������
    private float cameraPitch = 0f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalVelocity = 0f;

    public KeyCode escapeKey = KeyCode.Escape;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // ���� ������ �� ��������� � Inspector, ���������� ����� Main Camera
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

        // ��������� ������ � ������� ����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ������� ��������� �� �����������
        transform.Rotate(0, mouseX, 0);

        // ������� ������ �� ���������
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0, 0);

        // ��������� ����� ��� ��������
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        Vector3 move = transform.forward * moveZ + transform.right * moveX;

        // ����� ������� �������� (���, ���������� ��� ������)
        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            currentSpeed = crouchSpeed;
        }

        // ���� �������� ����� �� �����
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // ��������� ������������� ��������, ����� �������� �������� � �����

            // ������ �� ������� �������
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // ���������� ����������
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection = move * currentSpeed;
        moveDirection.y = verticalVelocity;

        // �������� ����� CharacterController!!!!!!
        controller.Move(moveDirection * Time.deltaTime);

        // ���������� ������, ���� �������� ������ ���� ����������� ����� (��������, -10 �� Y)
        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
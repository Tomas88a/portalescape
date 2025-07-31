using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float jumpForce = 8f;
    public Vector3 customGravity = new Vector3(0, -9.81f, 0);

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;

    [Header("Hand")]
    public Transform handTransform; // 在Inspector指定你的Hand空物体（建议为Player子物体，放在摄像机前）

    private CharacterController controller;
    private Camera mainCam;
    private Vector3 verticalVelocity = Vector3.zero;
    private float xRotation = 0f;
    private bool isGrounded;
    private bool wasGrounded;

    // 新增变量
    private float jumpTimeout = 0f;
    private const float jumpGroundingPreventTime = 0.15f; // 起跳后多长时间忽略地面检测

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up, mouseX, Space.Self);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (mainCam != null)
            mainCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        Vector3 gravityDir = customGravity.normalized;

        // 地面检测（双保险）
        bool controllerGrounded = controller.isGrounded;
        bool rayGrounded = false;
        float rayDist = controller.height / 2 + 0.3f;
        Vector3 rayStart = transform.position - gravityDir * 0.05f;
        if (Physics.Raycast(rayStart, gravityDir, out RaycastHit hit, rayDist))
            rayGrounded = true;

        bool realGrounded = controllerGrounded || rayGrounded;

        // 跳跃冷却判定
        if (jumpTimeout > 0)
        {
            jumpTimeout -= Time.deltaTime;
            isGrounded = false;
        }
        else
        {
            isGrounded = realGrounded;
        }

        // 跳跃和重力
        if (isGrounded)
        {
            if (!wasGrounded)
                verticalVelocity = Vector3.zero;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = -gravityDir * jumpForce;
                isGrounded = false;
                jumpTimeout = jumpGroundingPreventTime; // 跳跃后N秒内忽略地面
            }

            controller.Move(gravityDir * 0.01f);
        }
        else
        {
            verticalVelocity += customGravity * Time.deltaTime;
        }

        // 水平移动
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * inputX + transform.forward * inputZ);
        move = Vector3.ProjectOnPlane(move, gravityDir).normalized * moveSpeed;

        Vector3 totalMove = move * Time.deltaTime + verticalVelocity * Time.deltaTime;
        controller.Move(totalMove);

        wasGrounded = isGrounded;
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            float interactRange = 2f;
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, interactRange))
            {
                // 优先拾取物品
                var pickable = hit.collider.GetComponent<Pickable>();
                if (pickable != null)
                {
                    pickable.PickUp(handTransform);
                    return;
                }

                // 其它交互
                
                // ...你可以继续扩展更多机关类型
            }
        }
    }
}

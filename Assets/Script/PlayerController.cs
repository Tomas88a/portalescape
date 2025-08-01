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
    public Transform handTransform;

    [Header("初始重力&朝向")]
    public Vector3 initialGravity = new Vector3(0, -9.81f, 0);
    public Vector3 initialEulerAngles = Vector3.zero;

    private CharacterController controller;
    private Camera mainCam;
    private Vector3 verticalVelocity = Vector3.zero;
    private float xRotation = 0f;
    private bool isGrounded;
    private bool wasGrounded;

    private float jumpTimeout = 0f;
    private const float jumpGroundingPreventTime = 0.15f;

    private float cameraRollOffset = 0f;

    private Pickable heldPickable = null;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        customGravity = initialGravity;

        // 只改player水平方向（y轴）欧拉角
        transform.eulerAngles = new Vector3(0, initialEulerAngles.y, 0);
        xRotation = initialEulerAngles.x;
        cameraRollOffset = initialEulerAngles.z;

        if (mainCam != null)
            mainCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, cameraRollOffset);
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
    }

    // ★★★ 关键部分：自适应重力的鼠标Look ★★★
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 水平旋转
        Vector3 upAxis = -customGravity.normalized;
        transform.Rotate(upAxis, mouseX, Space.World);

        // 垂直pitch
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (mainCam != null)
        {
            mainCam.transform.localEulerAngles = new Vector3(xRotation, 0f, cameraRollOffset);
        }
    }


    void HandleMovement()
    {
        Vector3 gravityDir = customGravity.normalized;

        bool controllerGrounded = controller.isGrounded;
        bool rayGrounded = false;
        float rayDist = controller.height / 2 + 0.3f;
        Vector3 rayStart = transform.position - gravityDir * 0.05f;
        if (Physics.Raycast(rayStart, gravityDir, out RaycastHit hit, rayDist))
            rayGrounded = true;

        bool realGrounded = controllerGrounded || rayGrounded;

        if (jumpTimeout > 0)
        {
            jumpTimeout -= Time.deltaTime;
            isGrounded = false;
        }
        else
        {
            isGrounded = realGrounded;
        }

        if (isGrounded)
        {
            if (!wasGrounded)
                verticalVelocity = Vector3.zero;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = -gravityDir * jumpForce;
                isGrounded = false;
                jumpTimeout = jumpGroundingPreventTime;
            }

            controller.Move(gravityDir * 0.01f);
        }
        else
        {
            verticalVelocity += customGravity * Time.deltaTime;
        }

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
        // 鼠标左键拔钉子
        if (Input.GetMouseButtonDown(0))
        {
            float pullDistance = 2f;
            Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, pullDistance))
            {
                var nail = hit.collider.GetComponent<NailPullable>();
                if (nail != null)
                {
                    nail.PullOnce();
                    return;
                }
            }
        }

        // E键拾取/放下物体
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldPickable != null)
            {
                heldPickable.Drop();
                heldPickable = null;
                return;
            }

            float interactRange = 2f;
            float pickRadius = 0.3f;
            Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);

            if (Physics.SphereCast(ray, pickRadius, out RaycastHit hit, interactRange))
            {
                var pickable = hit.collider.GetComponent<Pickable>();
                if (pickable != null && !pickable.IsPicked())
                {
                    pickable.PickUp(handTransform);
                    heldPickable = pickable;
                    return;
                }
            }
        }
    }
}

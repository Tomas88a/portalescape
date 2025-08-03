using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform rotateCenter;
    public Vector3 gravityDirection = new Vector3(0, -1f, 0);
    public Transform body;
    public Rigidbody Playerbody;
    public float jumpForce = 3.81f;

    [Header("移动设置")]
    public float moveSpeed = 5f;           // 移动速度

    [Header("鼠标视角设置")]
    public float mouseSensitivity = 2f;    // 鼠标灵敏度
    public float maxLookAngle = 80f;       // 最大仰视角度

    [Header("音效设置")]
    public AudioSource audioSource;        // 拖 AudioSource 组件进来
    public AudioClip walkClip;             // 行走音效
    public AudioClip jumpClip;             // 跳跃音效
    [HideInInspector]
    public AudioClip landClip;             // 落地音效，GroundCheck中赋值

    private bool isWalking = false;
    private float verticalRotation = 0f;
    public bool canJump = true;

    void Start()
    {
        // 锁定鼠标到屏幕中心并隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();

        gravityDirection = -rotateCenter.up;
        body.rotation = rotateCenter.rotation;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        // 行走音效（仅在地面且有移动时循环）
        if (canJump && isMoving)
        {
            if (!isWalking)
            {
                isWalking = true;
                if (walkClip != null && audioSource != null)
                {
                    audioSource.clip = walkClip;
                    audioSource.loop = true;
                    audioSource.spatialBlend = 1f; // 3D音效
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                if (audioSource != null && audioSource.clip == walkClip)
                    audioSource.Stop();
            }
        }

        // 跳跃
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Playerbody.AddForce(-gravityDirection * jumpForce, ForceMode.Impulse);

            if (jumpClip != null && audioSource != null)
                audioSource.PlayOneShot(jumpClip, 1f);
        }
    }

    void FixedUpdate()
    {
        Playerbody.AddForce(gravityDirection * 9.81f, ForceMode.Acceleration);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 upDirection = -gravityDirection.normalized;
        Vector3 playerForward = transform.forward;
        Vector3 projectedForward = Vector3.ProjectOnPlane(playerForward, gravityDirection.normalized).normalized;

        if (projectedForward.magnitude < 0.1f)
        {
            Vector3 tempVector = Vector3.up;
            if (Vector3.Dot(gravityDirection.normalized, Vector3.up) > 0.9f)
            {
                tempVector = Vector3.forward;
            }
            projectedForward = Vector3.ProjectOnPlane(tempVector, gravityDirection.normalized).normalized;
        }

        Vector3 rightDirection = Vector3.Cross(upDirection, projectedForward).normalized;
        Vector3 forwardDirection = Vector3.Cross(rightDirection, upDirection).normalized;
        Vector3 direction = rightDirection * horizontal + forwardDirection * vertical;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localEulerAngles.y, 0f);
    }
}

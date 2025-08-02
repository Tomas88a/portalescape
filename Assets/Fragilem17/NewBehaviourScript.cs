using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform rotateCenter; 
    public Vector3 gravityDirection = new Vector3(0, -1f, 0);
    public Transform body;
    [Header("移动设置")]
    public float moveSpeed = 5f;           // 移动速度

    [Header("鼠标视角设置")]
    public float mouseSensitivity = 2f;    // 鼠标灵敏度
    public float maxLookAngle = 80f;       // 最大仰视角度

    private float verticalRotation = 0f;   // 垂直旋转角度

    // Start is called before the first frame update
    void Start()
    {
        // 锁定鼠标到屏幕中心并隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleMouseLook();

        gravityDirection = -rotateCenter.up;
    }

    void HandleMovement()
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 键
        float vertical = Input.GetAxis("Vertical");     // W/S 键

        // 根据重力方向计算当前的"上"方向
        Vector3 upDirection = -gravityDirection.normalized;

        // 获取玩家的前进方向（基于当前朝向）
        Vector3 playerForward = transform.forward;

        // 将玩家的前进方向投影到垂直于重力的平面上
        Vector3 projectedForward = Vector3.ProjectOnPlane(playerForward, gravityDirection.normalized).normalized;

        // 如果投影后的向量长度为0（即玩家朝向与重力方向平行），使用一个默认方向
        if (projectedForward.magnitude < 0.1f)
        {
            // 找一个与重力方向垂直的向量作为默认前进方向
            Vector3 tempVector = Vector3.up;
            if (Vector3.Dot(gravityDirection.normalized, Vector3.up) > 0.9f)
            {
                tempVector = Vector3.forward;
            }
            projectedForward = Vector3.ProjectOnPlane(tempVector, gravityDirection.normalized).normalized;
        }

        // 计算右侧方向：使用叉积，确保在垂直于重力的平面上
        Vector3 rightDirection = Vector3.Cross(upDirection, projectedForward).normalized;

        // 重新计算前进方向，确保三个方向互相垂直
        Vector3 forwardDirection = Vector3.Cross(rightDirection, upDirection).normalized;

        // 计算最终移动方向
        Vector3 direction = rightDirection * horizontal + forwardDirection * vertical;

        // 应用移动
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void HandleMouseLook()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 水平旋转（绕Y轴）
        transform.Rotate(Vector3.up * mouseX);

        // 垂直旋转（绕X轴）
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

        // 应用垂直旋转
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localEulerAngles.y, 0f);
    }
}

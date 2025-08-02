using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
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
    }

    void HandleMovement()
    {
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 键
        float vertical = Input.GetAxis("Vertical");     // W/S 键

        // 计算水平移动方向（忽略垂直旋转，只使用Y轴旋转）
        // 获取当前Y轴旋转角度
        float yRotation = transform.eulerAngles.y;

        // 基于Y轴旋转计算前进和右侧方向（保持在水平面）
        Vector3 forward = new Vector3(Mathf.Sin(yRotation * Mathf.Deg2Rad), 0, Mathf.Cos(yRotation * Mathf.Deg2Rad));
        Vector3 right = new Vector3(Mathf.Cos(yRotation * Mathf.Deg2Rad), 0, -Mathf.Sin(yRotation * Mathf.Deg2Rad));

        // 计算移动方向（只在水平面移动）
        Vector3 direction = right * horizontal + forward * vertical;

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

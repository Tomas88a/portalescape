using UnityEngine;

public class FallingNail : MonoBehaviour
{
    [Header("插地设置")]
    public float minDropDistance = 2f; // 掉落多远后插地（可调）
    public LayerMask groundMask;       // 指定地板layer（建议设置，防止插到奇怪物体）
    public float raycastDownLength = 1f; // 插地对齐射线长度

    [Header("插地角度调整")]
    public Vector3 rotationOffset;     // Inspector可调，插地后朝向修正（单位：度）

    [Header("自定义掉落重力")]
    public bool useCustomGravity = false;
    public Vector3 customGravity = new Vector3(0, -9.81f, 0); // 默认向下重力，可自定义

    private Vector3 startPos;
    private Rigidbody rb;
    private bool stuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (rb != null && !stuck && !rb.isKinematic && useCustomGravity)
        {
            rb.AddForce(customGravity, ForceMode.Acceleration);
        }
    }

    void Update()
    {
        if (stuck || rb.isKinematic) return;

        // 距离足够大才插地
        float dropDist = Vector3.Distance(transform.position, startPos);
        if (dropDist >= minDropDistance)
        {
            // 检测是不是快落到地面
            Ray ray = new Ray(transform.position, customGravity.normalized * -1); // 朝重力反方向射线
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDownLength, groundMask))
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;

                // 2. 对齐地面法线（这里可按自定义掉落方向修正）
                transform.position = hit.point;
                transform.up = hit.normal;
                transform.Rotate(rotationOffset, Space.Self);

                stuck = true;
            }
        }
    }
}

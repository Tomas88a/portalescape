using UnityEngine;

public class FallingNail : MonoBehaviour
{
    [Header("插地设置")]
    public float minDropDistance = 2f; // 掉落多远后插地（可调）
    public LayerMask groundMask;       // 指定地板layer（建议设置，防止插到奇怪物体）
    public float raycastDownLength = 1f; // 插地对齐射线长度

    [Header("插地角度调整")]
    public Vector3 rotationOffset;     // Inspector可调，插地后朝向修正（单位：度）

    private Vector3 startPos;
    private Rigidbody rb;
    private bool stuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void Update()
    {
        if (stuck || rb.isKinematic) return;

        // 距离足够大才插地
        float dropDist = Vector3.Distance(transform.position, startPos);
        if (dropDist >= minDropDistance)
        {
            // 检测是不是快落到地面
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDownLength, groundMask))
            {
                // 1. 把rb锁住
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                // 2. 对齐地面法线
                transform.position = hit.point;
                transform.up = hit.normal;

                // 3. 叠加Inspector可调旋转（本地欧拉角偏移）
                transform.Rotate(rotationOffset, Space.Self);

                stuck = true;
            }
        }
    }
}

using UnityEngine;

public class FallingNail : MonoBehaviour
{
    [Header("插地设置")]
    public float minDropDistance = 2f;
    public LayerMask groundMask;
    public float raycastDownLength = 1f;

    [Header("插地角度调整")]
    public Vector3 rotationOffset;

    [Header("自定义掉落重力")]
    public bool useCustomGravity = false;
    public Vector3 customGravity = new Vector3(0, -9.81f, 0);

    [Header("高级选项")]
    public bool ignorePlayerCollision = false; // 新增！仅勾选的钉子会忽略玩家

    private Vector3 startPos;
    private Rigidbody rb;
    private bool stuck = false;

    // ↓ 用于多collider支持
    private Collider[] playerColliders;
    private Collider myCol;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        myCol = GetComponent<Collider>();

        if (ignorePlayerCollision)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerColliders = player.GetComponentsInChildren<Collider>();
                foreach (var pc in playerColliders)
                {
                    if (myCol != null && pc != null)
                        Physics.IgnoreCollision(myCol, pc, true);
                }
            }
        }
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

        float dropDist = Vector3.Distance(transform.position, startPos);
        if (dropDist >= minDropDistance)
        {
            Ray ray = new Ray(transform.position, customGravity.normalized * -1);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDownLength, groundMask))
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;

                transform.position = hit.point;
                transform.up = hit.normal;
                transform.Rotate(rotationOffset, Space.Self);

                stuck = true;

                // ★ 如果你想插地后恢复与player的碰撞，可在此取消 IgnoreCollision
                // if (ignorePlayerCollision && playerColliders != null)
                // {
                //     foreach (var pc in playerColliders)
                //     {
                //         if (myCol != null && pc != null)
                //             Physics.IgnoreCollision(myCol, pc, false);
                //     }
                // }
            }
        }
    }
}

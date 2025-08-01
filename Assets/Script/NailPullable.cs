using UnityEngine;

public class NailPullable : MonoBehaviour
{
    [Header("拔出参数")]
    public Vector3 pullDirection = Vector3.forward;   // 拔出方向
    public float pullStep = 0.05f;                    // 每次点击拔出的距离
    public int totalSteps = 10;                       // 拔几步才能完全拔出
    public float rotateStep = 10f;                    // 每次点击旋转的角度（度）

    [Header("掉落参数")]
    public Rigidbody rb;                              // 钉子的刚体组件
    public Collider coll;                             // 钉子的碰撞体

    private int currentStep = 0;
    private bool isPulledOut = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (coll == null) coll = GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true; // 初始钉子不能掉落
    }

    // 玩家Controller里调用
    public void PullOnce()
    {
        if (isPulledOut) return;

        // 移动
        transform.position += pullDirection.normalized * pullStep;
        // 旋转
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);
        currentStep++;

        if (currentStep >= totalSteps)
        {
            isPulledOut = true;
            if (rb != null) rb.isKinematic = false;
            Debug.Log("钉子被完全拔出，掉落！");
        }
    }
}

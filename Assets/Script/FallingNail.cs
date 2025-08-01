using UnityEngine;

public class FallingNail : MonoBehaviour
{
    [Header("�������")]
    public float minDropDistance = 2f; // �����Զ���أ��ɵ���
    public LayerMask groundMask;       // ָ���ذ�layer���������ã���ֹ�嵽������壩
    public float raycastDownLength = 1f; // ��ض������߳���

    [Header("��ؽǶȵ���")]
    public Vector3 rotationOffset;     // Inspector�ɵ�����غ�����������λ���ȣ�

    [Header("�Զ����������")]
    public bool useCustomGravity = false;
    public Vector3 customGravity = new Vector3(0, -9.81f, 0); // Ĭ���������������Զ���

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

        // �����㹻��Ų��
        float dropDist = Vector3.Distance(transform.position, startPos);
        if (dropDist >= minDropDistance)
        {
            // ����ǲ��ǿ��䵽����
            Ray ray = new Ray(transform.position, customGravity.normalized * -1); // ����������������
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDownLength, groundMask))
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;

                // 2. ������淨�ߣ�����ɰ��Զ�����䷽��������
                transform.position = hit.point;
                transform.up = hit.normal;
                transform.Rotate(rotationOffset, Space.Self);

                stuck = true;
            }
        }
    }
}

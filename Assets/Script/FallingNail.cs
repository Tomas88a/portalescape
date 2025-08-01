using UnityEngine;

public class FallingNail : MonoBehaviour
{
    [Header("�������")]
    public float minDropDistance = 2f; // �����Զ���أ��ɵ���
    public LayerMask groundMask;       // ָ���ذ�layer���������ã���ֹ�嵽������壩
    public float raycastDownLength = 1f; // ��ض������߳���

    [Header("��ؽǶȵ���")]
    public Vector3 rotationOffset;     // Inspector�ɵ�����غ�����������λ���ȣ�

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

        // �����㹻��Ų��
        float dropDist = Vector3.Distance(transform.position, startPos);
        if (dropDist >= minDropDistance)
        {
            // ����ǲ��ǿ��䵽����
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDownLength, groundMask))
            {
                // 1. ��rb��ס
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                // 2. ������淨��
                transform.position = hit.point;
                transform.up = hit.normal;

                // 3. ����Inspector�ɵ���ת������ŷ����ƫ�ƣ�
                transform.Rotate(rotationOffset, Space.Self);

                stuck = true;
            }
        }
    }
}

using UnityEngine;

public class NailPullable : MonoBehaviour
{
    [Header("�γ�����")]
    public Vector3 pullDirection = Vector3.forward;   // �γ�����
    public float pullStep = 0.05f;                    // ÿ�ε���γ��ľ���
    public int totalSteps = 10;                       // �μ���������ȫ�γ�
    public float rotateStep = 10f;                    // ÿ�ε����ת�ĽǶȣ��ȣ�

    [Header("�������")]
    public Rigidbody rb;                              // ���ӵĸ������
    public Collider coll;                             // ���ӵ���ײ��

    private int currentStep = 0;
    private bool isPulledOut = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (coll == null) coll = GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true; // ��ʼ���Ӳ��ܵ���
    }

    // ���Controller�����
    public void PullOnce()
    {
        if (isPulledOut) return;

        // �ƶ�
        transform.position += pullDirection.normalized * pullStep;
        // ��ת
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);
        currentStep++;

        if (currentStep >= totalSteps)
        {
            isPulledOut = true;
            if (rb != null) rb.isKinematic = false;
            Debug.Log("���ӱ���ȫ�γ������䣡");
        }
    }
}

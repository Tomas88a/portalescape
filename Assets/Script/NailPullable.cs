using UnityEngine;

public class NailPullable : MonoBehaviour
{
    [Header("�γ�����")]
    public Vector3 pullDirection = Vector3.forward;
    public float pullStep = 0.05f;
    public int totalSteps = 10;
    public float rotateStep = 10f;

    [Header("�������")]
    public Rigidbody rb;
    public Collider coll;

    private int currentStep = 0;
    private bool isPulledOut = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (coll == null) coll = GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true;
    }

    public void PullOnce()
    {
        if (isPulledOut) return;

        transform.position += pullDirection.normalized * pullStep;
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);
        currentStep++;

        if (currentStep >= totalSteps)
        {
            isPulledOut = true;
            if (rb != null) rb.isKinematic = false;
            // ����������NailPullManager
            if (NailPullManager.Instance != null)
                NailPullManager.Instance.AddNail();
            Destroy(gameObject, 0.5f); // ��������ٶ���
        }
    }
}

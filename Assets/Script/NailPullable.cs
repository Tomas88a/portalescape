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

    [Header("�γ�����")]
    public bool destroyOnPull = true;    // �γ����Ƿ����٣�ǽ����true���Ŷ���false��
    [Header("Ŀ��������ӣ������/����/����UI��")]
    public bool countAsGoalNail = true;  // �Ŷ���false���ɲ�����

    [Header("��Ч&����")]
    public AudioClip pullSound;          // �γ�ʱ����Ч��ÿ�����һ�£�
    public AudioClip dropSound;          // ���ӵ���ʱ��Ч
    public Animator animator;            // ���ʱ����
    public string insertAnimName = "Insert"; // ��ض�����

    private int currentStep = 0;
    private bool isPulledOut = false;
    private AudioSource audioSource;
    private bool dropSoundPlayed = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (coll == null) coll = GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PullOnce()
    {
        if (isPulledOut) return;

        transform.position += pullDirection.normalized * pullStep;
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);
        currentStep++;

        // �γ���Ч
        if (pullSound)
            audioSource.PlayOneShot(pullSound);

        if (currentStep >= totalSteps)
        {
            isPulledOut = true;
            if (rb != null) rb.isKinematic = false;

            // ֻͳ��Ŀ�궤��
            if (countAsGoalNail && NailPullManager.Instance != null)
                NailPullManager.Instance.AddNail();

            if (destroyOnPull)
            {
                Destroy(gameObject, 0.5f); // ���������
            }
            // ����ȵ���ű�/FallingNailȥ����
        }
    }

    // �������ӵ��䵽����ʱ����Ч�Ͳ�ض��������Ŷ��ã�destroyOnPull=falseʱ�����ã�
    void OnCollisionEnter(Collision collision)
    {
        if (isPulledOut && !destroyOnPull && !dropSoundPlayed)
        {
            // �ж��ǲ��ǵ��棨�ɼ� Layer ����Ż���
            if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal == Vector3.up)
            {
                if (dropSound)
                    audioSource.PlayOneShot(dropSound);

                dropSoundPlayed = true;

                // ��ض���
                if (animator && !string.IsNullOrEmpty(insertAnimName))
                {
                    animator.Play(insertAnimName);
                }

                // ���ӳ��ײ��
                if (rb != null) rb.isKinematic = true;
            }
        }
    }
}

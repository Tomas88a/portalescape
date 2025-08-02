using UnityEngine;
using System.Collections;

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
    public bool destroyOnPull = true;        // �γ����Ƿ����٣�ǽ����true���Ŷ���false��
    [Header("Ŀ��������ӣ������/����/����UI��")]
    public bool countAsGoalNail = true;      // �Ŷ���false���ɲ�����

    [Header("�γ��󼤻��Trigger����ѡ��")]
    public GameObject triggerToActivate;     // �γ���Ҫ�����trigger

    [Header("��Ч&����")]
    public AudioClip pullSound;
    public AudioClip dropSound;
    public Animator animator;
    public string insertAnimName = "Insert";

    [Header("�γ�����")]
    public Color flashColor = Color.white;   // ��˸��ɫ
    public float flashDuration = 0.08f;      // ��˸ʱ��

    private int currentStep = 0;
    private bool isPulledOut = false;
    private AudioSource audioSource;
    private bool dropSoundPlayed = false;

    // ������˸Ч��
    private Renderer[] allRenderers;
    private Color[][] originalColors;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (coll == null) coll = GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // ��˸��س�ʼ��
        allRenderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[allRenderers.Length][];
        for (int i = 0; i < allRenderers.Length; i++)
        {
            var mats = allRenderers[i].materials;
            originalColors[i] = new Color[mats.Length];
            for (int j = 0; j < mats.Length; j++)
                originalColors[i][j] = mats[j].color;
        }
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

        // �γ�һ�ξ���˸
        StartCoroutine(FlashEffect());

        if (currentStep >= totalSteps)
        {
            isPulledOut = true;
            if (rb != null) rb.isKinematic = false;

            if (countAsGoalNail && NailPullManager.Instance != null)
                NailPullManager.Instance.AddNail();

            if (triggerToActivate != null)
            {
                triggerToActivate.SetActive(true);
                Debug.Log("�γ����Ӻ��Ѽ���Trigger: " + triggerToActivate.name);
            }

            if (destroyOnPull)
            {
                Destroy(gameObject, 0.5f); // ���������
            }
        }
    }

    private IEnumerator FlashEffect()
    {
        // 1. ��˸��ɫ
        for (int i = 0; i < allRenderers.Length; i++)
        {
            var mats = allRenderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = flashColor;
            }
        }
        yield return new WaitForSeconds(flashDuration);
        // 2. �ָ�ԭɫ
        for (int i = 0; i < allRenderers.Length; i++)
        {
            var mats = allRenderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = originalColors[i][j];
            }
        }
    }

    // ���ӵ��䵽����ʱ����Ч�Ͳ�ض���
    void OnCollisionEnter(Collision collision)
    {
        if (isPulledOut && !destroyOnPull && !dropSoundPlayed)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal == Vector3.up)
            {
                if (dropSound)
                    audioSource.PlayOneShot(dropSound);

                dropSoundPlayed = true;

                if (animator && !string.IsNullOrEmpty(insertAnimName))
                {
                    animator.Play(insertAnimName);
                }

                if (rb != null) rb.isKinematic = true;
            }
        }
    }
}

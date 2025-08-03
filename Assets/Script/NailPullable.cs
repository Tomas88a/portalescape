using UnityEngine;
using System.Collections;
using Fragilem17.MirrorsAndPortals;

public class NailPullable : MonoBehaviour
{
    [Header("�γ�����")]
    public Vector3 pullDirection = Vector3.forward;
    public float pullStep = 0.05f;         // ÿ�ΰγ�һС��
    public int totalSteps = 5;             // �ܹ��ܰμ��Σ���ͷ�����/��ʧ��
    public float rotateStep = 10f;         // ÿ����ת�Ƕ�

    [Header("�γ�����")]
    public Color flashColor = Color.white;
    public float flashDuration = 0.08f;
    public AudioClip pullSound;
    public AudioSource audioSource;
    public Animator animator;
    public string pullAnimName = "Pull";
    public bool destroyOnPull = true;      // �γ����Ƿ����٣����䶤�ӿ���Ϊfalse��

    [Header("��Ϊ���ռ���Ŀ��")]
    public bool countAsGoalNail = false;   // �Ƿ����NailPullManager����

    [Header("�γ��󼤻�Trigger")]
    public GameObject triggerToActivate;   // �γ��󼤻�ָ��trigger

    [Header("�γ���Portal����")]
    public Portal portalToSet;             // �γ���Ҫ���õ�Portal
    public Portal newOtherPortal;          // Ҫָ��ΪOtherPortal��Portal

    [Header("��Ļ����")]
    [Tooltip("ÿ�����Ӷ�Ӧ����Ļ�ı�")]
    public string nailSubtitle;
    [Tooltip("��Ļ��ʾʱ�����룩")]
    public float subtitleDuration = 2.5f;

    private int currentStep = 0;
    private bool isPulledOut = false;
    private Rigidbody rb;
    private Renderer[] allRenderers;
    private Color[][] originalColors;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

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

        // ÿ��������һ��
        transform.position += pullDirection.normalized * pullStep;
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);
        currentStep++;

        // ��Ч
        if (pullSound)
            audioSource.PlayOneShot(pullSound);

        // ��˸����
        StartCoroutine(FlashEffect());

        // ����
        if (animator && !string.IsNullOrEmpty(pullAnimName))
            animator.Play(pullAnimName);

        // �ε�ͷ
        if (currentStep >= totalSteps)
        {
            isPulledOut = true;

            // ��ʾ��Ļ
            if (!string.IsNullOrEmpty(nailSubtitle) && SubtitleManager.Instance != null)
                SubtitleManager.Instance.ShowSubtitle(nailSubtitle, subtitleDuration);

            // ����
            if (countAsGoalNail && NailPullManager.Instance != null)
                NailPullManager.Instance.AddNail();

            // ����trigger
            if (triggerToActivate != null)
            {
                triggerToActivate.SetActive(true);
                Debug.Log("�γ����Ӻ��Ѽ���Trigger: " + triggerToActivate.name);
            }

            // ����Portal��OtherPortal
            if (portalToSet != null && newOtherPortal != null)
            {
                portalToSet.OtherPortal = newOtherPortal;
                Debug.Log("������Portal: " + portalToSet.name + " ��OtherPortalΪ: " + newOtherPortal.name);
            }

            // ��������
            if (rb != null) rb.isKinematic = false;

            // ����
            if (destroyOnPull)
                Destroy(gameObject, 0.5f);
        }
    }

    private IEnumerator FlashEffect()
    {
        for (int i = 0; i < allRenderers.Length; i++)
        {
            var mats = allRenderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
                mats[j].color = flashColor;
        }
        yield return new WaitForSeconds(flashDuration);
        for (int i = 0; i < allRenderers.Length; i++)
        {
            var mats = allRenderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
                mats[j].color = originalColors[i][j];
        }
    }

    [ContextMenu("PullOnce_ContextMenu")]
    public void PullOnce_ContextMenu()
    {
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
        PullOnce(); 
    }
}

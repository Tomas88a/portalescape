using UnityEngine;
using System.Collections;

public class NailLimitedPull : MonoBehaviour
{
    [Header("�γ�����")]
    public Vector3 pullDirection = Vector3.forward;
    public float pullStep = 0.05f;    // ÿ�ΰγ��ľ���
    public int maxPullTimes = 3;      // ���ɰμ���
    public float rotateStep = 10f;    // ÿ����ת�Ƕ�

    [Header("��Ч&����")]
    public AudioClip pullSound;
    public Animator animator;
    public string pullAnimName = "Pull"; // ���Զ��嶯����

    [Header("�γ�����")]
    public Color flashColor = Color.white;
    public float flashDuration = 0.08f;

    private int currentPullCount = 0;
    private AudioSource audioSource;
    private Renderer[] allRenderers;
    private Color[][] originalColors;
    private bool reachedLimit = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

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

    // �ⲿ���ã�����PlayerController������ÿ�ε��ó��԰γ�һ��
    public void PullOnce()
    {
        if (reachedLimit) return;

        transform.position += pullDirection.normalized * pullStep;
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);

        // ��Ч
        if (pullSound)
            audioSource.PlayOneShot(pullSound);

        // ��˸����
        StartCoroutine(FlashEffect());

        // ���Ŷ�������ѡ��
        if (animator && !string.IsNullOrEmpty(pullAnimName))
            animator.Play(pullAnimName);

        currentPullCount++;
        if (currentPullCount >= maxPullTimes)
        {
            reachedLimit = true;
            // �������ﴥ�����ӱ�ɫ/���ý���������Ч��
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
}

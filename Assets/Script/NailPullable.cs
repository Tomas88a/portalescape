using UnityEngine;
using System.Collections;
using Fragilem17.MirrorsAndPortals;

public class NailPullable : MonoBehaviour
{
    [Header("拔出参数")]
    public Vector3 pullDirection = Vector3.forward;
    public float pullStep = 0.05f;         // 每次拔出一小段
    public int totalSteps = 5;             // 总共能拔几次（到头后掉落/消失）
    public float rotateStep = 10f;         // 每次旋转角度

    [Header("拔出反馈")]
    public Color flashColor = Color.white;
    public float flashDuration = 0.08f;
    public AudioClip pullSound;
    public AudioSource audioSource;
    public Animator animator;
    public string pullAnimName = "Pull";
    public bool destroyOnPull = true;      // 拔出后是否销毁（掉落钉子可设为false）

    [Header("作为解谜计数目标")]
    public bool countAsGoalNail = false;   // 是否参与NailPullManager计数

    [Header("拔出后激活Trigger")]
    public GameObject triggerToActivate;   // 拔出后激活指定trigger

    [Header("拔出后Portal关联")]
    public Portal portalToSet;             // 拔出后要设置的Portal
    public Portal newOtherPortal;          // 要指定为OtherPortal的Portal

    [Header("字幕功能")]
    [Tooltip("每根钉子对应的字幕文本")]
    public string nailSubtitle;
    [Tooltip("字幕显示时长（秒）")]
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

        // 每次往外拉一点
        transform.position += pullDirection.normalized * pullStep;
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);
        currentStep++;

        // 音效
        if (pullSound)
            audioSource.PlayOneShot(pullSound);

        // 闪烁反馈
        StartCoroutine(FlashEffect());

        // 动画
        if (animator && !string.IsNullOrEmpty(pullAnimName))
            animator.Play(pullAnimName);

        // 拔到头
        if (currentStep >= totalSteps)
        {
            isPulledOut = true;

            // 显示字幕
            if (!string.IsNullOrEmpty(nailSubtitle) && SubtitleManager.Instance != null)
                SubtitleManager.Instance.ShowSubtitle(nailSubtitle, subtitleDuration);

            // 计数
            if (countAsGoalNail && NailPullManager.Instance != null)
                NailPullManager.Instance.AddNail();

            // 激活trigger
            if (triggerToActivate != null)
            {
                triggerToActivate.SetActive(true);
                Debug.Log("拔出钉子后已激活Trigger: " + triggerToActivate.name);
            }

            // 设置Portal的OtherPortal
            if (portalToSet != null && newOtherPortal != null)
            {
                portalToSet.OtherPortal = newOtherPortal;
                Debug.Log("已设置Portal: " + portalToSet.name + " 的OtherPortal为: " + newOtherPortal.name);
            }

            // 允许掉落
            if (rb != null) rb.isKinematic = false;

            // 销毁
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
}

using UnityEngine;
using System.Collections;

public class NailLimitedPull : MonoBehaviour
{
    [Header("拔出参数")]
    public Vector3 pullDirection = Vector3.forward;
    public float pullStep = 0.05f;    // 每次拔出的距离
    public int maxPullTimes = 3;      // 最多可拔几次
    public float rotateStep = 10f;    // 每次旋转角度

    [Header("音效&动画")]
    public AudioClip pullSound;
    public Animator animator;
    public string pullAnimName = "Pull"; // 可自定义动画名

    [Header("拔出反馈")]
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

    // 外部调用（比如PlayerController）——每次调用尝试拔出一次
    public void PullOnce()
    {
        if (reachedLimit) return;

        transform.position += pullDirection.normalized * pullStep;
        transform.Rotate(Vector3.forward, rotateStep, Space.Self);

        // 音效
        if (pullSound)
            audioSource.PlayOneShot(pullSound);

        // 闪烁反馈
        StartCoroutine(FlashEffect());

        // 播放动画（可选）
        if (animator && !string.IsNullOrEmpty(pullAnimName))
            animator.Play(pullAnimName);

        currentPullCount++;
        if (currentPullCount >= maxPullTimes)
        {
            reachedLimit = true;
            // 可在这里触发钉子变色/禁用交互等其它效果
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

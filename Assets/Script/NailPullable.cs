using UnityEngine;
using System.Collections;

public class NailPullable : MonoBehaviour
{
    [Header("拔出参数")]
    public Vector3 pullDirection = Vector3.forward;
    public float pullStep = 0.05f;
    public int totalSteps = 10;
    public float rotateStep = 10f;

    [Header("掉落参数")]
    public Rigidbody rb;
    public Collider coll;

    [Header("拔出后处理")]
    public bool destroyOnPull = true;        // 拔出后是否销毁（墙钉用true，桥钉用false）
    [Header("目标计数钉子（会计数/开门/触发UI）")]
    public bool countAsGoalNail = true;      // 桥钉设false即可不计数

    [Header("拔出后激活的Trigger（可选）")]
    public GameObject triggerToActivate;     // 拔出后要激活的trigger

    [Header("音效&动画")]
    public AudioClip pullSound;
    public AudioClip dropSound;
    public Animator animator;
    public string insertAnimName = "Insert";

    [Header("拔出反馈")]
    public Color flashColor = Color.white;   // 闪烁颜色
    public float flashDuration = 0.08f;      // 闪烁时间

    private int currentStep = 0;
    private bool isPulledOut = false;
    private AudioSource audioSource;
    private bool dropSoundPlayed = false;

    // 用于闪烁效果
    private Renderer[] allRenderers;
    private Color[][] originalColors;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (coll == null) coll = GetComponent<Collider>();
        if (rb != null) rb.isKinematic = true;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // 闪烁相关初始化
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

        // 拔出音效
        if (pullSound)
            audioSource.PlayOneShot(pullSound);

        // 拔出一段就闪烁
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
                Debug.Log("拔出钉子后已激活Trigger: " + triggerToActivate.name);
            }

            if (destroyOnPull)
            {
                Destroy(gameObject, 0.5f); // 半秒后销毁
            }
        }
    }

    private IEnumerator FlashEffect()
    {
        // 1. 闪烁变色
        for (int i = 0; i < allRenderers.Length; i++)
        {
            var mats = allRenderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = flashColor;
            }
        }
        yield return new WaitForSeconds(flashDuration);
        // 2. 恢复原色
        for (int i = 0; i < allRenderers.Length; i++)
        {
            var mats = allRenderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = originalColors[i][j];
            }
        }
    }

    // 钉子掉落到地面时的音效和插地动画
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

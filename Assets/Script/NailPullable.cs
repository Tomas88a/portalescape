using UnityEngine;

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
    public bool destroyOnPull = true;    // 拔出后是否销毁（墙钉用true，桥钉用false）
    [Header("目标计数钉子（会计数/开门/触发UI）")]
    public bool countAsGoalNail = true;  // 桥钉设false即可不计数

    [Header("音效&动画")]
    public AudioClip pullSound;          // 拔出时的音效（每次左键一下）
    public AudioClip dropSound;          // 钉子掉地时音效
    public Animator animator;            // 插地时动画
    public string insertAnimName = "Insert"; // 插地动画名

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

        // 拔出音效
        if (pullSound)
            audioSource.PlayOneShot(pullSound);

        if (currentStep >= totalSteps)
        {
            isPulledOut = true;
            if (rb != null) rb.isKinematic = false;

            // 只统计目标钉子
            if (countAsGoalNail && NailPullManager.Instance != null)
                NailPullManager.Instance.AddNail();

            if (destroyOnPull)
            {
                Destroy(gameObject, 0.5f); // 半秒后销毁
            }
            // 否则等掉落脚本/FallingNail去处理
        }
    }

    // 新增钉子掉落到地面时的音效和插地动画（给桥钉用，destroyOnPull=false时会启用）
    void OnCollisionEnter(Collision collision)
    {
        if (isPulledOut && !destroyOnPull && !dropSoundPlayed)
        {
            // 判断是不是地面（可加 Layer 检测优化）
            if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal == Vector3.up)
            {
                if (dropSound)
                    audioSource.PlayOneShot(dropSound);

                dropSoundPlayed = true;

                // 插地动画
                if (animator && !string.IsNullOrEmpty(insertAnimName))
                {
                    animator.Play(insertAnimName);
                }

                // 钉子彻底插地
                if (rb != null) rb.isKinematic = true;
            }
        }
    }
}

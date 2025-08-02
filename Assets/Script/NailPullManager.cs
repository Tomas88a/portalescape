using UnityEngine;
using TMPro;
using System.Collections;

public class NailPullManager : MonoBehaviour
{
    public static NailPullManager Instance;
    public int totalNails = 2;
    [HideInInspector]
    public int pulledNails = 0;
    public DoorInteraction targetDoor;
    public GameObject[] doorNailPrefabs;
    public Transform[] doorNailSlots;
    public Vector3[] rotationOffsets;
    public TextMeshProUGUI nailCounterText;

    [Header("闪烁设置")]
    public int flashTimes = 4;      // 闪烁次数
    public float flashInterval = 0.18f; // 闪烁间隔时间

    [Header("音效")]
    public AudioSource audioSource;
    public AudioClip unlockClip;

    private void Awake()
    {
        Instance = this;
        if (nailCounterText)
            nailCounterText.gameObject.SetActive(false);
    }

    public void AddNail()
    {
        pulledNails++;

        if (pulledNails == 1 && nailCounterText)
            nailCounterText.gameObject.SetActive(true);

        UpdateUI();

        if (pulledNails <= doorNailPrefabs.Length && pulledNails <= doorNailSlots.Length)
        {
            Vector3 offset = Vector3.zero;
            if (rotationOffsets != null && rotationOffsets.Length >= pulledNails)
                offset = rotationOffsets[pulledNails - 1];

            Quaternion finalRotation = doorNailSlots[pulledNails - 1].rotation * Quaternion.Euler(offset);
            Instantiate(
                doorNailPrefabs[pulledNails - 1],
                doorNailSlots[pulledNails - 1].position,
                finalRotation,
                doorNailSlots[pulledNails - 1]
            );
        }

        if (pulledNails >= totalNails)
        {
            // 播放解锁音效
            if (audioSource != null && unlockClip != null)
                audioSource.PlayOneShot(unlockClip);

            if (targetDoor != null)
                targetDoor.OpenDoor();
            if (nailCounterText)
                StartCoroutine(FlashAndHideUI());
        }
    }

    void UpdateUI()
    {
        if (nailCounterText)
            nailCounterText.text = $"Nail{pulledNails}/{totalNails}";
    }

    IEnumerator FlashAndHideUI()
    {
        for (int i = 0; i < flashTimes; i++)
        {
            nailCounterText.gameObject.SetActive(false);
            yield return new WaitForSeconds(flashInterval);
            nailCounterText.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashInterval);
        }
        nailCounterText.gameObject.SetActive(false);
    }
}

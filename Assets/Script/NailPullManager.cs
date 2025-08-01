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

    private void Awake()
    {
        Instance = this;
        // 一开始不显示UI
        if (nailCounterText)
            nailCounterText.gameObject.SetActive(false);
    }

    public void AddNail()
    {
        pulledNails++;

        // 第一次拔出时才显示UI
        if (pulledNails == 1 && nailCounterText)
            nailCounterText.gameObject.SetActive(true);

        UpdateUI();

        // 门上长钉子
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
            if (targetDoor != null)
                targetDoor.OpenDoor();
            // UI闪烁再隐藏
            if (nailCounterText)
                StartCoroutine(FlashAndHideUI());
        }
    }

    void UpdateUI()
    {
        if (nailCounterText)
            nailCounterText.text = $"Nail{pulledNails}/{totalNails}";
    }

    // 协程：闪烁几下再隐藏
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

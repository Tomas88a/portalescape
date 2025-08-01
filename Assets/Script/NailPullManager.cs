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

    [Header("��˸����")]
    public int flashTimes = 4;      // ��˸����
    public float flashInterval = 0.18f; // ��˸���ʱ��

    private void Awake()
    {
        Instance = this;
        // һ��ʼ����ʾUI
        if (nailCounterText)
            nailCounterText.gameObject.SetActive(false);
    }

    public void AddNail()
    {
        pulledNails++;

        // ��һ�ΰγ�ʱ����ʾUI
        if (pulledNails == 1 && nailCounterText)
            nailCounterText.gameObject.SetActive(true);

        UpdateUI();

        // ���ϳ�����
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
            // UI��˸������
            if (nailCounterText)
                StartCoroutine(FlashAndHideUI());
        }
    }

    void UpdateUI()
    {
        if (nailCounterText)
            nailCounterText.text = $"Nail{pulledNails}/{totalNails}";
    }

    // Э�̣���˸����������
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

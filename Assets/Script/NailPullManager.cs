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

    [Header("��Ч")]
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
            // ���Ž�����Ч
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

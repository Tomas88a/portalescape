using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("�Ŷ�������")]
    public Transform door; // ������壨��ת����
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("����ת����")]
    public Vector3 openAxis = Vector3.up;   // ��ת�ᣨĬ��Y�ᣬǽ�ſ�����ΪVector3.right�ȣ�
    public bool useWorldAxis = false;       // �Ƿ���������
    public float closedAngle = 0f;          // ���ŽǶ�

    [Header("����Ч")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioSource audioSource;

    private bool isOpen = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private Quaternion initialRotation;

    void Start()
    {
        if (door == null) door = transform;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        targetAngle = closedAngle;
        currentAngle = closedAngle;
        initialRotation = door.localRotation;
    }

    void Update()
    {
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * openSpeed);

        // ����ָ������ת
        Quaternion targetRot = useWorldAxis ?
            Quaternion.AngleAxis(currentAngle, openAxis) * initialRotation
            : Quaternion.AngleAxis(currentAngle, door.TransformDirection(openAxis)) * initialRotation;

        door.localRotation = Quaternion.Lerp(door.localRotation, targetRot, Time.deltaTime * openSpeed);
    }


    [ContextMenu("OpenDoor")]
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            targetAngle = openAngle + closedAngle;
            if (openSound != null && audioSource != null)
                audioSource.PlayOneShot(openSound);
        }
    }

    [ContextMenu("CloseDoor")]
    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            targetAngle = closedAngle;
            if (closeSound != null && audioSource != null)
                audioSource.PlayOneShot(closeSound);
        }
    }
}

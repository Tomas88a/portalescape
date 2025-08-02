using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("门动画参数")]
    public Transform door; // 你的门体（旋转对象）
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("门旋转设置")]
    public Vector3 openAxis = Vector3.up;   // 旋转轴（默认Y轴，墙门可设置为Vector3.right等）
    public bool useWorldAxis = false;       // 是否用世界轴
    public float closedAngle = 0f;          // 关门角度

    [Header("门音效")]
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

        // 根据指定轴旋转
        Quaternion targetRot = useWorldAxis ?
            Quaternion.AngleAxis(currentAngle, openAxis) * initialRotation
            : Quaternion.AngleAxis(currentAngle, door.TransformDirection(openAxis)) * initialRotation;

        door.localRotation = Quaternion.Lerp(door.localRotation, targetRot, Time.deltaTime * openSpeed);
    }

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

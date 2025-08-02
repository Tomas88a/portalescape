using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("门动画参数")]
    public Transform door; // 你的门体（旋转对象）
    public float openAngle = 90f;
    public float openSpeed = 2f;

    [Header("门音效")]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioSource audioSource;

    private bool isOpen = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;

    void Start()
    {
        if (door == null) door = transform;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        targetAngle = 0f;
    }

    void Update()
    {
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * openSpeed);
        door.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            targetAngle = openAngle;
            if (openSound != null && audioSource != null)
                audioSource.PlayOneShot(openSound);
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            targetAngle = 0f;
            if (closeSound != null && audioSource != null)
                audioSource.PlayOneShot(closeSound);
        }
    }
}

using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 3f;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = transform.rotation * Quaternion.Euler(0, openAngle, 0);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        // Æ½»¬Ðý×ª
        if (isOpen)
            transform.rotation = Quaternion.Lerp(transform.rotation, openRotation, Time.deltaTime * openSpeed);
        else
            transform.rotation = Quaternion.Lerp(transform.rotation, closedRotation, Time.deltaTime * openSpeed);
    }
    public void OpenDoor()
    {
        isOpen = true;
    }
    public void CloseDoor()
    {
        isOpen = false;
    }

}

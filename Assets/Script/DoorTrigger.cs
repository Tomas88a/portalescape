using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorInteraction door;
    public float delayToClose = 1.0f; // �ӳٹ���ʱ�䣨�룩

    private bool playerInside = false;
    private float exitTime = 0f;



    [ContextMenu("OpenDoor")]
    public void OpenDoor()
    {
        playerInside = true;
        door.OpenDoor();
        CancelInvoke(nameof(CloseIfStillOutside));
    }


    [ContextMenu("CloseDoor")]
    public void CloseDoor()
    {
        playerInside = false;
        exitTime = Time.time;
        Invoke(nameof(CloseIfStillOutside), delayToClose);
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            door.OpenDoor();
            CancelInvoke(nameof(CloseIfStillOutside));
        }
    }

    [ContextMenu("CloseDoor")]
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            exitTime = Time.time;
            Invoke(nameof(CloseIfStillOutside), delayToClose);
        }
    }

    void CloseIfStillOutside()
    {
        if (!playerInside)
        {
            door.CloseDoor();
        }
    }
}

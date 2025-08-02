using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorInteraction door;
    public float delayToClose = 1.0f; // 延迟关门时间（秒）

    private bool playerInside = false;
    private float exitTime = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            door.OpenDoor();
            CancelInvoke(nameof(CloseIfStillOutside));
        }
    }

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

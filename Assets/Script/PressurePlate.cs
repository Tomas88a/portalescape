using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public DoorInteraction door; // 拖入需要控制的门（DoorInteraction脚本）

    private void OnTriggerEnter(Collider other)
    {
        // 检查进入的是可以放在地上的方块（比如有"Pickable" tag）
        if (other.CompareTag("Pickable"))
        {
            if (door != null)
            {
                door.OpenDoor(); // 打开门的方法
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 离开时关闭门（如需保持打开可省略此方法）
        if (other.CompareTag("Pickable"))
        {
            if (door != null)
            {
                door.CloseDoor(); // 关闭门的方法，如果有
            }
        }
    }
}

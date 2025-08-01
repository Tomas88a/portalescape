using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public DoorInteraction door; // ������Ҫ���Ƶ��ţ�DoorInteraction�ű���

    private void OnTriggerEnter(Collider other)
    {
        // ��������ǿ��Է��ڵ��ϵķ��飨������"Pickable" tag��
        if (other.CompareTag("Pickable"))
        {
            if (door != null)
            {
                door.OpenDoor(); // ���ŵķ���
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �뿪ʱ�ر��ţ����豣�ִ򿪿�ʡ�Դ˷�����
        if (other.CompareTag("Pickable"))
        {
            if (door != null)
            {
                door.CloseDoor(); // �ر��ŵķ����������
            }
        }
    }
}

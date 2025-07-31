using UnityEngine;

public class PortalTeleportWithPuzzle : MonoBehaviour
{
    public Transform newSpawnPoint;  // ����Ŀ��㣨�·����ſڣ�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PuzzleBall.solved)
            {
                // ������
                CharacterController cc = other.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                    other.transform.position = newSpawnPoint.position;
                    cc.enabled = true;
                }
                else
                {
                    other.transform.position = newSpawnPoint.position;
                }
                Debug.Log("���ͳɹ���");
            }
            else
            {
                Debug.Log("�����ţ�����Ҫ�Ƚ��գ������򣩣�");
            }
        }
    }
}

using UnityEngine;

public class PortalTeleportWithPuzzle : MonoBehaviour
{
    public Transform newSpawnPoint;  // 传送目标点（新房间门口）

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PuzzleBall.solved)
            {
                // 允许传送
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
                Debug.Log("传送成功！");
            }
            else
            {
                Debug.Log("门锁着，你需要先解谜（碰到球）！");
            }
        }
    }
}

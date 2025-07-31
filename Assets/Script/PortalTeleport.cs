using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform targetSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                // 禁用 CharacterController
                cc.enabled = false;
                other.transform.position = targetSpawnPoint.position;
                cc.enabled = true;
            }
            else
            {
                // 保险起见，附带Rigidbody和普通transform的方案
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.MovePosition(targetSpawnPoint.position);
                }
                else
                {
                    other.transform.position = targetSpawnPoint.position;
                }
            }
        }
    }
}

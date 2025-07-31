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
                // ���� CharacterController
                cc.enabled = false;
                other.transform.position = targetSpawnPoint.position;
                cc.enabled = true;
            }
            else
            {
                // �������������Rigidbody����ͨtransform�ķ���
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

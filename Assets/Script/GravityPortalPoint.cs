using UnityEngine;

public class GravityPortalPoint : MonoBehaviour
{
    public Transform targetSpawnPoint;
    public Vector3 targetEulerAngles;
    public Vector3 newGravity;

    private void OnTriggerEnter(Collider other)
    {
        // ֻҪ Portal ���ڼ���״̬���Ϳ��Դ��ͣ�Ҳ�����Լ��Ӳ����������Ƿ񼤻
        if (gameObject.activeSelf && other.CompareTag("Player"))
        {
            // �����߼����ֲ���...
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                other.transform.position = targetSpawnPoint.position;
                other.transform.rotation = Quaternion.Euler(targetEulerAngles);
                cc.enabled = true;
            }
            else
            {
                other.transform.position = targetSpawnPoint.position;
                other.transform.rotation = Quaternion.Euler(targetEulerAngles);
            }

            Camera cam = other.GetComponentInChildren<Camera>();
            if (cam != null)
                cam.transform.rotation = Quaternion.Euler(targetEulerAngles);

            Physics.gravity = newGravity;
            Debug.Log("�ѽ������������ţ�վ��ǽ��");

            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.customGravity = newGravity;
                Debug.Log("PlayerController�����������Ϊ��" + newGravity);
            }
        }
    }
}

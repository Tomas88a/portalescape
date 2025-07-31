using UnityEngine;

public class SimpleGravityTeleport : MonoBehaviour
{
    [Header("Ŀ��λ��")]
    public Transform targetPoint;           // ���ͺ����λ��
    [Header("���ͺ���ҳ���ŷ���ǣ�")]
    public Vector3 targetEulerAngles = Vector3.zero; // ���ͺ�����ӽ�
    [Header("�µ���������")]
    public Vector3 newGravity = new Vector3(0, -9.81f, 0); // Ĭ����ֱ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. �������
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                other.transform.position = targetPoint.position;
                other.transform.rotation = Quaternion.Euler(targetEulerAngles);
                cc.enabled = true;
            }
            else
            {
                other.transform.position = targetPoint.position;
                other.transform.rotation = Quaternion.Euler(targetEulerAngles);
            }

            // 2. ���ͬ����ת
            Camera cam = other.GetComponentInChildren<Camera>();
            if (cam != null)
                cam.transform.rotation = Quaternion.Euler(targetEulerAngles);

            // 3. �ı�ȫ������
            Physics.gravity = newGravity;
            Debug.Log("ֱ�Ӵ��Ͳ��ı�������");

            // 4. �����Զ����ɫ����������������
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.customGravity = newGravity;
                Debug.Log("PlayerController�����������Ϊ��" + newGravity);
            }
        }
    }
}

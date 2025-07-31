using UnityEngine;

public class GravityPortalUnlocker : MonoBehaviour
{
    public GameObject gravityPortalPoint; // ָ��������Ĵ��͵�
    private bool unlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!unlocked && other.CompareTag("Player"))
        {
            unlocked = true;
            Debug.Log("�������ͽ�����");
            if (gravityPortalPoint != null)
                gravityPortalPoint.SetActive(true);

            gameObject.SetActive(false); // ������ʧ�����
        }
    }
}

using UnityEngine;

public class PuzzleBall : MonoBehaviour
{
    public static bool solved = false;
    public GameObject portalTriggerToUnlock; // Inspector ��ָ��Ҫ����� Portal ����

    private void OnTriggerEnter(Collider other)
    {
        if (!solved && other.CompareTag("Player"))
        {
            solved = true;
            Debug.Log("�ѽ��գ��㴥������");

            // ����ͼ�����򣨱����ź��Portal��
            if (portalTriggerToUnlock != null)
                portalTriggerToUnlock.SetActive(true);

            // �ɼ���Ч����Ч�����������
            gameObject.SetActive(false); // ������ʧ
        }
    }
}

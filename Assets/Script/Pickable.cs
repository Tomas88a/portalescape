using UnityEngine;

public class Pickable : MonoBehaviour
{
    [Header("ʰȡ��ҵ��ĸ��������£���ѡ��")]
    public Transform holdParent; // �����Inspector�ֶ�ָ�������ýű��Զ�ָ��

    private bool isPicked = false;

    public void PickUp(Transform newParent)
    {
        if (isPicked) return;
        isPicked = true;

        if (newParent != null)
        {
            transform.SetParent(newParent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            var swing = GetComponent<HandItemSwing>();
            if (swing != null) swing.isHeld = true;
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log("��ʰȡ����Ʒ��" + name);
    }

    public void Drop()
    {
        if (!isPicked) return;
        isPicked = false;

        // �������
        transform.SetParent(null);

        // �ָ���������ײ��
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        // �Ӷ����ܽ���
        var swing = GetComponent<HandItemSwing>();
        if (swing != null) swing.isHeld = false;

        Debug.Log("�㶪������Ʒ��" + name);
    }

    // ��ѡ���жϵ�ǰ�Ƿ��ѱ�ʰȡ
    public bool IsPicked() => isPicked;
}

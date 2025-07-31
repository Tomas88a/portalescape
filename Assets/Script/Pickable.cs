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

            // �ؼ���ֻ�б�ʰȡ�������Ӷ�
            var swing = GetComponent<HandItemSwing>();
            if (swing != null) swing.isHeld = true;
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log("��ʰȡ����Ʒ��" + name);
    }
}

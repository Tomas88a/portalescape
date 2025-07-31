using UnityEngine;

public class Pickable : MonoBehaviour
{
    [Header("拾取后挂到哪个父物体下（可选）")]
    public Transform holdParent; // 你可以Inspector手动指定，或用脚本自动指定

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

            // 关键：只有被拾取后才允许挥动
            var swing = GetComponent<HandItemSwing>();
            if (swing != null) swing.isHeld = true;
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log("你拾取了物品：" + name);
    }
}

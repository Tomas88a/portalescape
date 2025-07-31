using UnityEngine;

public class GravityPortalUnlocker : MonoBehaviour
{
    public GameObject gravityPortalPoint; // 指定被激活的传送点
    private bool unlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!unlocked && other.CompareTag("Player"))
        {
            unlocked = true;
            Debug.Log("重力传送解锁！");
            if (gravityPortalPoint != null)
                gravityPortalPoint.SetActive(true);

            gameObject.SetActive(false); // 机关消失或禁用
        }
    }
}

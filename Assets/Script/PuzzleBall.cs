using UnityEngine;

public class PuzzleBall : MonoBehaviour
{
    public static bool solved = false;
    public GameObject portalTriggerToUnlock; // Inspector 可指定要激活的 Portal 区域

    private void OnTriggerEnter(Collider other)
    {
        if (!solved && other.CompareTag("Player"))
        {
            solved = true;
            Debug.Log("已解谜：你触碰了球！");

            // 激活传送检测区域（比如门后的Portal）
            if (portalTriggerToUnlock != null)
                portalTriggerToUnlock.SetActive(true);

            // 可加特效、音效、隐藏球体等
            gameObject.SetActive(false); // 让球消失
        }
    }
}

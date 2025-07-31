using UnityEngine;

public class GravityPortalPoint : MonoBehaviour
{
    public Transform targetSpawnPoint;
    public Vector3 targetEulerAngles;
    public Vector3 newGravity;

    private void OnTriggerEnter(Collider other)
    {
        // 只要 Portal 处于激活状态，就可以传送（也可以自己加布尔量控制是否激活）
        if (gameObject.activeSelf && other.CompareTag("Player"))
        {
            // 传送逻辑保持不变...
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
            Debug.Log("已进入重力传送门，站上墙！");

            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.customGravity = newGravity;
                Debug.Log("PlayerController重力方向更新为：" + newGravity);
            }
        }
    }
}

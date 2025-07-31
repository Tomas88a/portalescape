using UnityEngine;

public class SimpleGravityTeleport : MonoBehaviour
{
    [Header("目标位置")]
    public Transform targetPoint;           // 传送后的新位置
    [Header("传送后玩家朝向（欧拉角）")]
    public Vector3 targetEulerAngles = Vector3.zero; // 传送后玩家视角
    [Header("新的重力方向")]
    public Vector3 newGravity = new Vector3(0, -9.81f, 0); // 默认竖直向下

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. 传送玩家
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

            // 2. 相机同步旋转
            Camera cam = other.GetComponentInChildren<Camera>();
            if (cam != null)
                cam.transform.rotation = Quaternion.Euler(targetEulerAngles);

            // 3. 改变全局重力
            Physics.gravity = newGravity;
            Debug.Log("直接传送并改变重力！");

            // 4. 更新自定义角色控制器的重力方向
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.customGravity = newGravity;
                Debug.Log("PlayerController重力方向更新为：" + newGravity);
            }
        }
    }
}

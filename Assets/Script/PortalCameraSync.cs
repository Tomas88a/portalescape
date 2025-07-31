using UnityEngine;

public class PortalCameraSync : MonoBehaviour
{
    public Transform playerCamera;    // 玩家主摄像机
    public Transform portalIn;        // 入口门（比如A门）
    public Transform portalOut;       // 出口门（比如B门）

    void LateUpdate()
    {
        // 玩家相对于入口门的位置和朝向
        Vector3 localPos = portalIn.InverseTransformPoint(playerCamera.position);
        Vector3 localDir = portalIn.InverseTransformDirection(playerCamera.forward);

        // 应用到出口门空间
        transform.position = portalOut.TransformPoint(localPos);
        transform.forward = portalOut.TransformDirection(localDir);

        // 可选：如果想要更真实的FOV同步
        Camera portalCam = GetComponent<Camera>();
        Camera playerCam = playerCamera.GetComponent<Camera>();
        if (portalCam && playerCam)
        {
            portalCam.fieldOfView = playerCam.fieldOfView;
        }
    }
}

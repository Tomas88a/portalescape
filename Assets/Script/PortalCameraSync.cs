using UnityEngine;

public class PortalCameraSync : MonoBehaviour
{
    public Transform playerCamera;    // ����������
    public Transform portalIn;        // ����ţ�����A�ţ�
    public Transform portalOut;       // �����ţ�����B�ţ�

    void LateUpdate()
    {
        // ������������ŵ�λ�úͳ���
        Vector3 localPos = portalIn.InverseTransformPoint(playerCamera.position);
        Vector3 localDir = portalIn.InverseTransformDirection(playerCamera.forward);

        // Ӧ�õ������ſռ�
        transform.position = portalOut.TransformPoint(localPos);
        transform.forward = portalOut.TransformDirection(localDir);

        // ��ѡ�������Ҫ����ʵ��FOVͬ��
        Camera portalCam = GetComponent<Camera>();
        Camera playerCam = playerCamera.GetComponent<Camera>();
        if (portalCam && playerCam)
        {
            portalCam.fieldOfView = playerCam.fieldOfView;
        }
    }
}

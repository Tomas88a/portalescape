using UnityEngine;

public class ShowMouseHintOnProximity : MonoBehaviour
{
    public GameObject hintCanvas;      // ��WorldSpace Canvas����
    public float showDistance = 2f;    // ��ʾ��ʾ�ľ���

    private Camera mainCam;

    void Start()
    {
        if (hintCanvas != null)
            hintCanvas.SetActive(false);

        mainCam = Camera.main;
    }

    void Update()
    {
        if (mainCam == null) return;

        float dist = Vector3.Distance(transform.position, mainCam.transform.position);
        if (dist < showDistance)
        {
            if (hintCanvas != null && !hintCanvas.activeSelf)
                hintCanvas.SetActive(true);

            // ��Canvas��������ӽ�
            if (hintCanvas != null)
                hintCanvas.transform.LookAt(mainCam.transform, Vector3.up);
        }
        else
        {
            if (hintCanvas != null && hintCanvas.activeSelf)
                hintCanvas.SetActive(false);
        }
    }
}

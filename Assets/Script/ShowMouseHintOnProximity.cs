using UnityEngine;

public class ShowMouseHintOnProximity : MonoBehaviour
{
    public GameObject hintCanvas;      // 拖WorldSpace Canvas对象
    public float showDistance = 2f;    // 显示提示的距离

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

            // 让Canvas朝向玩家视角
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

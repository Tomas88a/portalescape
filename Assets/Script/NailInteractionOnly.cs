using UnityEngine;

public class NailInteractionOnly : MonoBehaviour
{
    [Header("∞Œ∂§◊”…Ë÷√")]
    public Camera mainCam;
    public float nailPullDistance = 2f;

    void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mainCam != null)
        {
            Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, nailPullDistance))
            {
                var limited = hit.collider.GetComponent<NailLimitedPull>();
                if (limited != null)
                {
                    limited.PullOnce();
                    return;
                }
                var nail = hit.collider.GetComponent<NailPullable>();
                if (nail != null)
                {
                    nail.PullOnce();
                    return;
                }
            }
        }
    }
}

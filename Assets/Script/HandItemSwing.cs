using UnityEngine;

public class HandItemSwing : MonoBehaviour
{
    public float swingAngle = 60f;
    public float swingSpeed = 10f;
    public AudioClip swingSound;

    [HideInInspector]
    public bool isHeld = false;

    private Quaternion originalRot;
    private bool swinging = false;
    private float t = 0;

    void Start()
    {
        originalRot = transform.localRotation;
    }

    void Update()
    {
        if (!isHeld) return;

        if (Input.GetMouseButtonDown(0) && !swinging)
        {
            swinging = true;
            t = 0;
            if (swingSound)
                AudioSource.PlayClipAtPoint(swingSound, transform.position);
        }

        if (swinging)
        {
            t += Time.deltaTime * swingSpeed;
            float angle = Mathf.Sin(t * Mathf.PI) * swingAngle;
            transform.localRotation = originalRot * Quaternion.Euler(angle, 0, 0);
            if (t >= 1)
            {
                swinging = false;
                transform.localRotation = originalRot;
            }
        }
    }
}

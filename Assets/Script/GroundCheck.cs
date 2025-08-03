using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public NewBehaviourScript player;
    public AudioClip landClip; // �����Ч�����

    private bool wasOnGround = false;

    private void Start()
    {
        // player = transform.parent.GetComponent<NewBehaviourScript>();
        if (player != null)
        {
            player.landClip = landClip;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            player.canJump = true;

            // ���ʱ���������Ч
            if (player.audioSource != null && landClip != null)
            {
                player.audioSource.PlayOneShot(landClip, 1f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            player.canJump = false;
        }
    }
}

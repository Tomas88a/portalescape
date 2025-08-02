using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public NewBehaviourScript player;
    public AudioClip landClip; // 落地音效拖这里！

    private bool wasOnGround = false;

    private void Start()
    {
        player = transform.parent.GetComponent<NewBehaviourScript>();
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

            // 落地时播放落地音效
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

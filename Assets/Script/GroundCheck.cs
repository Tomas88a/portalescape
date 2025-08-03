using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public NewBehaviourScript player;
    public AudioClip landClip;
    public LayerMask groundLayerMask = -1; // 地面层级遮罩
    public float checkRadius = 0.5f; // 检测球体半径

    private bool wasOnGround = false;

    private void Start()
    {
        if (player != null)
        {
            player.landClip = landClip;
        }
    }

    private void Update()
    {
        CheckGroundWithOverlap();
    }

    private void CheckGroundWithOverlap()
    {
        // 使用球形重叠检测
        Collider[] groundColliders = Physics.OverlapSphere(transform.position, checkRadius, groundLayerMask);
        
        bool isOnGround = false;
        foreach (Collider col in groundColliders)
        {
            if (col.CompareTag("Ground"))
            {
                isOnGround = true;
                break;
            }
        }

        // 检测着陆
        if (isOnGround && !wasOnGround)
        {
            player.canJump = true;
            if (player.audioSource != null && landClip != null)
            {
                player.audioSource.PlayOneShot(landClip, 1f);
            }
        }
        // 检测离开地面
        else if (!isOnGround && wasOnGround)
        {
            player.canJump = false;
        }

        wasOnGround = isOnGround;
    }

    // 在Scene视图中显示检测范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = wasOnGround ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

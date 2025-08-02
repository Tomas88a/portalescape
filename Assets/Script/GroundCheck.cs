using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public NewBehaviourScript player;
    private void Start()
    {
        player = transform.parent.GetComponent<NewBehaviourScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
            player.canJump = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
            player.canJump = false;
    }
}

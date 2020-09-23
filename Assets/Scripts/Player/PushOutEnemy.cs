using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushOutEnemy : MonoBehaviour
{
    private PlayerMovementController playerMovement;
    
    void Start()
    {
        playerMovement = this.transform.root.gameObject.GetComponent<PlayerMovementController>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (playerMovement.isGrounded && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            
        }
    }
}

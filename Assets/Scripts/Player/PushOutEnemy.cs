using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushOutEnemy : MonoBehaviour
{
    private PlayerMovementController playerMovement;
    private Collider2D enemyPushOutTrigger;
    private Collider2D enemyPushOutBox;
    void Start()
    {
        playerMovement = this.transform.root.gameObject.GetComponent<PlayerMovementController>();
        Component[] pushboxes = GetComponents(typeof(Collider2D));
        enemyPushOutTrigger = (Collider2D) pushboxes[0];
        enemyPushOutBox = (Collider2D) pushboxes[1];
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (playerMovement.isGrounded && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("WOW!");
            enemyPushOutBox.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("OWO!");
            enemyPushOutBox.enabled = false;
        }
    }
}

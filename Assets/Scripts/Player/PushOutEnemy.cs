using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PushOutEnemy : MonoBehaviour
{
    // private PlayerMovementController playerMovement;
    private Transform selfTransform;
    // private Rigidbody2D rb2d;
    private Collider2D pushOutTrigger;

    void Start()
    {
        // playerMovement = this.transform.root.gameObject.GetComponent<PlayerMovementController>();
        selfTransform = this.transform.root.gameObject.transform;
        // rb2d = this.transform.root.gameObject.GetComponent<Rigidbody2D>();
        pushOutTrigger = GetComponent<Collider2D>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pushbox") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // repel pushboxes fast
            Debug.Log("PushOut");
            // rb2d.AddForce(new Vector2(-2.0f, 0f), ForceMode2D.Impulse);
            selfTransform.Translate(CalculatePushOutVector(pushOutTrigger, other));
        }
    }
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         // stop repel pushboxes fast
    //     }
    // }
    private Vector3 CalculatePushOutVector(Collider2D self, Collider2D other) {
        Vector3 selfPosition = self.transform.position;
        Vector3 otherPosition = other.transform.position;
        // Debug.Log("self: " + selfPosition.x);
        // Debug.Log("other: " + otherPosition.x);
        Debug.Log("self - other: " + (selfPosition.x - otherPosition.x));
        Vector2 selfBoxSize = ((BoxCollider2D) self).size;
        Vector2 otherBoxSize = ((BoxCollider2D) other).size;

        float boxXDiff = otherPosition.x - selfPosition.x;
        if (boxXDiff >= 0) {
            float halfSelfBoxLen = (selfBoxSize.x / 2.0f) * Math.Abs(self.transform.root.transform.localScale.x);
            float halfOtherBoxLen = (otherBoxSize.x / 2.0f) * Math.Abs(other.transform.root.transform.localScale.x);
            float pushBoxOverlap = (boxXDiff) - ((boxXDiff - halfOtherBoxLen) + (boxXDiff - halfSelfBoxLen));
            Debug.Log(-pushBoxOverlap);
            return new Vector3 (-pushBoxOverlap, 0f, 0f);
        } else {
            boxXDiff = selfPosition.x - otherPosition.x;
            float halfSelfBoxLen = (selfBoxSize.x / 2.0f) * Math.Abs(self.transform.root.transform.localScale.x);
            float halfOtherBoxLen = (otherBoxSize.x / 2.0f) * Math.Abs(other.transform.root.transform.localScale.x);
            float pushBoxOverlap = (boxXDiff) - ((boxXDiff - halfOtherBoxLen) + (boxXDiff - halfSelfBoxLen));
            Debug.Log(pushBoxOverlap);
            return new Vector3 (pushBoxOverlap, 0f, 0f);
        }
    }
}

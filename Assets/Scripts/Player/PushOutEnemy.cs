using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PushOutEnemy : MonoBehaviour
{
    private Transform selfTransform;
    private Collider2D pushOutTrigger;

    void Start()
    {
        selfTransform = this.transform.root.gameObject.transform;
        pushOutTrigger = GetComponent<Collider2D>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pushbox"))
        {
            // repel pushboxes fast
            Vector3 pushVector = CalculatePushOutVector(pushOutTrigger, other);
            selfTransform.Translate(pushVector);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            Vector3 pushVector = CalculatePushOutVector(pushOutTrigger, other);
            selfTransform.Translate(pushVector * 2);
        }
    }

    private Vector3 CalculatePushOutVector(Collider2D self, Collider2D other) {
        Vector3 selfPosition = self.transform.position;
        Vector3 otherPosition = other.transform.position;
        Vector2 selfBoxSize = ((BoxCollider2D) self).size;
        Vector2 otherBoxSize = ((BoxCollider2D) other).size;

        float boxXDiff = otherPosition.x - selfPosition.x;
        float halfSelfBoxLen = (selfBoxSize.x / 2.0f) * Math.Abs(self.transform.root.transform.localScale.x);
        float halfOtherBoxLen = (otherBoxSize.x / 2.0f) * Math.Abs(other.transform.root.transform.localScale.x);
        float pushBoxOverlap = 0f;
        if (boxXDiff >= 0) {
            pushBoxOverlap = (boxXDiff) - ((boxXDiff - halfOtherBoxLen) + (boxXDiff - halfSelfBoxLen));
        } else {
            pushBoxOverlap = (boxXDiff) - ((boxXDiff + halfOtherBoxLen) + (boxXDiff + halfSelfBoxLen));
        }
        return new Vector3 (-pushBoxOverlap / 2.0f, 0f, 0f);
    }
}

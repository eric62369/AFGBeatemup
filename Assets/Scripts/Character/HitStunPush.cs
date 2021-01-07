using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStunPush : MonoBehaviour
{
    IMovementController self;

    void Start() {
        self = GetComponent<IMovementController>();
    }

    private void Pushback() {
        self.Pushback(new Vector2(30f, 0f));
    }
}

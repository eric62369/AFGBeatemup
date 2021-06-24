using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStunPush : MonoBehaviour
{
    IMovementController self;

    void Start() {
        self = GetComponent<IMovementController>();
        // ParentHurtbox parentHurtbox = GetComponent<ParentHurtbox>();
        IAttackController selfAttackSender = GetComponent<IAttackController>();
        selfAttackSender.SendHitEvent += PushbackSendHit;
        self.GetHitEvent += PushbackGetHit;
    }

    private void PushbackSendHit(object sender, SendHitEventArgs e) {
        int pushback = e.attackData.GetPushback();
        int direction = e.attackData.GetPushBackDirection(e.victimXPosition);
        Vector2 pushbackVector = new Vector2 (-pushback * direction, 0f);
        if (!self.isGrounded) {
            pushbackVector = new Vector2 (-pushback * direction / 2.0f, 0f);
        }
        Debug.Log("Send Hit");
        // self.Pushback(pushbackVector);
        // TODO: No pushback currently
    }

    private void PushbackGetHit(object sender, GetHitEventArgs e) {
        Debug.Log("Got Hit");
        int pushback = e.attackData.GetPushback();
        int direction = e.attackData.GetPushBackDirection(self.xPosition);
        Vector2 pushbackVector = new Vector2 (pushback * direction * 10f, 0f);
        if (!self.isGrounded) {
            pushbackVector = new Vector2 (pushback * direction / 2.0f, 0f);
        }
        self.Pushback(pushbackVector);
    }
}

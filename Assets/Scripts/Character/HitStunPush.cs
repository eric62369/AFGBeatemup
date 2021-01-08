using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStunPush : MonoBehaviour
{
    IMovementController self;

    void Start() {
        self = GetComponent<IMovementController>();
        // ParentHurtbox parentHurtbox = GetComponent<ParentHurtbox>();
        PlayerAttackController selfAttackSender = GetComponent<PlayerAttackController>();
        if (selfAttackSender != null) {
            selfAttackSender.SendHitEvent += PushbackSendHit;
        }
        self.GetHitEvent += PushbackGetHit;
    }

    private void PushbackSendHit(object sender, SendHitEventArgs e) {
        int pushback = e.attackData.GetPushback();
        int direction = e.attackData.GetPushBackDirection();
        self.Pushback(new Vector2(-pushback * direction, 0f));
    }

    private void PushbackGetHit(object sender, GetHitEventArgs e) {
        int pushback = e.attackData.GetPushback();
        int direction = e.attackData.GetPushBackDirection();
        self.Pushback(new Vector2(pushback * direction, 0f));
    }
}

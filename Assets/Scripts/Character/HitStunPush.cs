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
        // self.Pushback(pushbackVector);
        // TODO: No pushback currently
    }

    private void PushbackGetHit(object sender, GetHitEventArgs e) {
        int pushback = e.attackData.GetPushback();
        int direction = e.attackData.GetPushBackDirection(self.xPosition);
        if (e.attackData.Type == AttackType.Launcher)
        {
            // Launch enemy uP!
            GetLaunched(e.attackData);
        }
        else if (e.attackData.Type == AttackType.Dunk) {
            // Launch enemy Down!
            GetDunked(e.attackData);
        }
        else if (e.attackData.Type == AttackType.HeavyLauncher) {
            GetHeavyLaunched(e.attackData);
        }
        else
        {
            Vector2 pushbackVector = new Vector2 (pushback * direction, 0f);
            if (!self.isGrounded) {
                pushbackVector = new Vector2 (pushback * direction / 2.0f, 0f);
            }
            self.Pushback(pushbackVector);


            // normal attack
            if (!self.isGrounded) {
                GetLaunched(e.attackData);
            } else {
                // TODO: See if the overlap bug is caused by this
                // rb2d.AddForce(new Vector2(
                //     attackData.GetPushback() * attackData.GetPushBackDirection(), 0),
                //     ForceMode2D.Force);
            }
        }
    }

    private void GetLaunched(Attack attackData)
    {
        self.Launch(attackData.GetPushBackDirection(self.xPosition));
    }

    private void GetHeavyLaunched(Attack attackData)
    {
        self.HeavyLaunch(attackData.GetPushBackDirection(self.xPosition));
    }

    private void GetDunked(Attack attackData)
    {
        self.Dunk(attackData.GetPushBackDirection(self.xPosition));
    }
}

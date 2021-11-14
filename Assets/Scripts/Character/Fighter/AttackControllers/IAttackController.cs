using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SendHitEventArgs
{
    public Attack attackData { get; private set; }

    public float victimXPosition { get; private set; }

    public SendHitEventArgs(Attack attackData_, float victimXPosition_) {
        attackData = attackData_;
        victimXPosition = victimXPosition_;
    }
}

public delegate void SendHit(object sender, SendHitEventArgs args);

public interface IAttackController
{
    event SendHit SendHitEvent;
    void TriggerHitStop(Attack AttackData, float victimXPosition);
    
}

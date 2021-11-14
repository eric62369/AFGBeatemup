using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public enum CancelAction
{
    Jump,
    Attack
}

/// <summary>
/// Hub for all combat related resources
/// </summary>
public class UniversalAttackController : IAttackController {
    // Should be a universal feature: No fighting entity goes without hitstop
    private HitStopTracker InternalHitStopTracker;
    public event SendHit SendHitEvent;

    // Must always be called before Recovery frames
    public void TriggerHitStop(Attack AttackData, float victimXPosition)
    {
        RaiseSendHitEvent(new SendHitEventArgs(AttackData, victimXPosition));
    }

    protected virtual void RaiseSendHitEvent(SendHitEventArgs e) {
        SendHit raiseEvent = SendHitEvent;

        if (raiseEvent != null) {
            raiseEvent(this, e);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EnemyAttackController : MonoBehaviour, IAttackController
{
    IMovementController movementController;
    private CharacterAnimationController animator;

    public event SendHit SendHitEvent;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<IMovementController>();
        animator = GetComponent<CharacterAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async Task TriggerHitStop(Attack AttackData, float victimXPosition)
    {
        Vector2 oldVelocity = FreezePlayer();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        // await Task.Delay(AttackData.GetHitStop());
        // TODO: Raise SendHit event
        UnFreezePlayer(oldVelocity);
        RaiseSendHitEvent(new SendHitEventArgs(AttackData, victimXPosition));
    }
    protected virtual void RaiseSendHitEvent(SendHitEventArgs e) {
        SendHit raiseEvent = SendHitEvent;

        if (raiseEvent != null) {
            raiseEvent(this, e);
        }
    }


    public Vector2 FreezePlayer()
    {
        animator.AnimatorEnable(false);
        return movementController.FreezeCharacter();
    }
    public void UnFreezePlayer(Vector2 oldVelocity)
    {
        animator.AnimatorEnable(true);
        movementController.UnFreezeCharacter(oldVelocity);
    }
}

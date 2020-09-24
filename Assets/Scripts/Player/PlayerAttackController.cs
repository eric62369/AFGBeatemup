using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public enum CancelAction
{
    Jump,
    Attack
}

public class PlayerAttackController : MonoBehaviour {
    
    public bool isAttacking { get; private set; }

    private PlayerMovementController movementController;
    private PlayerStateManager playerState;
    private CancelAction? currentCancelAction;
    void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        playerState = GetComponent<PlayerStateManager>();
        isAttacking = false;
        currentCancelAction = null;
    }
    
    public void GroundedAttackFlags(string attackName)
    {
        if (movementController.isGrounded)
        {
            if (isAttacking)
            {
                // Player is already attacking, is a cancel possible?
                if (movementController.AnimationGetBool("CanCancel"))
                {
                    movementController.AnimationSetBool(attackName, true);
                }
            }
            else
            {
                isAttacking = true;
                movementController.isRunning = false;
                movementController.AnimationSetBool(attackName, true);
                movementController.AnimationSetBool("IsRunning", false);
                movementController.StopRun();
            }
        }
    }
    public void Attack5B()
    {
        GroundedAttackFlags("5B");
    }
    public void Attack5C()
    {
        GroundedAttackFlags("5C");
    }
    public void Throw(bool isForward)
    {
        if (movementController.isGrounded)
        {
            if (!isAttacking)
            {
                isAttacking = true; // TODO: Do we need a throw flag?
                playerState.SetThrowDirection(isForward);
                movementController.isRunning = false;
                movementController.AnimationSetBool("ThrowWhiff", true);
                movementController.AnimationSetBool("IsRunning", false);
                movementController.StopRun();
            }
        }
    }
    public void ThrowFreeze()
    {
        movementController.rb2d.velocity = new Vector2(0f, 0f);
        movementController.rb2d.bodyType = RigidbodyType2D.Kinematic;
    }
    public void ThrowUnFreeze()
    {
        movementController.rb2d.bodyType = RigidbodyType2D.Dynamic;
        MoveDone();
    }

    public void Startup()
    {
        ResetCancel();
    }
    public void Active() {
        // Activate hitbox and hurtbox
    }
    public void Recovery() {
        // Deactivate hitbox
        ResetCancel();
    }
    public void MoveDone() {
        // Deactivate hurtbox
        ResetAttackStateToNeutral();
    }
    public void Cancel() {
        ResetAttackStateToNeutral();
    }
    public void SetCancel()
    {
        movementController.AnimationSetBool("CanCancel", true);
    }
    public void ResetCancel()
    {
        movementController.AnimationSetBool("CanCancel", false);
    }

    public Vector2 FreezePlayer()
    {
        movementController.animator.enabled=false;
        movementController.rb2d.bodyType = RigidbodyType2D.Kinematic;
        Vector2 oldVelocity = movementController.rb2d.velocity;
        movementController.rb2d.velocity = new Vector2(0f, 0f);
        return oldVelocity;
    }

    public void UnFreezePlayer(Vector2 oldVelocity)
    {
        movementController.animator.enabled=true;
        movementController.rb2d.bodyType = RigidbodyType2D.Dynamic;
        movementController.rb2d.velocity = oldVelocity;
    }

    // Must always be called before Recovery frames
    public async Task TriggerHitStop(Attack AttackData)
    {
        SetCancel();
        Vector2 oldVelocity = FreezePlayer();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(AttackData.GetHitStop());
        UnFreezePlayer(oldVelocity);
        UseCancelAction();
    }

    public void SetCancelAction(CancelAction action)
    {
        // OnHit / OnBlock cancels
        if (movementController.AnimationGetBool("CanCancel"))
        {
            currentCancelAction = action;
        }
        else
        {
            currentCancelAction = null;
        }
    }
    private void UseCancelAction()
    {
        if (currentCancelAction != null)
        {
            Debug.Log(currentCancelAction);
            playerState.UseCancelAction(currentCancelAction);
        }
        currentCancelAction = null;
    }

    private void ResetAttackStateToNeutral()
    {
        movementController.AnimationSetBool("5B", false);
        movementController.AnimationSetBool("5C", false);
        movementController.AnimationSetBool("ThrowWhiff", false);
        movementController.AnimationSetBool("ThrowHit", false);
        isAttacking = false;
    }
}
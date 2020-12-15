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
    public RedAttackProperties attackProperties;
    private PlayerMovementController movementController;
    private PlayerStateManager playerState;
    private CancelAction? currentCancelAction;
    private string currentActiveAttack;

    private PlayerAnimationController animator;

    void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        playerState = GetComponent<PlayerStateManager>();
        animator = GetComponent<PlayerAnimationController>();
        isAttacking = false;
        currentCancelAction = null;
        currentActiveAttack = null;
    }
    
    public void GroundedAttackFlags(string attackName)
    {
        if (movementController.isGrounded && !animator.AnimationGetBool("IsJumping"))
        {
            if (isAttacking)
            {
                // Player is already attacking, is a cancel possible?
                if (animator.AnimationGetBool("CanCancel"))
                {
                    animator.AnimationSetBool(attackName, true);
                    currentActiveAttack = attackName;
                }
            }
            else
            {
                isAttacking = true;
                movementController.isRunning = false;
                animator.AnimationSetBool(attackName, true);
                currentActiveAttack = attackName;
                animator.AnimationSetBool("IsRunning", false);
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
                animator.AnimationSetBool("ThrowWhiff", true);
                animator.AnimationSetBool("IsRunning", false);
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
        SoundManagerController.playSFX(SoundManagerController.whiffLvl1Sound);
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
        animator.AnimationSetBool("CanCancel", true);
    }
    public void ResetCancel()
    {
        animator.AnimationSetBool("CanCancel", false);
    }

    public Vector2 FreezePlayer()
    {
        animator.AnimatorEnable(false);
        movementController.rb2d.bodyType = RigidbodyType2D.Kinematic;
        Vector2 oldVelocity = movementController.rb2d.velocity;
        movementController.rb2d.velocity = new Vector2(0f, 0f);
        return oldVelocity;
    }

    public void UnFreezePlayer(Vector2 oldVelocity)
    {
        animator.AnimatorEnable(true);
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
        if (animator.AnimationGetBool("CanCancel"))
        {
            if (action == CancelAction.Jump && 
                !attackProperties.CanJumpCancel(currentActiveAttack))
            {
                currentCancelAction = null;
            }
            else
            {
                // valid cancel action
                currentCancelAction = action;
                // If no hitstop present, use cancel action now!
                if (animator.GetAnimatorEnable())
                {
                    UseCancelAction();
                }
            }
        }
        else
        {
            // Cannot cancel right now
            currentCancelAction = null;
        }
    }
    private void UseCancelAction()
    {
        if (currentCancelAction != null)
        {
            animator.AnimationSetTrigger("ExecutingCancel");
            playerState.UseCancelAction(currentCancelAction);
            currentCancelAction = null;
        }
    }

    public void ResetAttackStateToNeutral()
    {
        animator.AnimationSetBool("5B", false);
        animator.AnimationSetBool("5C", false);
        animator.AnimationSetBool("ThrowWhiff", false);
        animator.AnimationSetBool("ThrowHit", false);
        currentActiveAttack = null;
        isAttacking = false;
    }
}
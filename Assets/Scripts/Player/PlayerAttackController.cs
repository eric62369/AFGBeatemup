using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class PlayerAttackController : MonoBehaviour {
    
    public bool isAttacking { get; private set; }

    private PlayerMovementController player;
    void Start()
    {
        player = GetComponent<PlayerMovementController>();
        isAttacking = false;
    }
    
    public void GroundedAttackFlags(string attackName)
    {
        if (player.isGrounded)
        {
            if (isAttacking)
            {
                // Player is already attacking, is a cancel possible?
                if (player.AnimationGetBool("CanCancel"))
                {
                    player.AnimationSetBool(attackName, true);
                }
            }
            else
            {
                isAttacking = true;
                player.isRunning = false;
                player.AnimationSetBool(attackName, true);
                player.AnimationSetBool("IsRunning", false);
                player.StopRun();
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
        player.AnimationSetBool("5B", false);
        player.AnimationSetBool("5C", false);
        isAttacking = false;
    }
    public void Cancel() {
        player.AnimationSetBool("5B", false);
        player.AnimationSetBool("5C", false);
        isAttacking = false;
    }
    public void SetCancel()
    {
        player.AnimationSetBool("CanCancel", true);
    }
    public void ResetCancel()
    {
        player.AnimationSetBool("CanCancel", false);
    }

    // Must always be called before Recovery frames
    public async Task TriggerHitStop(Attack AttackData)
    {
        SetCancel();
        player.animator.enabled=false;
        player.rb2d.bodyType = RigidbodyType2D.Kinematic;
        Vector2 oldVelocity = player.rb2d.velocity;
        player.rb2d.velocity = new Vector2(0f, 0f);
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(AttackData.GetHitStop());
        player.animator.enabled=true;
        player.rb2d.bodyType = RigidbodyType2D.Dynamic;
        player.rb2d.velocity = oldVelocity;
    }
}
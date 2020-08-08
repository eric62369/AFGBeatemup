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
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            player.AnimationSetBool("5B", true);
            isAttacking = true;
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            player.AnimationSetBool("5C", true);
            isAttacking = true;
        }
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
        Debug.Log("YEET");
        await Task.Delay(AttackData.GetHitStop());
        player.animator.enabled=true;
        player.rb2d.bodyType = RigidbodyType2D.Dynamic;
        player.rb2d.velocity = oldVelocity;
    }
}
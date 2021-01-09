using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EnemyMovementController : MonoBehaviour, IMovementController
{
    private Rigidbody2D rb2d;
    public Animator animator;

    private EnemyStateManager enemyState;

    public event GetHit GetHitEvent;

    public bool isGrounded { get; private set; }

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyState = GetComponent<EnemyStateManager>();
    }

    public void Pushback(Vector2 force) {
        rb2d.AddForce(force, ForceMode2D.Impulse);
    }

    public async Task TriggerHitStun(Attack attackData)
    {
        // Trigger animation's hitstun 
        FreezeCharacter();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(attackData.GetHitStop());
        UnFreezeCharacter();
        if (attackData.Type == AttackType.Launcher)
        {
            // Launch enemy uP!
            enemyState.GetLaunched(attackData);
        }
        else
        {
            // normal attack
            if (!enemyState.isGrounded) {
                enemyState.GetLaunched(attackData);
            } else {
                // rb2d.AddForce(new Vector2(pushback * direction, 0), ForceMode2D.Force);
            }
        }
        RaiseGetHitEvent(new GetHitEventArgs(attackData));
    }

    protected virtual void RaiseGetHitEvent(GetHitEventArgs e) {
        GetHit raiseEvent = GetHitEvent;

        if (raiseEvent != null) {
            raiseEvent(this, e);
        }
    }

    /// Returns the velocity before freezing
    public Vector2 FreezeCharacter()
    {
        animator.enabled=false;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        Vector2 oldVelocity = rb2d.velocity;
        rb2d.velocity = new Vector2(0f, 0f);
        return oldVelocity;
    }

    public void UnFreezeCharacter(Vector2 oldVelocity)
    {
        animator.enabled=true;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.velocity = oldVelocity;
    }
    public void UnFreezeCharacter()
    {
        animator.enabled=true;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.velocity = new Vector2(0f, 0f);
    }

    public void LaunchEnemy(int direction)
    {
        rb2d.velocity = new Vector2(
            AttackConstants.LightLaunchForce[0] * direction,
            AttackConstants.LightLaunchForce[1]);
    }

    // Must always be called before Recovery frames
    public async Task TriggerHitStop(Attack attackData)
    {
        // Get animator
        // Pause animator for x seconds
        Vector2 oldVelocity = FreezeCharacter();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(attackData.GetHitStop());
        UnFreezeCharacter(oldVelocity);
        // resume animation
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EnemyMovementController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public Animator animator;

    private EnemyStateManager enemyState;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyState = GetComponent<EnemyStateManager>();
    }

    /// Returns the velocity before freezing
    public Vector2 FreezeEnemy()
    {
        animator.enabled=false;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        Vector2 oldVelocity = rb2d.velocity;
        rb2d.velocity = new Vector2(0f, 0f);
        return oldVelocity;
    }

    public void UnFreezeEnemy(Vector2 oldVelocity)
    {
        animator.enabled=true;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.velocity = oldVelocity;
    }
    public void UnFreezeEnemy()
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
        Vector2 oldVelocity = FreezeEnemy();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(attackData.GetHitStop());
        UnFreezeEnemy(oldVelocity);
        // resume animation
    }

    public async Task TriggerHitStun(Attack attackData)
    {
        // Trigger animation's hitstun 
        FreezeEnemy();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(attackData.GetHitStop());
        UnFreezeEnemy();
        int pushback = attackData.GetPushback();
        int direction = attackData.GetPushBackDirection();
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
                rb2d.AddForce(new Vector2(pushback * direction, 0), ForceMode2D.Force);
            }
        }
    }
}

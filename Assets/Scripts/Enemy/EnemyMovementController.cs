using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EnemyMovementController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public Animator animator;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    // Must always be called before Recovery frames
    public async Task TriggerHitStop(Attack AttackData)
    {
        // Get animator
        // Pause animator for x seconds
        Vector2 oldVelocity = FreezeEnemy();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(AttackData.GetHitStop());
        UnFreezeEnemy(oldVelocity);
        // resume animation
    }

    public async Task TriggerHitStun(Attack AttackData)
    {
        // Trigger animation's hitstun 
        FreezeEnemy();
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(AttackData.GetHitstun());
        UnFreezeEnemy();
        int pushback = AttackData.GetPushback();
        int direction = AttackData.GetPushBackDirection();
        rb2d.AddForce(new Vector2(pushback * direction, 0), ForceMode2D.Force);
    }
}

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

    // Must always be called before Recovery frames
    public async Task TriggerHitStop(Attack AttackData)
    {
        // Get animator
        // Pause animator for x seconds
        animator.enabled=false;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        Vector2 oldVelocity = rb2d.velocity;
        rb2d.velocity = new Vector2(0f, 0f);
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(AttackData.GetHitStop());
        animator.enabled=true;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.velocity = oldVelocity;
        // resume animation
    }

    public async Task TriggerHitStun(Attack AttackData)
    {
        // Trigger animation's hitstun 
        animator.enabled=false;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        rb2d.velocity = new Vector2(0f, 0f);
        // TODO: Do we need to be able to interrupt hitstop? Probably
        await Task.Delay(AttackData.GetHitstun());
        animator.enabled=true;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        int pushback = AttackData.GetPushback();
        int direction = AttackData.GetPushBackDirection();
        rb2d.AddForce(new Vector2(pushback * direction, 0), ForceMode2D.Force);
    }
}

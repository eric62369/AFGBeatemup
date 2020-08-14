using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;

public class PlayerMovementController : MonoBehaviour {

    public float WalkSpeed;
    public float InitialDashSpeed;
    public float HorizontalJumpSpeed;
    public float JumpForce;
    public float RunForce;
    public float MaxRunSpeed;
    public bool isRunning;
    public int MaxAirActions;
    public int AirActionsLeft;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    public bool isGrounded;
    public bool isHoldingJump;

    public Rigidbody2D rb2d;
    public Animator animator;
    private PlayerAttackController attackController;
    private PlayerInputManager playerInput;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<PlayerAttackController>();
        playerInput = GetComponent<PlayerInputManager>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        bool newGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
        if (newGrounded != isGrounded && newGrounded == true)
        {
            // Landed!
            animator.SetBool("IsJumping", false);
            isHoldingJump = false;
            AirActionsLeft = MaxAirActions;
        }
        isGrounded = newGrounded;
    }

    public void Walk(Numpad direction)
    {
        if (direction != Numpad.N4 && direction != Numpad.N6)
        {
            throw new ArgumentException(direction + " is not a horizontal direction");
        }
        int walkDirection = 0;
        if (direction == Numpad.N6) {
            walkDirection = 1;
        }
        else if (direction == Numpad.N4)
        {
            walkDirection = -1;
        }
        if (isGrounded && !attackController.isAttacking) {
            rb2d.velocity = new Vector2(walkDirection * WalkSpeed, 0f);
        }
    }
    public void Dash(Numpad direction)
    {
        if (direction != Numpad.N6)
        {
            throw new ArgumentException(direction + " is not the forward direction");
        }
        if (isGrounded && !attackController.isAttacking) {
            rb2d.velocity = new Vector2(InitialDashSpeed, 0f);
        }
        isRunning = true;
        animator.SetBool("IsRunning", true);
    }
    public void Run(Numpad direction)
    {
        if (direction != Numpad.N6)
        {
            throw new ArgumentException(direction + " is not a horizontal direction");
        }
        if (isGrounded && !attackController.isAttacking) {
            rb2d.AddForce(new Vector2(RunForce, 0f));
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            if (rb2d.velocity.x > MaxRunSpeed)
            {
                Debug.Log("Max Speed!");
                rb2d.velocity = new Vector2(MaxRunSpeed, 0f);
            }
        }
        isRunning = true;
    }
    public void StopRun()
    {
        isRunning = false;
        animator.SetBool("IsSkidding", false);
    }
    public void Skid()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsSkidding", true);
    }

    public void Jump(Numpad direction)
    {
        if (direction != Numpad.N7 && direction != Numpad.N8 && direction != Numpad.N9)
        {
            throw new ArgumentException(direction + " is not an upwards direction");
        }
        // TODO: can you fix this later?
        bool canJump = (isGrounded && !attackController.isAttacking && !isHoldingJump) ||
            (!isGrounded && !attackController.isAttacking && !isHoldingJump && AirActionsLeft > 0);
        if (canJump) {
            float horizontalVelocity = 0;
            if (isRunning) 
            {
                // Slight dash momentum factored in
                horizontalVelocity = rb2d.velocity.x;
            }
            if (direction == Numpad.N7)
            {
                // Dash momentum not factored in
                horizontalVelocity = -HorizontalJumpSpeed;
            }
            else if (direction == Numpad.N9)
            {
                horizontalVelocity += HorizontalJumpSpeed;
            }
            rb2d.velocity = new Vector2(horizontalVelocity, 0f);
            rb2d.AddForce(new Vector2(0f, JumpForce));
            isRunning = false;
            setIsHoldingJump(true);
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsRunning", false);
            AirActionsLeft--;
        }
    }

    public void setIsHoldingJump(bool state)
    {
        isHoldingJump = state;
    }

    public Vector2 GetVelocity()
    {
        return rb2d.velocity;
    }
    public void SetVelocity(Vector2 movement)
    {
        rb2d.velocity = new Vector2(0f, 0f);
    }
    public void AnimationSetBool(string animationId, bool setValue)
    {
        animator.SetBool(animationId, setValue);
    }
}
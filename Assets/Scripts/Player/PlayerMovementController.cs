using UnityEngine;
using System;
using System.Collections;
using BattleInput;

public class PlayerMovementController : MonoBehaviour {

    public float WalkSpeed;
    public float InitialDashSpeed;
    public float AirDashSpeed;
    public float ForwardAirDashDuration;
    public float BackwardAirDashDuration;
    public float BackDashDuration;
    public float BackDashBackSpeed;
    public float BackDashUpSpeed;
    public float HorizontalJumpSpeed;
    public float JumpForce;
    public float RunForce;
    public float MaxRunSpeed;
    public float SkidSpeed;
    public bool isRunning;
    // 4 for back, 6 for forward, 5 for no walk
    public Numpad IsWalking;
    public bool isAirDashing;
    public bool isBackDashing;
    public int MaxAirActions;
    private int AirActionsLeft;
    public float GravityScale;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    public bool isGrounded { get; private set; }
    public bool isHoldingJump { get; private set; }
    private Numpad PrevJumpInput;
    private bool hasDashMomentum;

    private Rigidbody2D rb2d;
    
    private PlayerAttackController attackController;
    private PlayerInputManager playerInput;
    private PlayerStateManager playerState;

    private PlayerAnimationController animator;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        attackController = GetComponent<PlayerAttackController>();
        playerInput = GetComponent<PlayerInputManager>();
        playerState = GetComponent<PlayerStateManager>();
        rb2d.gravityScale = GravityScale;
        animator = GetComponent<PlayerAnimationController>();
        IsWalking = Numpad.N5;
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (!isAirDashing)
        {
            bool newGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);
            if (newGrounded != isGrounded && newGrounded == true)
            {
                // Landed!
                animator.AnimationSetBool("IsJumping", false);
                hasDashMomentum = false;
                AirActionsLeft = MaxAirActions;
                if (isHoldingJump) {
                    isHoldingJump = false;
                    Jump(PrevJumpInput);
                }
            }
            isGrounded = newGrounded;

            bool isIdling = isGrounded && !attackController.isAttacking && !isBackDashing;
            if (isIdling)
            {
                UpdateFacingDirection();
            }
            WalkUpdate();
            Run(Numpad.N6);
        }
    }

    public void ResetMovementStateToNeutral()
    {
        isRunning = false;
        isAirDashing = false;
        isBackDashing = false;
        animator.AnimationSetBool("IsJumping", false);
        animator.AnimationSetBool("IsRunning", false);
        animator.AnimationSetBool("IsSkidding", false);
    }

    public void Walk(Numpad direction)
    {
        if (direction != Numpad.N4 && direction != Numpad.N6)
        {
            throw new ArgumentException(direction + " is not a horizontal direction");
        }
        int walkDirection = -10;
        if (direction == Numpad.N6) {
            walkDirection = 1;
        }
        else if (direction == Numpad.N4)
        {
            walkDirection = -1;
        }
        if (!playerState.GetCurrentFacingDirection())
        {
            walkDirection *= -1;
        }

        if (walkDirection > 0) {
            IsWalking = Numpad.N6;
        } else {
            IsWalking = Numpad.N4;
        }
    }
    public void StopWalk() {
        IsWalking = Numpad.N0;
    }

    private void WalkUpdate() {
        bool canWalk =
            isGrounded &&
            !attackController.isAttacking &&
            !isBackDashing &&
            !animator.AnimationGetBool("IsJumping");
        if (canWalk) {
            if (IsWalking == Numpad.N6) {
                rb2d.velocity = new Vector2(1 * WalkSpeed, 0f);
                UpdateFacingDirection();
            } else if (IsWalking == Numpad.N4) {
                rb2d.velocity = new Vector2(-1 * WalkSpeed, 0f);
                UpdateFacingDirection();
            }
        }
    }

    public void StartForwardDash() {
        if (isGrounded) {
            Dash(Numpad.N6);
        } else {
            AirDash(true);
        }
    }

    public void Dash(Numpad direction)
    {
        if (direction != Numpad.N6)
        {
            throw new ArgumentException(direction + " is not the forward direction");
        }
        if (!isGrounded)
        {
            throw new InvalidProgramException("Tried to Dash while airborne!");
        }
        if (isGrounded && !attackController.isAttacking && !isBackDashing && !animator.AnimationGetBool("IsJumping")) {
            float horizontalVelocity = InitialDashSpeed;
            if (!playerState.GetCurrentFacingDirection())
            {
                horizontalVelocity *= -1;
            }
            rb2d.velocity = new Vector2(horizontalVelocity, 0f);
            isRunning = true;
            hasDashMomentum = true;
            animator.AnimationSetBool("IsRunning", true);
        }
    }
    public void BackDash(Numpad direction)
    {
        if (direction != Numpad.N4)
        {
            throw new ArgumentException(direction + " is not the backward direction");
        }
        if (!isGrounded)
        {
            throw new InvalidProgramException("Tried to Backdash while airborne!");
        }
        if (isGrounded && !isBackDashing && !attackController.isAttacking && !animator.AnimationGetBool("IsJumping")) {
            StopRun();
            float horizontalVelocity = -BackDashBackSpeed;
            if (!playerState.GetCurrentFacingDirection())
            {
                horizontalVelocity *= -1;
            }
            rb2d.velocity = new Vector2(horizontalVelocity, BackDashUpSpeed);
            isBackDashing = true;
            IEnumerator coroutine = StopBackDashCoroutine();
            StartCoroutine(coroutine);
        }
        // animator.AnimationSetBool("IsRunning", true);
    }

    public void AirDash(bool isForward)
    {
        if (isGrounded)
        {
            throw new InvalidProgramException("Tried to Airdash while grounded!");
        }
        if (AirActionsLeft > 0 && !isBackDashing)
        {
            isAirDashing = true;
            rb2d.gravityScale = 0f;
            IEnumerator coroutine;
            float AirDashVelocity = -AirDashSpeed;
            float AirDashDuration = BackwardAirDashDuration;
            if (isForward)
            {
                AirDashVelocity = AirDashSpeed;
                AirDashDuration = ForwardAirDashDuration;
            }
            if (!playerState.GetCurrentFacingDirection())
            {
                AirDashVelocity *= -1;
            }
            rb2d.velocity = new Vector2(AirDashVelocity, 0f);
            coroutine = StopAirDashCoroutine(AirDashDuration);
            AirActionsLeft--;
            SoundManagerController.playSFX(SoundManagerController.airdashSound);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator StopAirDashCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb2d.gravityScale = GravityScale;
        rb2d.velocity = new Vector2(rb2d.velocity.x / 2, rb2d.velocity.y);
        isAirDashing = false;
    }
    private IEnumerator StopBackDashCoroutine()
    {
        yield return new WaitForSeconds(BackDashDuration);
        isBackDashing = false;
    }
    public void Run(Numpad direction)
    {
        if (direction != Numpad.N6)
        {
            throw new ArgumentException(direction + " is not a horizontal direction");
        }
        if (animator.AnimationGetBool("IsRunning") &&
            !animator.AnimationGetBool("IsSkidding") &&
            isGrounded && !attackController.isAttacking && !isBackDashing) {
            float horizontalVelocity = Math.Abs(rb2d.velocity.x);
            float horizontalForce = RunForce;
            if (horizontalVelocity > MaxRunSpeed)
            {
                horizontalVelocity = MaxRunSpeed;
            }
            if (!playerState.GetCurrentFacingDirection())
            {
                horizontalVelocity *= -1;
                horizontalForce *= -1;
            }
            // rb2d.velocity = new Vector2(horizontalVelocity, 0f);
            rb2d.AddForce(new Vector2(horizontalForce, 0f), ForceMode2D.Force);
        }
    }

    /// Called at the end of the skidding animation, and used to cancel Run state
    public void StopRun()
    {
        isRunning = false;
        animator.AnimationSetBool("IsSkidding", false);
        animator.AnimationSetBool("IsRunning", false);
    }
    public void Skid()
    {
        int direction = 1;
        if (!playerState.GetCurrentFacingDirection())
        {
            direction *= -1;
        }
        rb2d.velocity = new Vector2(SkidSpeed * direction, 0f);
        animator.AnimationSetBool("IsSkidding", true);
    }

    public void Jump(Numpad direction)
    {
        if (direction != Numpad.N7 && direction != Numpad.N8 && direction != Numpad.N9)
        {
            throw new ArgumentException(direction + " is not an upwards direction");
        }
        PrevJumpInput = direction;
        // TODO: can you fix this later?
        bool canJump = !attackController.isAttacking && !isHoldingJump && AirActionsLeft > 0  && !isBackDashing;
        if (canJump) {
            float horizontalVelocity = 0;
            if (hasDashMomentum) 
            {
                // Slight dash momentum factored in
                // Could be negative too
                horizontalVelocity = Math.Abs(rb2d.velocity.x / 2);
            }
            if (direction == Numpad.N7)
            {
                // Dash momentum not factored in
                horizontalVelocity = -HorizontalJumpSpeed;
                hasDashMomentum = false;
            }
            else if (direction == Numpad.N9)
            {
                horizontalVelocity += HorizontalJumpSpeed;
            }
            // P1 or P2 side
            if (!playerState.GetCurrentFacingDirection())
            {
                horizontalVelocity *= -1;
            }
            rb2d.velocity = new Vector2(horizontalVelocity, 0f);
            rb2d.AddForce(new Vector2(0f, JumpForce));
            setIsHoldingJump(true);
            animator.AnimationSetBool("IsJumping", true);
            animator.AnimationSetBool("IsRunning", false);
            StopRun();
            AirActionsLeft--;
            SoundManagerController.playSFX(SoundManagerController.jumpSound);
        }
    }

    public void setIsHoldingJump(bool state)
    {
        isHoldingJump = state;
    }

    /**
    How should movement behave on throw hit?
    */
    public void ThrowHit() {
        rb2d.velocity = new Vector2(0f, 0f);
        rb2d.bodyType = RigidbodyType2D.Kinematic;
    }

    /**
    How should movement behave after throw ends?
    */
    public void ThrowEnd() {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    public Vector2 FreezePlayer()
    {
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        Vector2 oldVelocity = rb2d.velocity;
        rb2d.velocity = new Vector2(0f, 0f);
        return oldVelocity;
    }

    public void UnFreezePlayer(Vector2 oldVelocity)
    {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.velocity = oldVelocity;
    }

    /// Reevaluate facing direction, update if necessary
    private void UpdateFacingDirection()
    {
        // updates local scale if necessary
        playerState.UpdateFacingDirection();
    }
}
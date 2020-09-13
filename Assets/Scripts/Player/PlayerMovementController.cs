using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;

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
    public bool isRunning;
    public bool isAirDashing;
    public bool isBackDashing;
    public int MaxAirActions;
    private int AirActionsLeft;
    public float GravityScale;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    public bool isGrounded;
    public bool isHoldingJump;
    private bool hasDashMomentum;

    public Rigidbody2D rb2d;
    public Animator animator;
    private PlayerAttackController attackController;
    private PlayerInputManager playerInput;
    private PlayerStateManager playerState;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<PlayerAttackController>();
        playerInput = GetComponent<PlayerInputManager>();
        playerState = GetComponent<PlayerStateManager>();
        rb2d.gravityScale = GravityScale;
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
                animator.SetBool("IsJumping", false);
                isHoldingJump = false;
                hasDashMomentum = false;
                AirActionsLeft = MaxAirActions;
            }
            isGrounded = newGrounded;

            bool isIdling = isGrounded && !attackController.isAttacking && !isBackDashing;
            if (isIdling)
            {
                UpdateFacingDirection();
            }
        }
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
        if (!playerState.GetCurrentFacingDirection())
        {
            walkDirection *= -1;
        }
        if (isGrounded && !attackController.isAttacking && !isBackDashing) {
            rb2d.velocity = new Vector2(walkDirection * WalkSpeed, 0f);
            UpdateFacingDirection();
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
        if (isGrounded && !attackController.isAttacking && !isBackDashing) {
            float horizontalVelocity = InitialDashSpeed;
            if (!playerState.GetCurrentFacingDirection())
            {
                horizontalVelocity *= -1;
            }
            rb2d.velocity = new Vector2(horizontalVelocity, 0f);
        }
        isRunning = true;
        hasDashMomentum = true;
        animator.SetBool("IsRunning", true);
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
        if (!isBackDashing && !attackController.isAttacking) {
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
        // animator.SetBool("IsRunning", true);
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
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator StopAirDashCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        rb2d.gravityScale = GravityScale;
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
        if (isGrounded && !attackController.isAttacking && !isBackDashing) {
            float horizontalVelocity = Math.Abs(rb2d.velocity.x);
            float horizontalForce = RunForce;
            if (rb2d.velocity.x > MaxRunSpeed)
            {
                horizontalVelocity = MaxRunSpeed;
            }
            if (!playerState.GetCurrentFacingDirection())
            {
                horizontalVelocity *= -1;
                horizontalForce *= -1;
            }
            rb2d.velocity = new Vector2(horizontalVelocity, 0f);
            rb2d.AddForce(new Vector2(horizontalForce, 0f));
        }
        isRunning = true;
    }

    /// Called at the end of the skidding animation, and used to cancel Run state
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
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsRunning", false);
            StopRun();
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

    /// Reevaluate facing direction, update if necessary
    private void UpdateFacingDirection()
    {
        // updates local scale if necessary
        playerState.UpdateFacingDirection();
    }
}
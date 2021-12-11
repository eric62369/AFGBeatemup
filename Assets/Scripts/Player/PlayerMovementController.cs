using UnityEngine;
using System;
using System.Collections;
using BattleInput;
using System.Threading.Tasks;

public class PlayerMovementController : MonoBehaviour {

    public float WalkSpeed;
    public float InitialDashSpeed;
    public float AirDashSpeed;

    // in frames
    public int ForwardAirDashDuration;
    public int BackwardAirDashDuration;
    private int maxAirDashFrames;
    private int framesIntoAirdash;

    public float BackDashBackSpeed;
    public float HorizontalJumpSpeed;
    public float JumpForce;
    public float RunForce;
    public float MaxRunSpeed;
    public float SkidSpeed;
    // 4 for back, 6 for forward, 5 for no walk
    public Numpad IsWalking;
    public bool isAirDashing;
    public int MaxAirActions;
    public int AirActionsLeft { get; private set; }
    public float GravityScale;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    public bool isGrounded { get; private set; }
    public bool isHoldingJump { get; private set; }
    public bool hasNotUsedJump { get; set; }
    public Numpad PrevJumpInput { get; set; }

    private bool hasDashMomentum;

    private bool inHitStop;

    private Rigidbody2D rb2d;
    // private PlayerAttackController attackController;
    // private PlayerStateManager playerState;
    // private CharacterAnimationController animator;

    // Use this for initialization
    void Start()
    {
        // Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        // attackController = GetComponent<PlayerAttackController>();
        // playerInput = GetComponent<PlayerInputManager>();
        // playerState = GetComponent<PlayerStateManager>();
        rb2d.gravityScale = GravityScale;
        // animator = GetComponent<CharacterAnimationController>();
        IsWalking = Numpad.N5;
        inHitStop = false;
        framesIntoAirdash = 0;
        hasNotUsedJump = false;
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        if (IsWalking == Numpad.N6) {
            rb2d.velocity = new Vector2(WalkSpeed, 0);
        } else if (IsWalking == Numpad.N4) {
            rb2d.velocity = new Vector2(-WalkSpeed, 0);
        }
    }

    public void Walk(Numpad direction) {
        IsWalking = direction;
    }
}
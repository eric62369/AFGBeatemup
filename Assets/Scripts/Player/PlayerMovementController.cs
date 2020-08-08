using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class PlayerMovementController : MonoBehaviour {

    public float WalkForce;                //Floating point variable to store the player's movement speed.
    public float JumpForce;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    public Rigidbody2D rb2d;        //Store a reference to the Rigidbody2D component required to use 2D Physics.
    public bool isGrounded;
    public Animator animator;
    private PlayerAttackController attackController;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackController = GetComponent<PlayerAttackController>();
    }
    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxisRaw("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 horizontalMovement = new Vector2 (moveHorizontal, 0f);
    
        if (isGrounded && !attackController.isAttacking && moveVertical > 0) {
            Jump(moveHorizontal);
        }

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        if (isGrounded && !attackController.isAttacking) {
            rb2d.velocity = horizontalMovement * WalkForce;
        }

    }

    public void Jump(float moveHorizontal)
    {
        rb2d.velocity = new Vector2(moveHorizontal * WalkForce, 0f);
        rb2d.AddForce(new Vector2(0f, JumpForce));
        isGrounded = false;
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
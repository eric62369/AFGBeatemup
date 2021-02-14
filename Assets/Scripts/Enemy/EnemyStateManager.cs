using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public bool isBeingThrown { get; private set; }

    private EnemyMovementController movementController;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool newGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        if (newGrounded != isGrounded && newGrounded == true)
        {
            // // Landed!
            // animator.AnimationSetBool("IsJumping", false);
            // hasDashMomentum = false;
            // AirActionsLeft = MaxAirActions;

            // if (isHoldingJump) {
            //     isHoldingJump = false;
            //     Jump(PrevJumpInput);
            // }
        }
        isGrounded = newGrounded;
    }

    public void TakeThrow(PlayerStateManager playerState)
    {
        isBeingThrown = true;
        Vector3 playerPosition = playerState.GetCurrentPosition();
        playerState.ThrowHit();
        float posOffset = playerState.GetThrowPositionOffset();
        this.gameObject.transform.position = new Vector3(
            playerPosition.x + posOffset, playerPosition.y, playerPosition.z);
    }

    public void GetHitOutOfThrow()
    {
        isBeingThrown = false;
    }

    public void GetLaunched(Attack attackData)
    {
        movementController.LaunchEnemy(attackData.GetPushBackDirection());
    }

    public void GetDunked(Attack attackData)
    {
        movementController.DunkEnemy(attackData.GetPushBackDirection());
    }
}

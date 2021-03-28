using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour, IStateManager
{
    public bool isBeingThrown { get; private set; }

    private EnemyMovementController movementController;

    private bool p1Side;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayers;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        p1Side = false;
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
    
    // Interface functions

    public int GetPlayerIndex()
    {
        // TODO: Make unique enemy Ids
        return 1;
    }
    public Vector3 GetCurrentPosition()
    {
        return this.gameObject.transform.position;
    }

    public PlayerAttackController GetAttackController()
    {
        throw new NotImplementedException();
    }
    public void ThrowHit()
    {
        throw new NotImplementedException();
        // animator.AnimationSetBool("ThrowHit", true);
        // attackController.ThrowFreeze();
    }
    public float GetThrowPositionOffset()
    {
        throw new NotImplementedException();
    }
    public bool GetIsP1Side()
    {
        return p1Side;
    }
    public void TurnCharacterAround()
    {
        p1Side = !p1Side;
        if (p1Side) {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void TakeThrow(IStateManager playerState)
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

    public void GetHeavyLaunched(Attack attackData)
    {
        movementController.HeavyLaunchEnemy(attackData.GetPushBackDirection());
    }

    public void GetDunked(Attack attackData)
    {
        movementController.DunkEnemy(attackData.GetPushBackDirection());
    }
}

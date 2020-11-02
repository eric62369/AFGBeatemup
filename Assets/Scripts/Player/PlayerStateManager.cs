using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField]  // TODO: What's this?
    private int playerIndex; // 0 is P1, 1 is P2

    private GameObject boss;
    /// i.e. I jumped and crossed up, I airdash forward (the direction I'm facing)
    private bool isFacingRight;
    private PlayerInputManager inputManager;
    private PlayerMovementController movementController;
    private PlayerAttackController attackController;
    private bool forwardThrowing;
    private Numpad cancelActionInput;

    // Start is called before the first frame update
    void Start()
    {
        SearchForBoss();
        inputManager = GetComponent<PlayerInputManager>();
        movementController = GetComponent<PlayerMovementController>();
        attackController = GetComponent<PlayerAttackController>();
    }
    
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    /// Set reference to the current boss enemy
    public void SearchForBoss()
    {
        boss = GameObject.FindWithTag("Boss");
        if (boss == null)
        {
            throw new InvalidOperationException("Tried to search for boss when no boss found");
        }
    }

    /// If on same x position, player is on P1 side
    /// Gets absolute p1 or p2 side, not facing direction
    public bool GetIsP1Side()
    {
        if (boss == null)
        {
            throw new InvalidOperationException(
                "Player state must have a boss enemy reference first (currently null)"
            );

        }
        float posDiff = this.gameObject.transform.position.x - boss.transform.position.x;
        return posDiff <= 0;
    }

    /// Return last updated facing direction
    /// True is Right facing (P1) False is Left Facing
    public bool GetCurrentFacingDirection()
    {
        return isFacingRight;
    }

    public void UpdateFacingDirection()
    {
        Vector3 newScale = this.gameObject.transform.localScale;
        newScale.x = Math.Abs(newScale.x);
        bool oldDirection = isFacingRight;
        if (!GetIsP1Side())
        {
            newScale.x *= -1;
            isFacingRight = false;
        }
        else
        {
            isFacingRight = true;
        }
        if (oldDirection != isFacingRight)
        {
            this.gameObject.transform.localScale = newScale;
            inputManager.FacingDirectionChanged();
        }
    }

    public Vector3 GetCurrentPosition()
    {
        return this.gameObject.transform.position;
    }

    //////////////////
    // THROW
    //////////////////
    public void SetThrowDirection(bool isForward)
    {
        forwardThrowing = isForward;
    }
    public float GetThrowPositionOffset()
    {
        float positionOffset = 1f;
        if (!GetCurrentFacingDirection())
        {
            positionOffset *= -1;
        }
        if (!forwardThrowing)
        {
            positionOffset *= -1;
        }
        return positionOffset;
    }
    public void ThrowHit()
    {
        movementController.AnimationSetBool("ThrowHit", true);
        attackController.ThrowFreeze();
    }
    public void ExitThrowWhiff()
    {
        movementController.AnimationSetBool("ThrowWhiff", false);
        UpdateFacingDirection();
    }
    public void ExitThrowHit()
    {
        movementController.AnimationSetBool("ThrowHit", false);
        attackController.ThrowUnFreeze();
    }

    //////////////////
    // CANCELS
    //////////////////
    public void SetCancelAction(CancelAction action, Numpad _cancelActionInput)
    {
        cancelActionInput = _cancelActionInput;
        attackController.SetCancelAction(action);
    }

    public void UseCancelAction(CancelAction? action)
    {
        if (action == null)
        {
            throw new ArgumentException("Tried to use null cancel action!");
        }
        // reset states to neutral
        ResetStateToNeutral();
        if (action == CancelAction.Jump)
        {
            movementController.Jump(cancelActionInput);
        }
        movementController.AnimationSetBool("CanCancel", false);
    }

    private void ResetStateToNeutral()
    {
        movementController.ResetMovementStateToNeutral();
        attackController.ResetAttackStateToNeutral();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour, IStateManager
{
    public bool isBlocking { get; private set; }

    public bool isBeingThrown { get; private set; }

    private EnemyMovementController movementController;
    private EnemyAttackController attackController;

    private bool p1Side;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        attackController = GetComponent<EnemyAttackController>();
        p1Side = false;
        isBlocking = true;
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

    public IAttackController GetAttackController()
    {
        return attackController;
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
        Vector3 scale = this.gameObject.transform.localScale;
        if (p1Side) {
            this.gameObject.transform.localScale = new Vector3(-Math.Abs(scale.x), scale.y, scale.z);
        } else {
            this.gameObject.transform.localScale = new Vector3(Math.Abs(scale.x), scale.y, scale.z);
        }
    }

    public void Block() {
        isBlocking = true;
    }

    public void RemoveBlock() {
        isBlocking = false;
    }

    public void TakeThrow(IStateManager playerState)
    {
        isBeingThrown = true;
        RemoveBlock();
        Vector3 playerPosition = playerState.GetCurrentPosition();
        float posOffset = playerState.GetThrowPositionOffset();
        playerState.ThrowHit();
        this.gameObject.transform.position = new Vector3(
            playerPosition.x + posOffset, playerPosition.y, playerPosition.z);
    }

    public void GetHitOutOfThrow()
    {
        isBeingThrown = false;
    }

    public void GetLaunched(Attack attackData)
    {
        movementController.LaunchEnemy(attackData.GetPushBackDirection(movementController.xPosition));
    }

    public void GetHeavyLaunched(Attack attackData)
    {
        movementController.HeavyLaunchEnemy(attackData.GetPushBackDirection(movementController.xPosition));
    }

    public void GetDunked(Attack attackData)
    {
        movementController.DunkEnemy(attackData.GetPushBackDirection(movementController.xPosition));
    }
}

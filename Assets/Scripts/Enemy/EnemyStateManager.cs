using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public bool isBeingThrown { get; private set; }

    private EnemyMovementController movementController;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }
}

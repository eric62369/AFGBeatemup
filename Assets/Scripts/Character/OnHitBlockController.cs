using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitBlockController : MonoBehaviour
{
    private IMovementController movementController;
    private CharacterAnimationController animator;
    private IStateManager state;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<IMovementController>();
        state = GetComponent<IStateManager>();
        animator = GetComponent<CharacterAnimationController>();
        movementController.GetHitEvent += OnHitOrBlock;
    }

    private void OnHitOrBlock(object sender, GetHitEventArgs e) {
        animator.AnimationSetTrigger("GotHit");
        animator.AnimationSetFloat("StunAnimationSpeed", e.attackData.GetStunSpeed());
        state.canAct = false;
    }

    public void ExitHitOrBlock() {
        state.canAct = true;
    }
}

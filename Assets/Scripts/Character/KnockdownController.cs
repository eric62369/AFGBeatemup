using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockdownController : MonoBehaviour
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
        movementController.LandEvent += CheckKnockdown;
    }

    private void CheckKnockdown(object sender, LandEventArgs e) {
        // Called on Landing
        if (!state.canAct) {
            // in hitstun or blockstun: For now just do knockdown
            animator.AnimationSetTrigger("Knockdown");
        }
    }

    public void ExitKnockdown() {
        state.canAct = true;
    }
}

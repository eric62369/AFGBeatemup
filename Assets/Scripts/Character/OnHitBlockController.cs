using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitBlockController : MonoBehaviour
{
    private IMovementController movementController;
    private CharacterAnimationController animator;
    
    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<IMovementController>();
        animator = GetComponent<CharacterAnimationController>();
        movementController.GetHitEvent += OnHitOrBlock;
    }

    private void OnHitOrBlock(object sender, GetHitEventArgs e) {
        // if (animator.AnimationGetBool("isBlocking")) {
        //     animator.AnimationSetTrigger("EnterBlocking");
        // } else {
            // animator.AnimationSetTrigger("GotHit");
        // }
        animator.AnimationSetTrigger("GotHit");
        
        // Infinite stun on overkill
        // if (defeat.isDefeated) {
        //     animator.AnimationSetFloat("StunAnimationSpeed", 0f);
        // } else {
        //     animator.AnimationSetFloat("StunAnimationSpeed", e.attackData.GetStunSpeed());
        // }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutCrabAI : MonoBehaviour
{
    public float StepForce;
    public int StepsInOneDirection;
    private int StepsTakenInOneDirection;
    private EnemyMovementController movementController;
    private IStateManager stateManager;
    private CharacterAnimationController animator;
    private DefeatProcess defeat;
    
    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        stateManager = GetComponent<IStateManager>();
        animator = GetComponent<CharacterAnimationController>();
        defeat = GetComponent<DefeatProcess>();
        movementController.GetHitEvent += CoconutCrabOnHit;
    }
    
    void StepForward() {
        float direction = 1;
        if (!stateManager.GetIsP1Side()) {
            direction *= -1;
        }
        movementController.Pushback(new Vector2(direction * StepForce, 0));
        StepsTakenInOneDirection++;
    }

    void CheckTurnAround() {
        if (StepsTakenInOneDirection >= StepsInOneDirection) {
            StepsTakenInOneDirection = 0;
            stateManager.TurnCharacterAround();
        }
    }

    private void CoconutCrabOnHit(object sender, GetHitEventArgs e) {
        if (stateManager.isBlocking) {
            animator.AnimationSetTrigger("EnterBlocking");
        } else {
            animator.AnimationSetTrigger("GotHit");
        }
        
        // Infinite stun on overkill
        if (defeat.isDefeated) {
            animator.AnimationSetFloat("StunAnimationSpeed", 0f);
        } else {
            animator.AnimationSetFloat("StunAnimationSpeed", e.attackData.GetStunSpeed());
        }
    }
}

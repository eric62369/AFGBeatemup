// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;

// public class EnemyAttackController : UniversalAttackController
// {
//     IMovementController movementController;
//     private CharacterAnimationController animator;

//     // Start is called before the first frame update
//     void Start()
//     {
//         // movementController = GetComponent<IMovementController>();
//         // animator = GetComponent<CharacterAnimationController>();
//         movementController = null;
//         animator = null;
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     public Vector2 FreezePlayer()
//     {
//         animator.AnimatorEnable(false);
//         return movementController.FreezeCharacter();
//     }
//     public void UnFreezePlayer(Vector2 oldVelocity)
//     {
//         animator.AnimatorEnable(true);
//         movementController.UnFreezeCharacter(oldVelocity);
//     }
// }

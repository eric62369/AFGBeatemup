using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleInput {
    public class RedBattleInputActions : MonoBehaviour, IBattleInputActions {
        private PlayerAttackController playerAttack;
        private PlayerMovementController playerMovement;
        private PlayerStateManager playerState;
        private PlayerAnimationController playerAnimator;

        void Start () {
            playerAttack = GetComponent<PlayerAttackController> ();
            playerMovement = GetComponent<PlayerMovementController>();
            playerState = GetComponent<PlayerStateManager>();
            playerAnimator = GetComponent<PlayerAnimationController>();
        }

        // Universal Movement
        public void Dash () {
            playerMovement.StartForwardDash();
            Walk(Numpad.N6);
        }
        public void BackDash () {
            playerMovement.StartBackwardDash();
            Walk(Numpad.N4);
        }
        public void StopRun () {
            if (playerAnimator.AnimationGetBool("IsRunning")) {
                playerMovement.Skid();
            }
        } 
        public void Walk (Numpad direction) {
            playerMovement.Walk(direction);
        }
        public void StopWalk() {
            playerMovement.StopWalk();
        }
        public void Jump (Numpad direction) {
            if (!playerMovement.isHoldingJump) {
                playerState.SetCancelAction(CancelAction.Jump, direction);
            }
            playerMovement.PrevJumpInput = direction;
            playerMovement.setIsHoldingJump(true);
        }
        public void ReleaseJump () {
            playerMovement.setIsHoldingJump(false);
            playerMovement.hasNotUsedJump = true;
        }

        // Universal Actions
        public void Throw (bool direction) {
            playerAttack.Throw(direction);
        }
        public void InputBufferCancel(int frameLimit) {
            playerAttack.InputBufferCancel(frameLimit);
        }
        public void RC(int frameLimit) {
            playerAttack.RC(frameLimit);
        }

        // Normals
        public void N5 (Button button) {
            if (playerMovement.isGrounded) {
                switch (button) {
                    case Button.A:
                        playerAttack.Attack5A ();
                        break;
                    case Button.B:
                        playerAttack.Attack5B ();
                        break;
                    case Button.C:
                        playerAttack.Attack5C ();
                        break;
                    case Button.D:
                        break;
                    default:
                        throw new InvalidOperationException (button + " was not an expected normal button!");
                }
            } else {
                switch (button) {
                    case Button.A:
                        playerAttack.AttackJ5A ();
                        break;
                    case Button.B:
                        playerAttack.AttackJ5B ();
                        break;
                    case Button.C:
                        playerAttack.AttackJ5C ();
                        break;
                    case Button.D:
                        break;
                    default:
                        throw new InvalidOperationException (button + " was not an expected normal button!");
                }
            }

        }

        // Command Normals

        // Specials
        public void S236 (Button button) { }
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BattleInput;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleInput {
    public class BattleInputParser : MonoBehaviour {
        public int Time66; // in (frames) window to input 66 (dash)
        public int Time236; // in (frames) window to input 236

        public IBattleInputActions inputActions;

        // TODO: Move initialization for these inputs somewhere else (these are universal inputs)
        // Full list of attack commands to interpret will probably come from the input actions
        private static AttackMotionInput S236B;
        private static AttackMotionInput N5B;
        private static AttackMotionInput N5C;
        private static MotionInput M66;
        private static MotionInput M44;
        private static AttackMotionInput ForwardThrow;
        private static AttackMotionInput BackwardThrow;

        void Start () {
            // Might not work, interface might need to be a component
            inputActions = GetComponent<IBattleInputActions> ();
            initMotionInputs ();
        }

        private void initMotionInputs () {
            IList<string> list236B = new List<string> ();
            list236B.Add ("236");
            list236B.Add ("2365");
            list236B.Add ("2369");
            S236B = new AttackMotionInput (list236B, "B", Time236);

            IList<string> list66 = new List<string> ();
            list66.Add ("656");
            list66.Add ("956");
            list66.Add ("9856");
            list66.Add ("9656");
            M66 = new MotionInput (list66, Time66);

            IList<string> list44 = new List<string> ();
            list44.Add ("454");
            list44.Add ("754");
            list44.Add ("7854");
            list44.Add ("7454");
            M44 = new MotionInput (list44, Time66);

            IList<string> list5B = new List<string> ();
            list5B.Add ("5");
            N5B = new AttackMotionInput (list5B, "B", 0);

            IList<string> list5C = new List<string> ();
            list5C.Add ("5");
            N5C = new AttackMotionInput (list5C, "C", 0);

            IList<string> listForwardThrow = new List<string> ();
            listForwardThrow.Add ("6");
            ForwardThrow = new AttackMotionInput (listForwardThrow, "AD", 0);

            IList<string> listBackwardThrow = new List<string> ();
            listBackwardThrow.Add ("4");
            BackwardThrow = new AttackMotionInput (listBackwardThrow, "AD", 0);
        }

        /**
        Warning, not called every frame
        Only called when new input is detected from scanner
        */
        public void ParseNewInput (InputHistory inputHistory) {
            bool matched = false;
            matched = InterpretAttack (inputHistory);
            if (!matched) {
                matched = InterpretMovement (inputHistory);
            }
        }

        /// <summary>
        /// Attack input priority is determined here too!
        /// </summary>
        /// <param name="inputHistory"></param>
        /// <returns>true if interpreted to something</returns>
        private bool InterpretAttack (InputHistory inputHistory) {
            // invul moves

            // button combos

            // specials
            if (InterpretUtil.InterpretSpecialAttackInput (inputHistory, S236B)) {
                Debug.Log (S236B.ToString ());
                inputActions.S236 (Button.B);
                return true;
            }

            // // normals
            // if (InterpretUtil.InterpretNormalAttackInput(inputHistory, N5B)) {
            //     Debug.Log(N5B.ToString());
            //     inputActions.N5(Button.B);
            //     return true;
            // }
            // if (InterpretUtil.InterpretNormalAttackInput(inputHistory, N5C)) {
            //     Debug.Log(N5C.ToString());
            //     inputActions.N5(Button.C);
            //     return true;
            // }
            // if (buttonsDown.Contains(Button.A))
            // {
            //     InterpretSpecial(Button.A);
            // }
            // else if (buttonsDown.Contains(Button.B))
            // {
            //     InterpretSpecial(Button.B);
            // }
            // else if (buttonsDown.Contains(Button.C))
            // {
            //     InterpretSpecial(Button.C);
            // }
            // else if (buttonsDown.Contains(Button.D))
            // {
            //     Numpad firstInput = inputHistory[0];
            //     switch (firstInput)
            //     {
            //         case Numpad.N6:
            //             playerAttack.Throw(true);
            //             break;
            //         case Numpad.N4:
            //             playerAttack.Throw(false);
            //             break;
            //         default:
            //             break;
            //     }
            // }

            // // empty out button bag
            // while (!buttonDownBag.IsEmpty)
            // {
            //     Button button;
            //     buttonDownBag.TryTake(out button);
            // }
            return false;
        }

        private bool InterpretMovement (InputHistory inputHistory) {
            // if (IsNumpadUp(firstInput))
            // {
            //     playerState.SetCancelAction(CancelAction.Jump, firstInput);
            //     playerMovement.Jump(firstInput);
            // }
            // else if (!playerMovement.isRunning && (firstInput == Numpad.N6 || firstInput == Numpad.N4))
            // {
            //     // Walk check
            //     playerMovement.Walk(firstInput);
            // }
            // else if (playerMovement.isRunning && (firstInput == Numpad.N6 || firstInput == Numpad.N3) && !animator.AnimationGetBool("IsSkidding"))
            // {
            //     // Holding run check
            //     playerMovement.Run(firstInput);
            // }
            // else
            // {
            //     if (animator.AnimationGetBool("IsRunning") && !animator.AnimationGetBool("IsSkidding"))
            //     {
            //         playerMovement.Skid();
            //     }
            //     // Nothing / Idle
            // }

            // if (!IsNumpadUp(firstInput))
            // {
            //     playerMovement.setIsHoldingJump(false);
            // }
            return false;
        }
        private bool IsNumpadUp (Numpad num) {
            return num == Numpad.N7 || num == Numpad.N8 || num == Numpad.N9;
        }

        // private void InterpretDash()
        // {
        //     Numpad firstInput = inputHistory[0];
        //     Numpad secondInput = inputHistory[1];
        //     Numpad thirdInput = inputHistory[2];
        //     Numpad fourthInput = inputHistory[3];
        //     float firstTime = timeHistory[0];
        //     float secondTime = timeHistory[1];

        //     if (!playerAttack.isAttacking)
        //     {
        //         bool forwardDash = 
        //             (firstInput == Numpad.N6 && secondInput == Numpad.N5 && (thirdInput == Numpad.N6 || thirdInput == Numpad.N9)) ||
        //             (firstInput == Numpad.N6 && secondInput == Numpad.N5 && thirdInput == Numpad.N8 && fourthInput == Numpad.N9);
        //         bool backwardDash = 
        //             (firstInput == Numpad.N4 && secondInput == Numpad.N5 && (thirdInput == Numpad.N4 || thirdInput == Numpad.N7)) ||
        //             (firstInput == Numpad.N4 && secondInput == Numpad.N5 && thirdInput == Numpad.N8 && fourthInput == Numpad.N7);
        //         if (forwardDash && !animator.AnimationGetBool("IsRunning") && !animator.AnimationGetBool("IsSkidding"))
        //         {
        //             if (firstTime + secondTime <= Time66)
        //             {
        //                 if (playerMovement.isGrounded)
        //                 {
        //                     // Grounded forward step dash
        //                     playerMovement.Dash(firstInput);
        //                 }
        //                 else
        //                 {
        //                     // forward airdash
        //                     playerMovement.AirDash(true);
        //                 }
        //             }
        //         }
        //         else if (backwardDash)
        //         {
        //             if (firstTime + secondTime <= Time66)
        //             {
        //                 if (playerMovement.isGrounded)
        //                 {
        //                     // Grounded backdash
        //                     playerMovement.BackDash(firstInput);
        //                 }
        //                 else
        //                 {
        //                     // back airdash
        //                     playerMovement.AirDash(false);
        //                 }
        //             }
        //         }
        //     }
        // }
        // private void InterpretSpecial(Button button)
        // {
        //     Numpad firstInput = inputHistory[0];
        //     Numpad secondInput = inputHistory[1];
        //     Numpad thirdInput = inputHistory[2];
        //     Numpad fourthInput = inputHistory[3];
        //     float firstTime = timeHistory[0];
        //     float secondTime = timeHistory[1];
        //     float thirdTime = timeHistory[2];
        //     // 236 or 236? motion
        //     if (thirdInput == Numpad.N2 && secondInput == Numpad.N3 && firstInput == Numpad.N6)
        //     {
        //         // Motion detected!
        //         if (firstTime + runningTime <= Time236)
        //         {
        //             if (button == Button.A)
        //             {
        //                 Debug.Log("Stun Edge!");
        //             }
        //         }
        //     }
        //     else if (fourthInput == Numpad.N2 && thirdInput == Numpad.N3 && secondInput == Numpad.N6) 
        //     {
        //         // Motion detected!
        //         if (secondTime + firstTime + runningTime <= Time236)
        //         {
        //             if (button == Button.A)
        //             {
        //                 Debug.Log("Stun Edge Extra!");
        //             }
        //         }
        //     }
        //     else
        //     {
        //         // Normals
        //         if (button == Button.A)
        //         {
        //             playerAttack.Attack5B();
        //         }
        //         else if (button == Button.B)
        //         {
        //             playerAttack.Attack5C();
        //         }
        //     }
        // }
    }
}
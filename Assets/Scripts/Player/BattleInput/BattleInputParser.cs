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

        public bool DEBUG; // Print Debug Messages

        public IBattleInputActions inputActions;
        public BattleInputScanner scanner; // TODO: try to remove this if possible

        // TODO: Move initialization for these inputs somewhere else (these are universal inputs)
        // Full list of attack commands to interpret will probably come from the input actions
        private static AttackMotionInput S236B;
        private static AttackMotionInput N5A;
        private static AttackMotionInput N5B;
        private static AttackMotionInput N5C;
        private static MotionInput M66;
        private static MotionInput M44;
        private static MotionInput M4;
        private static MotionInput M6;
        private static MotionInput MJump;
        private static AttackMotionInput ForwardThrow;
        private static AttackMotionInput BackwardThrow;
        private static AttackMotionInput RC;

        void Start () {
            // Might not work, interface might need to be a component
            inputActions = GetComponent<IBattleInputActions> ();
            scanner = GetComponent<BattleInputScanner>();
            initMotionInputs ();
        }

        private void DebugMessage(String message) {
            if (DEBUG) {
                Debug.Log(message);
            }
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

            IList<string> list4 = new List<string> ();
            list4.Add ("4");
            M4 = new MotionInput (list4, 0);
            IList<string> list6 = new List<string> ();
            list6.Add ("6");
            M6 = new MotionInput (list6, 0);
            IList<string> listJump = new List<string> ();
            listJump.Add ("8");
            listJump.Add ("7");
            listJump.Add ("9");
            MJump = new MotionInput (listJump, 0);

            IList<string> list5A = new List<string> ();
            list5A.Add ("5");
            list5A.Add ("6");
            list5A.Add ("4");
            list5A.Add ("1");
            list5A.Add ("2");
            list5A.Add ("3");
            list5A.Add ("7");
            list5A.Add ("8");
            list5A.Add ("9");
            N5A = new AttackMotionInput (list5A, "A", 0);

            IList<string> list5B = new List<string> ();
            list5B.Add ("5");
            list5B.Add ("6");
            list5B.Add ("4");
            list5B.Add ("1");
            list5B.Add ("2");
            list5B.Add ("3");
            list5B.Add ("7");
            list5B.Add ("8");
            list5B.Add ("9");
            N5B = new AttackMotionInput (list5B, "B", 0);

            IList<string> list5C = new List<string> ();
            list5C.Add ("5");
            list5C.Add ("6");
            list5C.Add ("4");
            list5C.Add ("1");
            list5C.Add ("2");
            list5C.Add ("3");
            list5C.Add ("7");
            list5C.Add ("8");
            list5C.Add ("9");
            N5C = new AttackMotionInput (list5C, "C", 0);

            IList<string> listForwardThrow = new List<string> ();
            listForwardThrow.Add ("6");
            ForwardThrow = new AttackMotionInput (listForwardThrow, "AD", 2);

            IList<string> listBackwardThrow = new List<string> ();
            listBackwardThrow.Add ("4");
            BackwardThrow = new AttackMotionInput (listBackwardThrow, "AD", 2);

            IList<string> listRC = new List<string> ();
            listRC.Add ("5");
            listRC.Add ("6");
            listRC.Add ("4");
            listRC.Add ("1");
            listRC.Add ("2");
            listRC.Add ("3");
            listRC.Add ("7");
            listRC.Add ("8");
            listRC.Add ("9");
            RC = new AttackMotionInput (listRC, "ABD", 3);
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
            if (InterpretUtil.InterpretTapButtonCombo(inputHistory, RC)) {
                DebugMessage(RC.ToString());
                inputActions.RC(RC.frameLimit);
                return true;
            }

            if (InterpretUtil.InterpretTapButtonCombo(inputHistory, ForwardThrow)) {
                DebugMessage(ForwardThrow.ToString());
                inputActions.InputBufferCancel(ForwardThrow.frameLimit);
                inputActions.Throw(true);
                return true;
            }

            if (InterpretUtil.InterpretTapButtonCombo(inputHistory, BackwardThrow)) {
                DebugMessage(BackwardThrow.ToString());
                inputActions.InputBufferCancel(BackwardThrow.frameLimit);
                inputActions.Throw(false);
                return true;
            }

            // specials
            if (InterpretUtil.InterpretSpecialAttackInput (inputHistory, S236B)) {
                DebugMessage(S236B.ToString());
                inputActions.S236 (Button.B);
                return true;
            }

            // command normals

            // normals
            if (InterpretUtil.InterpretNormalAttackInput(inputHistory, N5A)) {
                DebugMessage(N5A.ToString());
                inputActions.N5(Button.A);
                return true;
            }
            if (InterpretUtil.InterpretNormalAttackInput(inputHistory, N5B)) {
                DebugMessage(N5B.ToString());
                inputActions.N5(Button.B);
                return true;
            }
            if (InterpretUtil.InterpretNormalAttackInput(inputHistory, N5C)) {
                DebugMessage(N5C.ToString());
                inputActions.N5(Button.C);
                return true;
            }

            return false;
        }

        private bool InterpretMovement (InputHistory inputHistory) {
            inputActions.StopWalk();
            if (InterpretUtil.InterpretMotionInput(inputHistory, M66))
            {
                DebugMessage(M66.ToString());
                inputActions.Dash();
                return true;
            } else {
                inputActions.StopRun();
            }

            if (InterpretUtil.InterpretMotionInput(inputHistory, M44))
            {
                DebugMessage(M44.ToString());
                inputActions.BackDash();
                return true;
            }


            if (InterpretUtil.InterpretMotionInput(inputHistory, MJump))
            {
                DebugMessage(MJump.ToString());
                inputActions.Jump(inputHistory.GetEntry(0).direction);
                return true;
            } else {
                inputActions.ReleaseJump();
            }

            if (InterpretUtil.InterpretMotionInput(inputHistory, M6))
            {
                DebugMessage(M6.ToString());
                inputActions.Walk(Numpad.N6);
                return true;
            }
            if (InterpretUtil.InterpretMotionInput(inputHistory, M4))
            {
                DebugMessage(M4.ToString());
                inputActions.Walk(Numpad.N4);
                return true;
            }
            
            return false;
        }
    }
}
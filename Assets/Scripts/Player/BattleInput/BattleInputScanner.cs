using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleInput {

    public enum Numpad {
        N0, // Initial state (no inputs detected yet)
        N5, // Neutral
        N1, // Downback
        N2, // Down
        N3, // Downforward
        N4, // Back
        N6, // Forward
        N7, // Upback
        N8, // Up
        N9, // Upforward
    }
    public enum ButtonStatus {
        Down, // Pressed down on this frame!
        Hold, // Button was pressed earlier, being held down now
        Release, // Button was released this frame! (Negative edge)
        Up // Button is in neutral (not pressed) state
    }

    /// Mainly responsible for managing input history
    public class BattleInputScanner : MonoBehaviour {
        private static readonly int ButtonCount = 4; // A B C D
        public static readonly int InputHistorySize = 18; // size for input history
        private int runningFrames; // How much time (in frames) since last input?

        private BattleInputParser parser;

        // Input history data
        private InputHistory inputHistory;

        // Received inputs from ControllerReader
        private Numpad nextDirection;
        private IList<ButtonStatus> nextButtons;
        // new inputs must be added to the input history on the next frame!
        private bool newInputs;

        void Start () {
            runningFrames = 0; // Frame 1 will be first update frame

            parser = GetComponent<BattleInputParser> ();

            nextDirection = Numpad.N5;
            nextButtons = new List<ButtonStatus> ();
            for (int j = 0; j < ButtonCount; j++) {
                nextButtons.Add (ButtonStatus.Up);
            }
            newInputs = false;

            // initialize input history
            inputHistory = new InputHistory (InputHistorySize, ButtonCount);
        }

        // every frame update
        void Update () {
            runningFrames++;

            if (newInputs) {
                // Add all received inputs to input history
                IList<ButtonStatus> copyButtons = new List<ButtonStatus> ();
                for (int i = 0; i < ButtonCount; i++) {
                    copyButtons.Add (nextButtons[i]);
                }
                inputHistory.AddNewEntry (
                    nextDirection,
                    copyButtons,
                    runningFrames
                );

                // pass data to input parser
                parser.ParseNewInput (inputHistory);

                // // print input history to console
                Debug.Log (inputHistory.ToString ());

                // reset flags and running state
                newInputs = false;
                runningFrames = 0; // Frame 1 will be the next new update frame
                for (int i = 0; i < ButtonCount; i++) {
                    ButtonStatus status = nextButtons[i];
                    if (status == ButtonStatus.Down) {
                        nextButtons[i] = ButtonStatus.Hold;
                    } else if (status == ButtonStatus.Release) {
                        nextButtons[i] = ButtonStatus.Up;
                    }
                }
            }
        }

        /// Called when new input received
        /// takes a numpad direction
        /// modifies input and time History
        public void InterpretNewStickInput (Numpad newInput) {
            nextDirection = newInput;
            newInputs = true;
        }

        // TODO: Change Controller Reader to detect button release (and maybe hold)
        public void InterpretNewButtonInput (Button buttonPressed, bool isPressed) {
            ButtonStatus newStatus = ButtonStatus.Down;
            if (!isPressed) {
                newStatus = ButtonStatus.Release;
            }
            switch (buttonPressed) {
                case Button.A:
                    nextButtons[0] = newStatus;
                    break;
                case Button.B:
                    nextButtons[1] = newStatus;
                    break;
                case Button.C:
                    nextButtons[2] = newStatus;
                    break;
                case Button.D:
                    nextButtons[3] = newStatus;
                    break;
                default:
                    throw new InvalidOperationException (buttonPressed + " is not an ABCD button!");
            }
            newInputs = true;
        }

        // TODO: Connect to player's turnaround event somehow
        // public void FacingDirectionChanged()
        // {
        //     // TODO: can be switch case or something else
        //     if (currentInput == Numpad.N7)
        //     {
        //         InterpretNewStickInput(Numpad.N9);
        //     }
        //     else if (currentInput == Numpad.N4)
        //     {
        //         InterpretNewStickInput(Numpad.N6);
        //     }
        //     else if (currentInput == Numpad.N1)
        //     {
        //         InterpretNewStickInput(Numpad.N3);
        //     }
        //     else if (currentInput == Numpad.N9)
        //     {
        //         InterpretNewStickInput(Numpad.N7);
        //     }
        //     else if (currentInput == Numpad.N6)
        //     {
        //         InterpretNewStickInput(Numpad.N4);
        //     }
        //     else if (currentInput == Numpad.N3)
        //     {
        //         InterpretNewStickInput(Numpad.N1);
        //     }
        //     newInputs = true;
        // }
    }
}
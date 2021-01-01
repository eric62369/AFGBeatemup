using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleInput {
    public class AttackMotionInput : MotionInput {
        public ButtonStatus[] buttons;

        private string displayButtons;

        public AttackMotionInput (IList<string> inputs_, string buttons_, int frameLimit_) : base (inputs_, frameLimit_) {
            this.buttons = StringToButtons (buttons_);
            this.displayButtons = buttons_;
        }

        public override String ToString () {
            return "AttackMotionInput: " + base.displayInput + displayButtons;
        }

        private ButtonStatus[] StringToButtons (string input) {
            // TODO: Remove hardcoded 4
            ButtonStatus[] buttons = new ButtonStatus[4];
            for (int i = 0; i < buttons.Length; i++) {
                buttons[i] = ButtonStatus.Up;
            }

            for (int i = input.Length - 1; i >= 0; i--) {
                switch (input[i]) {
                    case 'A':
                        buttons[0] = ButtonStatus.Down;
                        break;
                    case 'B':
                        buttons[1] = ButtonStatus.Down;
                        break;
                    case 'C':
                        buttons[2] = ButtonStatus.Down;
                        break;
                    case 'D':
                        buttons[3] = ButtonStatus.Down;
                        break;
                    default:
                        throw new InvalidOperationException (input[i] + " is not an expected Button input!");
                }
            }

            return buttons;
        }
    }

    public class MotionInput {
        /// <summary>
        /// Contains multiple definitions of a certain motion input
        /// 
        /// 0th element is last input, last element is first input
        /// 6523 -> 3256, 236 -> 632
        /// </summary>
        /// <typeparam name="Numpad">represent directions</typeparam>
        public IList<IList<Numpad>> motionInputs { get; private set; }

        public int frameLimit { get; private set; }

        /// <summary>
        /// Note: just for ToString use
        /// </summary>
        protected string displayInput;

        public MotionInput (string input_, int frameLimit_) {
            // TODO: copy pasted from other constructor
            motionInputs.Add (StringToNumpads (input_));
            frameLimit = frameLimit_;
            displayInput = input_;
        }
        public MotionInput (IList<string> inputs_, int frameLimit_) {
            motionInputs = new List<IList<Numpad>> ();
            foreach (string input in inputs_) {
                motionInputs.Add (StringToNumpads (input));
            }
            frameLimit = frameLimit_;
            displayInput = inputs_[0];
        }

        public override String ToString () {
            return "MotionInput: " + displayInput;
        }

        private IList<Numpad> StringToNumpads (string input) {
            IList<Numpad> numpads = new List<Numpad> ();

            for (int i = input.Length - 1; i >= 0; i--) {
                Numpad nextNumpad = Numpad.N0;
                switch (input[i]) {
                    case '1':
                        nextNumpad = Numpad.N1;
                        break;
                    case '2':
                        nextNumpad = Numpad.N2;
                        break;
                    case '3':
                        nextNumpad = Numpad.N3;
                        break;
                    case '4':
                        nextNumpad = Numpad.N4;
                        break;
                    case '5':
                        nextNumpad = Numpad.N5;
                        break;
                    case '6':
                        nextNumpad = Numpad.N6;
                        break;
                    case '7':
                        nextNumpad = Numpad.N7;
                        break;
                    case '8':
                        nextNumpad = Numpad.N8;
                        break;
                    case '9':
                        nextNumpad = Numpad.N9;
                        break;
                    default:
                        throw new InvalidOperationException (input[i] + " is not an expected Numpad input!");
                }
                numpads.Add (nextNumpad);
            }

            return numpads;
        }
    }

    public class InterpretUtil {
        /// <summary>
        /// Return true if input history matches the numpad inputs given
        /// 
        /// TODO: Make sure this function isn't too slow
        /// </summary>
        /// <param name="inputHistory">The player's most recent inputs</param>
        /// <param name="motionInput">All possible forms of the given motion input</param>
        /// <returns>true if the player inputs match this motion input</returns>
        public static bool InterpretMotionInput (InputHistory inputHistory, MotionInput motionInput) {
            int historyIndex = -1; // which index in the input history to look at?
            Numpad prevInput = Numpad.N0; // most recently interpreted numpad direction
            int curIndex = 0; // which index in the motion inputs to look at?
            int totalFrames = 0; // total frames the input took to input
            bool noMatchesFound = false; // has the input history matched a motion input yet?
            // assume watching all at first
            bool[] notWatching = new bool[motionInput.motionInputs.Count];
            while (!noMatchesFound) {
                // find next input in inputHistory to consider
                historyIndex++;
                if (historyIndex >= inputHistory.GetSize ()) {
                    return false;
                }
                InputHistoryEntry currEntry = inputHistory.GetEntry (historyIndex);
                // add to total frames

                if (currEntry.direction != prevInput) {
                    // new numpad input to investigate!
                    // update prev input for next iteration
                    prevInput = currEntry.direction;
                    for (int i = 0; i < notWatching.Length; i++) {
                        if (notWatching[i] == false) {
                            // still watching this motion input list
                            IList<Numpad> curMotionInput = motionInput.motionInputs[i];
                            if (curIndex == curMotionInput.Count - 1 &&
                                curMotionInput[curIndex] == currEntry.direction) {
                                // curMotionInput was on watch list and is not exhausted
                                // means a match was detected!
                                if (totalFrames <= motionInput.frameLimit) {
                                    // TODO: You can put this check earlier probably!
                                    return true;
                                }
                                notWatching[i] = true;
                            } else if (curMotionInput[curIndex] != currEntry.direction) {
                                // motion input did not match up, don't watch it anymore
                                notWatching[i] = true;
                            }
                        }
                    }
                    // looking for next index on next new direction found
                    curIndex++;
                }

                // are we watching any other motion inputs?
                noMatchesFound = true;
                foreach (bool notWatch in notWatching) {
                    noMatchesFound = noMatchesFound && notWatch;
                }

                if (historyIndex > 0) {
                    // i.e. first input has running frames since last input.
                    // Only factor in if motion input is longer than 1 input (ie. 46A)
                    totalFrames += inputHistory.GetEntry (historyIndex - 1).runningFrames;
                }
            }
            return false;
        }

        /// <summary>
        /// Note, if input history does not contain a button input on the most recent entry,
        /// this will return false.
        /// 
        /// Works only for single button attack inputs (Normals and Specials)

        /// 
        /// TODO: work out the details of button combo interpretations later
        /// </summary>
        /// <param name="inputHistory"></param>
        /// <param name="motionInput"></param>
        /// <returns></returns>
        public static bool InterpretSpecialAttackInput (InputHistory inputHistory, AttackMotionInput motionInput) {
            InputHistoryEntry entry = inputHistory.GetEntry (0);
            IList<ButtonStatus> buttons = entry.buttons;
            ButtonStatus[] reference = motionInput.buttons;

            for (int i = 0; i < buttons.Count; i++) {
                ButtonStatus currStatus = buttons[i];
                if (reference[i] == ButtonStatus.Down &&
                    (currStatus == ButtonStatus.Down || currStatus == ButtonStatus.Release)) {
                    // Down match
                } else if (reference[i] == ButtonStatus.Up) {
                    // Up ignored
                } else {
                    // mismatch!
                    // TODO: might lead to strange behavior currently. i.e. 632AB might not give nothing instead of A DP
                    return false;
                }
            }

            return InterpretMotionInput (inputHistory, motionInput);
        }

        /// <summary>
        /// i.e. won't detect negative edge or holds
        /// </summary>
        /// <param name="inputHistory"></param>
        /// <param name="motionInput"></param>
        /// <returns></returns>
        public static bool InterpretNormalAttackInput (InputHistory inputHistory, AttackMotionInput motionInput) {
            InputHistoryEntry entry = inputHistory.GetEntry (0);
            IList<ButtonStatus> buttons = entry.buttons;
            ButtonStatus[] reference = motionInput.buttons;

            for (int i = 0; i < buttons.Count; i++) {
                ButtonStatus currStatus = buttons[i];
                if (reference[i] == ButtonStatus.Down &&
                    currStatus == ButtonStatus.Down) {
                    // Down match
                } else if (reference[i] == ButtonStatus.Up) {
                    // Up ignored
                } else {
                    // mismatch!
                    // TODO: might lead to strange behavior currently. i.e. 632AB might not give nothing instead of A DP
                    return false;
                }
            }

            return InterpretMotionInput (inputHistory, motionInput);
        }

    }

}


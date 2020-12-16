using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionInput
{
    /// <summary>
    /// Contains multiple definitions of a certain motion input
    /// 
    /// 0th element is last input, last element is first input
    /// 6523 -> 3256, 236 -> 632
    /// </summary>
    /// <typeparam name="Numpad">represent directions</typeparam>
    public IList<IList<Numpad>> motionInputs { get; private set; }

    public int frameLimit { get; private set; }

    public MotionInput(string input, int frameLimit_) {
        // TODO: copy pasted from other constructor
        motionInputs.Add(StringToNumpads(input));
        frameLimit = frameLimit_;
    }
    public MotionInput(IList<string> inputs, int frameLimit_) {
        motionInputs = new List<IList<Numpad>>();
        foreach (string input in inputs) {
            motionInputs.Add(StringToNumpads(input));
        }
        frameLimit = frameLimit_;
    }

    private IList<Numpad> StringToNumpads(string input) {
        IList<Numpad> numpads = new List<Numpad>();

        for (int i = input.Length - 1; i <= 0; i--) {
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
                    throw new InvalidOperationException(input[i] + " is not an expected Numpad input!");
            }
            numpads.Add(nextNumpad);
        }

        return numpads;
    }
}


public class InterpretUtil
{
    /// <summary>
    /// Return true if input history matches the numpad inputs given
    /// 
    /// TODO: Make sure this function isn't too slow
    /// </summary>
    /// <param name="inputHistory">The player's most recent inputs</param>
    /// <param name="motionInput">All possible forms of the given motion input</param>
    /// <returns>true if the player inputs match this motion input</returns>
    public static bool InterpretInput(InputHistory inputHistory, MotionInput motionInput) {
        int historyIndex = -1;  // which index in the input history to look at?
        Numpad prevInput = Numpad.N0;  // most recently interpreted numpad direction
        int curIndex = 0;  // which index in the motion inputs to look at?
        int totalFrames = 0;  // total frames the input took to input
        bool noMatchesFound = false;   // has the input history matched a motion input yet?
        // assume watching all at first
        bool[] notWatching = new bool[motionInput.motionInputs.Count];
        while (!noMatchesFound) {
            // find next input in inputHistory to consider
            historyIndex++;
            InputHistoryEntry currEntry = inputHistory.GetEntry(historyIndex); 
            // add to total frames
            totalFrames += currEntry.runningFrames;

            if (currEntry.direction != prevInput) {
                // new numpad input to investigate!
                // update prev input for next iteration
                prevInput = currEntry.direction;
                for (int i = 0; i < notWatching.Length; i++) {
                    if (notWatching[i] == false) {
                        // still watching this motion input list
                        IList<Numpad> curMotionInput = motionInput.motionInputs[i];
                        if (curIndex > curMotionInput.Count - 1) {
                            // curMotionInput was on watch list and is not exhausted
                            // means a match was detected!
                            if (totalFrames <= motionInput.frameLimit) {
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
        }
        return false;
    }

}

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
    IList<IList<Numpad>> motionInputs { get; private set; }

    public MotionInput(string input) {
        IList<string> inputs = new List<string>();
        inputs.Add(input);
        this(inputs);
    }
    public MotionInput(IList<string> inputs) {
        motionInputs = new List<List<Numpad>>();
        foreach (string input in inputs) {
            motionInputs.Add(StringToNumpads(input));
        }
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
                    break;
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
    /// </summary>
    /// <param name="inputHistory">The player's most recent inputs</param>
    /// <param name="motionInput">All possible forms of the given motion input</param>
    /// <returns>true if the player inputs match this motion input</returns>
    public static bool InterpretInput(InputHistory inputHistory, MotionInput motionInput) {
        int historyIndex = -1;
        Numpad prevInput = Numpad.N0;
        int curIndex = 0;
        int totalFrames = 0;
        boolean matched = false;
        // assume watching all at first
        bool[] notWatching = new bool[motionInput.motionInputs.Count];
        while (!matched) {
            // find next input in inputHistory to consider
            historyIndex++;
            InputHistoryEntry currEntry = inputHistory.GetEntry(historyIndex);
            if (currEntry.direction != prevInput) {
                // new numpad input to investigate!
                prevInput = currEntry.direction;

                for (int i = 0; i < notWatching.Length; i++) {

                }
            }
        }
        return true;
    }

}

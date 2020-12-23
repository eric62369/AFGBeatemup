using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleInput {

    public class InputHistory {
        private int ButtonCount; // A B C D = 4
        private int InputHistorySize; // size for input history

        private IList<Numpad> inputHistory;
        private IList<IList<ButtonStatus>> buttonHistory;
        private IList<int> timeHistory;

        public InputHistory (int inputHistorySize, int buttonCount) {
            ButtonCount = buttonCount;
            InputHistorySize = inputHistorySize;

            inputHistory = new List<Numpad> ();
            buttonHistory = new List<IList<ButtonStatus>> ();
            timeHistory = new List<int> ();

            // initialize input history
            for (int i = 0; i < InputHistorySize; i++) {
                inputHistory.Add (Numpad.N0);
                timeHistory.Add (0);

                IList<ButtonStatus> emptyButtons = new List<ButtonStatus> ();
                for (int j = 0; j < ButtonCount; j++) {
                    emptyButtons.Add (ButtonStatus.Up);
                }
                buttonHistory.Add (emptyButtons);
            }
        }

        /// Add new entry to inputhistory (numpad direction, button states, and frames since last input)
        public void AddNewEntry (
            Numpad direction,
            IList<ButtonStatus> buttons,
            int runningFrames
        ) {
            inputHistory.Insert (0, direction);
            buttonHistory.Insert (0, buttons);
            timeHistory.Insert (0, runningFrames);

            // trim input history size
            if (inputHistory.Count > InputHistorySize) {
                inputHistory.RemoveAt (InputHistorySize);
                buttonHistory.RemoveAt (InputHistorySize);
                timeHistory.RemoveAt (InputHistorySize);
            }
        }

        /// <summary>
        /// Get input history entry. 0 is most recent history entry
        /// </summary>
        /// <param name="index">0 is most recent history entry, errors if out of bounds index given</param>
        /// <returns>bundled InputHistoryEntry, read only</returns>
        public InputHistoryEntry GetEntry (int index) {
            return new InputHistoryEntry (
                inputHistory[index],
                buttonHistory[index],
                timeHistory[index]
            );
        }

        public int GetSize () {
            return inputHistory.Count;
        }

        public override string ToString () {
            string result = "InputHistory:\n";
            for (int i = 0; i < InputHistorySize; i++) {
                InputHistoryEntry entry = new InputHistoryEntry (
                    inputHistory[i],
                    buttonHistory[i],
                    timeHistory[i]
                );
                result += "[" + entry.ToString () + "]\n";
            }
            return result;
        }

    }

    public class InputHistoryEntry {
        public Numpad direction { get; private set; }
        /// <summary>
        /// 0 is A, last index is D
        /// </summary>
        /// <value></value>
        public IList<ButtonStatus> buttons { get; private set; } // TODO: Make this immutable list?
        public int runningFrames { get; private set; }

        public InputHistoryEntry (
            Numpad direction_,
            IList<ButtonStatus> buttons_,
            int runningFrames_) {
            direction = direction_;
            buttons = buttons_;
            runningFrames = runningFrames_;
        }

        public override string ToString () {
            string result = "";
            result += direction + " ";

            result += "(";
            if (buttons.Count != 0) {
                result += ButtonStatusToString (buttons[0]);
            }
            for (int i = 1; i < buttons.Count; i++) {
                result += "," + ButtonStatusToString (buttons[i]);
            }
            result += ") ";

            result += runningFrames;

            return result;
        }
        private string ButtonStatusToString (ButtonStatus button) {
            switch (button) {
                case ButtonStatus.Down:
                    return "Dwn";
                case ButtonStatus.Hold:
                    return "Hld";
                case ButtonStatus.Release:
                    return "Rls";
                case ButtonStatus.Up:
                    return "Up_";
                default:
                    throw new InvalidOperationException (button + " was not expected button status!");
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHistory
{
    private int ButtonCount; // A B C D = 4
    private int InputHistorySize; // size for input history

    private IList<Numpad> inputHistory;
    private IList<IList<ButtonStatus>> buttonHistory;
    private IList<int> timeHistory;

    public InputHistory(int inputHistorySize, int buttonCount) {
        ButtonCount = buttonCount;
        InputHistorySize = inputHistorySize;

        inputHistory = new List<Numpad>();
        buttonHistory = new List<IList<ButtonStatus>>();
        timeHistory = new List<int>();

        // initialize input history
        for (int i = 0; i < InputHistorySize; i++)
        {
            inputHistory.Add(Numpad.N0);
            timeHistory.Add(0);

            IList<ButtonStatus> emptyButtons = new List<ButtonStatus>();
            for (int j = 0; j < ButtonCount; j++) {
                emptyButtons.Add(ButtonStatus.Up);
            }
            buttonHistory.Add(emptyButtons);
        }
    }

    /// Add new entry to inputhistory (numpad direction, button states, and frames since last input)
    public void AddNewEntry(
        Numpad direction,
        IList<ButtonStatus> buttons,
        int runningFrames
        ) {
        inputHistory.Insert(0, direction);
        buttonHistory.Insert(0, buttons);
        timeHistory.Insert(0, runningFrames);

        // trim input history size
        if (inputHistory.Count > InputHistorySize)
        {
            inputHistory.RemoveAt(InputHistorySize);
            buttonHistory.RemoveAt(InputHistorySize);
            timeHistory.RemoveAt(InputHistorySize);
        }
    }

    /// <summary>
    /// Get input history entry. 0 is most recent history entry
    /// </summary>
    /// <param name="index">0 is most recent history entry, errors if out of bounds index given</param>
    /// <returns>bundled InputHistoryEntry, read only</returns>
    public InputHistoryEntry GetEntry(int index) {
        return new InputHistoryEntry(
            inputHistory[index],
            buttonHistory[index],
            timeHistory[index]
        );
    }

    public int GetSize() {
        return inputHistory.Count;
    }

}

public class InputHistoryEntry
{
    public Numpad direction { get; private set; }
    public IList<ButtonStatus> buttons { get; private set; }  // TODO: Make this immutable list?
    public int runningFrames { get; private set; }

    public InputHistoryEntry(
        Numpad direction_,
        IList<ButtonStatus> buttons_,
        int runningFrames_)
    {
        direction = direction_;
        buttons = buttons_;
        runningFrames = runningFrames_;
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RefillClockAttribute {
    HitStun,
    Refillable,
    Interruptable,
    Uninterruptable
}
// public class RefillClockEventArgs : EventArgs
// {
//     public int frames { get; set; }
//     public RefillClockAttribute attribute { get; set; }
// }

public class RefillClock
{
    /// <summary>
    /// Event called on new clock fill
    /// </summary>
    // public event EventHandler<RefillClockEventArgs> ClockFill;

    /// <summary>
    /// Event called on empty clock or interrupt
    /// </summary>
    public event EventHandler ClockEmpty;

    private Action update;

    /// <summary>
    /// Frames until hitstop ends
    /// </summary>
    private int framesLeft;

    public readonly RefillClockAttribute clockAttribute;

    public RefillClock() {
        // update = Update;
        UpdateScript.AddUpdateCallback(Update);
    }

    // Update is called once per frame
    private void Update()
    {
        if (framesLeft > 0) {
            framesLeft--;
            if (framesLeft <= 0) {
                ClockEmpty?.Invoke(this, EventArgs.Empty);
                Debug.Log("Clock Emptied");
            }
        }
    }

    public void ResetClock(int frames) {
        if (framesLeft <= 0) {
            framesLeft = frames;
            Debug.Log("Clock Reset");
            // RefillClockEventArgs args = new RefillClockEventArgs();
            //     args.frames = frames;
            //     args.attribute = attribute;
            //     OnThresholdReached(args);
        }
    }
}

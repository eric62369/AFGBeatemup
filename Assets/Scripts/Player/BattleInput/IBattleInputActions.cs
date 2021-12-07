using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleInput {
    /// <summary>
    /// Interface for battle actions a character can perform
    /// </summary>
    public interface IBattleInputActions {
        // Universal Movement
        void Dash ();
        // void AirDash (bool direction);
        void BackDash ();
        // void Run ();
        void StopRun();
        void Walk (Numpad direction);
        void StopWalk();
        void Jump (Numpad direction);
        void ReleaseJump();

        // Universal Actions
        void Throw (bool direction);
        void InputBufferCancel(int frameLimit);
        void RC(int frameLimit);

        // Normals
        // void N5 (Button button);

        // Command Normals

        // Specials
        // void S236 (Button button);
    }

}
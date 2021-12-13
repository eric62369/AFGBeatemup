using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleInput {
    public enum Button {
        None,
        A, // Light
        B, // Medium
        C, // Heavy
        D // Unique
    }
    public class ControllerReader : MonoBehaviour, IController {
        /*
         New module for directly interfacing with:
        - Player N instance (Hooks via PlayerStateManager interface)
        - Physical Game Controllers (Using Unity Input system / Input Actions)
        - Has no concept of what actions the character can do, only input manager events
        */

        /*
        This script will be attached to a PlayerInputModule GameObject
        - The PlayerInputModule GameObject will attach to whatever character matches with the
            playerUnityInput playerIndex is, through the playerStateManager
        */

        public float DeadZone; // square for no input detection

        private PlayerInput playerUnityInput;
        // private IStateManager playerStateManager;
        // private PlayerInputManager playerInputManager;
        private BattleInputScanner scanner;
        // public StickVisualizerController stickVisualizer;

        // Start is called before the first frame update

        // Follows the pattern that IGame needs inputs in
        private long currentInputs;

        private void Awake () {
            playerUnityInput = GetComponent<PlayerInput> ();
            int playerIndex = playerUnityInput.playerIndex;
            // IEnumerable<IStateManager> stateManagers =
            //     (IEnumerable<IStateManager>) FindObjectsOfType<IStateManager> ();
            // playerStateManager = stateManagers.FirstOrDefault (s => s.GetPlayerIndex () == playerIndex);
            // scanner = playerStateManager.GetInputScanner ();

            IEnumerable<BattleInputScanner> scanners =
                (IEnumerable<BattleInputScanner>) FindObjectsOfType<BattleInputScanner>();
            scanner = scanners.First();
        }

        public long GetCurrentInput() {
            return currentInputs;
        }

        //////////////////
        // Input Manager Events
        //////////////////
        private void OnMove (InputValue value) {
            Vector2 input = value.Get<Vector2> ();
            Numpad newInput = GetInputToNumpad (input.x, input.y);
            // playerInputManager.InterpretNewStickInput(newInput);
            scanner.InterpretNewStickInput (newInput);
            // TODO: stickVisualizer.UpdateStickUI(newInput);
        }
        private void OnA (InputValue value) {
            scanner.InterpretNewButtonInput (Button.A, value.isPressed);
        }
        private void OnB (InputValue value) {
            scanner.InterpretNewButtonInput (Button.B, value.isPressed);
        }
        private void OnC (InputValue value) {
            scanner.InterpretNewButtonInput (Button.C, value.isPressed);
        }
        private void OnD (InputValue value) {
            scanner.InterpretNewButtonInput (Button.D, value.isPressed);
        }
        private void OnStart () {
            MenuController menu = (MenuController) FindObjectOfType (typeof (MenuController));
            menu.PauseGame ();
        }

        // Will always return P1 side style inputs
        // (i.e. P2 side 4 input is translated to -> 6)
        private Numpad GetInputToNumpad (float x, float y) {
            x = DeadZoneInput (x);
            y = DeadZoneInput (y);
            // TODO: There's a better way, pls fix this
            if (true){//playerStateManager.GetCurrentFacingDirection ()) {
                if (x == -1) {
                    if (y == -1) {
                        return Numpad.N1;
                    } else if (y == 1) {
                        return Numpad.N7;
                    } else if (y == 0) {
                        return Numpad.N4;
                    }
                } else if (x == 1) {
                    if (y == -1) {
                        return Numpad.N3;
                    } else if (y == 1) {
                        return Numpad.N9;
                    } else if (y == 0) {
                        return Numpad.N6;
                    }
                } else if (x == 0) {
                    if (y == -1) {
                        return Numpad.N2;
                    } else if (y == 1) {
                        return Numpad.N8;
                    } else if (y == 0) {
                        return Numpad.N5;
                    }
                } else {
                    throw new InvalidOperationException (x + " is not -1, 0, 1 for input");
                }
                throw new InvalidOperationException (y + " is not -1, 0, 1 for input");
            } else // Left facing (P2 side)
            {
                if (x == -1) {
                    if (y == -1) {
                        return Numpad.N3;
                    } else if (y == 1) {
                        return Numpad.N9;
                    } else if (y == 0) {
                        return Numpad.N6;
                    }
                } else if (x == 1) {
                    if (y == -1) {
                        return Numpad.N1;
                    } else if (y == 1) {
                        return Numpad.N7;
                    } else if (y == 0) {
                        return Numpad.N4;
                    }
                } else if (x == 0) {
                    if (y == -1) {
                        return Numpad.N2;
                    } else if (y == 1) {
                        return Numpad.N8;
                    } else if (y == 0) {
                        return Numpad.N5;
                    }
                } else {
                    throw new InvalidOperationException (x + " is not -1, 0, 1 for input");
                }
                throw new InvalidOperationException (y + " is not -1, 0, 1 for input");
            }
        }

        // Apply the deadzone to the given input
        private float DeadZoneInput (float x) {
            float absx = System.Math.Abs (x);
            if (absx < DeadZone) {
                x = 0;
            } else {
                x /= absx;
            }
            return x;
        }
    }

}
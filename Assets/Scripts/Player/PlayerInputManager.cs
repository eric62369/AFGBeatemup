using System;
using System.Collections.Generic;
using UnityEngine;

public enum Numpad
{
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
public enum Button
{
    None, // No buttons
    A, // Light
    B, // Medium
    C, // Heavy
    D // Unique
}
public enum Direction
{
    Down, // i.e. 1, 2, 3  are all down inputsInterpretInput();
    Left,
    Up,
    Right
}

public class PlayerInputManager : MonoBehaviour
{
    public int InputHistorySize; // size for input history
    public float DeadZone; // square for no input detection
    public float Time66; // in (ms) window to input 66 (dash)
    public float Time236; // in (ms) window to input 236
    private PlayerMovementController playerMovement;
    private PlayerAttackController playerAttack;
    private PlayerStateManager playerState;
    private Numpad currentInput; // Current stick input in Numpad
    private float runningTime; // How much time (in ms) since last input?
    private IList<Numpad> inputHistory;
    private IList<float> timeHistory;
    private bool A;
    private bool B;
    private bool C;
    private bool D;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovementController>();
        playerAttack = GetComponent<PlayerAttackController>();
        playerState = GetComponent<PlayerStateManager>();
        currentInput = Numpad.N0;
        runningTime = 0;
        inputHistory = new List<Numpad>();
        timeHistory = new List<float>();

        for (int i = 0; i < InputHistorySize; i++)
        {
            inputHistory.Add(Numpad.N0);
            timeHistory.Add(0);
        }
    }
    public void Update()
    {
        Numpad newInput = GetCurrentInput();
        runningTime += Time.deltaTime;

        InterpretMovement();
        // New input detected!
        if (newInput != currentInput)
        {
            currentInput = newInput;
            inputHistory.Insert(0, currentInput);
            timeHistory.Insert(0, runningTime);
            runningTime = 0;
            if (inputHistory.Count > InputHistorySize)
            {
                inputHistory.RemoveAt(InputHistorySize);
                timeHistory.RemoveAt(InputHistorySize);
            }
            InterpretDash();
        }
        InterpretButtons();
    }
    private void InterpretMovement()
    {
        Numpad firstInput = inputHistory[0];
        Numpad secondInput = inputHistory[1];

        if (IsNumpadUp(firstInput))
        {
            playerMovement.Jump(firstInput);
        }
        else if (!playerMovement.isRunning && (firstInput == Numpad.N6 || firstInput == Numpad.N4))
        {
            // Walk check
            playerMovement.Walk(firstInput);
        }
        else if (playerMovement.isRunning && (firstInput == Numpad.N6))
        {
            // Holding run check
            playerMovement.Run(firstInput);
        }
        else
        {
            if (playerMovement.isRunning)
            {
                playerMovement.Skid();
            }
            // Nothing / Idle
        }

        if (!IsNumpadUp(firstInput))
        {
            playerMovement.setIsHoldingJump(false);
        }
    }
    private bool IsNumpadUp(Numpad num)
    {
        return num == Numpad.N7 || num == Numpad.N8 || num == Numpad.N9;
    }

    private void InterpretDash()
    {
        Numpad firstInput = inputHistory[0];
        Numpad secondInput = inputHistory[1];
        Numpad thirdInput = inputHistory[2];
        float firstTime = timeHistory[0];
        float secondTime = timeHistory[1];

        if (!playerAttack.isAttacking)
        {
            if (firstInput == Numpad.N6 && secondInput == Numpad.N5 && (thirdInput == Numpad.N6 || thirdInput == Numpad.N9))
            {
                if (firstTime + secondTime <= Time66)
                {
                    if (playerMovement.isGrounded)
                    {
                        // Grounded forward step dash
                        playerMovement.Dash(firstInput);
                    }
                    else
                    {
                        // forward airdash
                        playerMovement.AirDash(true);
                    }
                }
            }
            else if (firstInput == Numpad.N4 && secondInput == Numpad.N5 && (thirdInput == Numpad.N4 || thirdInput == Numpad.N7))
            {
                if (firstTime + secondTime <= Time66)
                {
                    if (playerMovement.isGrounded)
                    {
                        // Grounded backdash
                        playerMovement.BackDash(firstInput);
                    }
                    else
                    {
                        // back airdash
                        playerMovement.AirDash(false);
                    }
                }
            }
        }
    }
    private void InterpretSpecial(Button button)
    {
        Numpad firstInput = inputHistory[0];
        Numpad secondInput = inputHistory[1];
        Numpad thirdInput = inputHistory[2];
        Numpad fourthInput = inputHistory[3];
        float firstTime = timeHistory[0];
        float secondTime = timeHistory[1];
        float thirdTime = timeHistory[2];
        // 236 or 236? motion
        if (thirdInput == Numpad.N2 && secondInput == Numpad.N3 && firstInput == Numpad.N6)
        {
            // Motion detected!
            if (firstTime + runningTime <= Time236)
            {
                if (button == Button.A)
                {
                    Debug.Log("Stun Edge!");
                }
            }
        }
        else if (fourthInput == Numpad.N2 && thirdInput == Numpad.N3 && secondInput == Numpad.N6) 
        {
            // Motion detected!
            if (secondTime + firstTime + runningTime <= Time236)
            {
                if (button == Button.A)
                {
                    Debug.Log("Stun Edge Extra!");
                }
            }
        }
        else
        {
            // Normals
            if (button == Button.A)
            {
                playerAttack.Attack5B();
            }
            else if (button == Button.B)
            {
                playerAttack.Attack5C();
            }
        }
    }
    private void InterpretButtons()
    {
        bool curA = Input.GetButton("Fire1");
        bool curB = Input.GetButton("Fire2");
        bool curC = Input.GetButton("Fire3");
        // bool curD = Input.GetButton("Fire4");

        if (curA == true && A == false) {
            InterpretSpecial(Button.A);
        }
        if (curB == true && B == false) {
            InterpretSpecial(Button.B);
        }


        A = curA;
        B = curB;
        C = curC;
    }

    // Will always return P1 side style inputs
    // (i.e. P2 side 4 input is translated to -> 6)
    public Numpad GetCurrentInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        x = deadZoneInput(x);
        y = deadZoneInput(y);
        // TODO: There's a better way, pls fix this
        if (playerState.GetCurrentFacingDirection())
        {
            if (x == -1)
            {
                if (y == -1)
                {
                    return Numpad.N1;
                }
                else if (y == 1) 
                {
                    return Numpad.N7;
                }
                else if (y == 0)
                {
                    return Numpad.N4;
                }
            }
            else if (x == 1) 
            {
                if (y == -1)
                {
                    return Numpad.N3;
                }
                else if (y == 1) 
                {
                    return Numpad.N9;
                }
                else if (y == 0)
                {
                    return Numpad.N6;
                }
            }
            else if (x == 0)
            {
                if (y == -1)
                {
                    return Numpad.N2;
                }
                else if (y == 1) 
                {
                    return Numpad.N8;
                }
                else if (y == 0)
                {
                    return Numpad.N5;
                }
            }
            else
            {
                throw new InvalidOperationException(x + " is not -1, 0, 1 for input");
            }
            throw new InvalidOperationException(y + " is not -1, 0, 1 for input");
        }
        else  // Left facing (P2 side)
        {
            if (x == -1)
            {
                if (y == -1)
                {
                    return Numpad.N3;
                }
                else if (y == 1) 
                {
                    return Numpad.N9;
                }
                else if (y == 0)
                {
                    return Numpad.N6;
                }
            }
            else if (x == 1) 
            {
                if (y == -1)
                {
                    return Numpad.N1;
                }
                else if (y == 1) 
                {
                    return Numpad.N7;
                }
                else if (y == 0)
                {
                    return Numpad.N4;
                }
            }
            else if (x == 0)
            {
                if (y == -1)
                {
                    return Numpad.N2;
                }
                else if (y == 1) 
                {
                    return Numpad.N8;
                }
                else if (y == 0)
                {
                    return Numpad.N5;
                }
            }
            else
            {
                throw new InvalidOperationException(x + " is not -1, 0, 1 for input");
            }
            throw new InvalidOperationException(y + " is not -1, 0, 1 for input");
        }
    }

    // Apply the deadzone to the given input
    private float deadZoneInput(float x)
    {
        float absx = System.Math.Abs(x);
        if (absx < DeadZone)
        {
            x = 0;
        }
        else
        {
            x /= absx;
        }
        return x;
    }
}

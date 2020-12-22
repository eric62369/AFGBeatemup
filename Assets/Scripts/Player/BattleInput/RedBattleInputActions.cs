using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleInput
{
public class RedBattleInputActions : MonoBehaviour, IBattleInputActions
{
    private PlayerAttackController playerAttack;

    void Start() {
        playerAttack = GetComponent<PlayerAttackController>();
    }

    void Update() {

    }

    // Universal Movement
    public void Dash() {}
    public void AirDash(bool direction) {}
    public void BackDash() {}
    public void Run() {}
    public void Skid() {}
    public void Walk() {}
    public void Jump() {}

    // Universal Actions
    public void Throw(bool direction) {}


    // Normals
    public void N5(Button button) {
        switch (button) {
            case Button.A:
                break;
            case Button.B:
                playerAttack.Attack5B();
                break;
            case Button.C:
                playerAttack.Attack5C();
                break;
            case Button.D:
                break;
            default:
                throw new InvalidOperationException(button + " was not an expected normal button!");
        }
        
    }

    // Command Normals

    // Specials
    public void S236(Button button) {}
}
}
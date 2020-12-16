using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBattleInputActions : MonoBehaviour, IBattleInputActions
{
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
    public void N5(Button button) {}

    // Command Normals

    // Specials
    public void S236(Button button) {}
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using BattleInput;
// using System.Threading.Tasks;

public class LandEventArgs {

}


public interface IMovementController
{
    void StartForwardDash();
    void StartBackwardDash();
    void Skid();
    void Walk(Numpad direction);
    void StopWalk();
    Numpad PrevJumpInput { get; set; }
    bool isGrounded { get; }
    // bool isHoldingJump { get; private set; }
    bool hasNotUsedJump { get; set; }
//     bool isGrounded { get; }
    
//     float xPosition { get; }

//     event Land LandEvent;

//     Task TriggerHitStun(Attack attackData);
//     Task TriggerBlockStun(Attack attackData);

//     void Pushback(Vector2 force);

//     Vector2 FreezeCharacter();
//     void UnFreezeCharacter(Vector2 oldVelocity);

//     // Useful for defeat animation
//     void HighLaunch();
//     void Launch(int direction);

//     void HeavyLaunch(int direction);
//     void Dunk(int direction);
}

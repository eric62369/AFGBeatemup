using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public delegate void GetHit(object sender, GetHitEventArgs args);

public interface IMovementController
{
    bool isGrounded { get; }
    
    float xPosition { get; }

    event GetHit GetHitEvent;

    Task TriggerHitStun(Attack attackData);
    Task TriggerBlockStun(Attack attackData);

    void Pushback(Vector2 force);

    Vector2 FreezeCharacter();
    void UnFreezeCharacter(Vector2 oldVelocity);

    // Useful for defeat animation
    void HighLaunch();
}

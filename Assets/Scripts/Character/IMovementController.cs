using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public interface IMovementController
{
    Task TriggerHitStun(Attack attackData);

    void Pushback(Vector2 force);

    Vector2 FreezeCharacter();
}

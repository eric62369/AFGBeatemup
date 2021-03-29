using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateManager
{
    bool isBlocking {
        get;
    }

    int GetPlayerIndex();
    Vector3 GetCurrentPosition();
    PlayerAttackController GetAttackController();
    void ThrowHit();
    float GetThrowPositionOffset();
    bool GetIsP1Side();
    void TurnCharacterAround();
}

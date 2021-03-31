using System.Collections;
using System.Collections.Generic;

public enum AttackType
{
    Strike,
    Launcher,
    Throw,
    AntiAirThrow,
    Dunk,
    HeavyLauncher
}

public class Attack
{
    public int Damage { get; private set; }
    public int Level { get; private set; }
    /// Example: P1-5B
    public string Id { get; private set; }
    public AttackType Type { get; private set; }
    public IMovementController movement;
    public PlayerStateManager playerState;

    /// Defaults to strike attack type
    public Attack(string attackId, int attackLevel, int attackDamage, IMovementController _movement, PlayerStateManager _playerState)
    {
        Id = attackId;
        Level = attackLevel;
        Damage = attackDamage;
        movement = _movement;
        playerState = _playerState;
        Type = AttackType.Strike;
    }
    /// Takes in an attack type also
    public Attack(string attackId, int attackLevel, int attackDamage, AttackType attackType, IMovementController _movement, PlayerStateManager _playerState)
    {
        Id = attackId;
        Level = attackLevel;
        Damage = attackDamage;
        movement = _movement;
        playerState = _playerState;
        Type = attackType;
    }

    /// A force number  
    public int GetPushback()
    {
        return AttackConstants.AttackLevelPushback[Level];
    }

    /// hitstun is in ms
    // public float GetHitstun()
    // {
    //     return AttackConstants.AttackLevelStunSpeed[Level];
    // }

    public float GetStunSpeed()
    {
        return AttackConstants.AttackLevelStunSpeed[Level];
    }

    /// hitstop is in ms
    public int GetHitStop()
    {
        return AttackConstants.AttackLevelHitStop[Level];
    }

    /// return 1 for P1Side, -1 for P2Side
    public int GetPushBackDirection(float victimXPosition){
        if (GetAttackerPosition() - victimXPosition >= 0) {
            return -1;
        } else {
            return 1;
        }
    }
    private float GetAttackerPosition()
    {   
        return movement.xPosition;
    }
}

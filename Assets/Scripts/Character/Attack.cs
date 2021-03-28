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
    public IStateManager playerState;

    /// Defaults to strike attack type
    public Attack(string attackId, int attackLevel, int attackDamage, IStateManager _playerState)
    {
        Id = attackId;
        Level = attackLevel;
        Damage = attackDamage;
        playerState = _playerState;
        Type = AttackType.Strike;
    }
    /// Takes in an attack type also
    public Attack(string attackId, int attackLevel, int attackDamage, AttackType attackType, IStateManager _playerState)
    {
        Id = attackId;
        Level = attackLevel;
        Damage = attackDamage;
        playerState = _playerState;
        Type = attackType;
    }

    /// A force number
    public int GetPushback()
    {
        return AttackConstants.AttackLevelPushback[Level];
    }

    /// histun is in ms
    public int GetHitstun()
    {
        return AttackConstants.AttackLevelHitStun[Level];
    }

    /// hitstop is in ms
    public int GetHitStop()
    {
        return AttackConstants.AttackLevelHitStop[Level];
    }

    /// return 1 for P1Side, -1 for P2Side
    public int GetPushBackDirection()
    {
        if(playerState.GetIsP1Side())
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}

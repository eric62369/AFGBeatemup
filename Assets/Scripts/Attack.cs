using System.Collections;
using System.Collections.Generic;

public class Attack
{
    public int Damage { get; private set; }
    public int Level { get; private set; }
    /// Example: P1-5B
    public string Id { get; private set; }

    public Attack(string attackId, int attackLevel, int attackDamage)
    {
        Id = attackId;
        Level = attackLevel;
        Damage = attackDamage;
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
}

using System.Collections;
using System.Collections.Generic;

public class AttackConstants
{
    /// AttackLevel to Pushback Force
    public static readonly int[] AttackLevelPushback = { 350, 400, 450, 500 };

    /// AttackLevel to Hit / Block stun time (ms)
    public static readonly int[] AttackLevelHitStun = { 150, 250, 350, 500 };

    /// AttackLevel to HitStop in (ms)
    public static readonly int[] AttackLevelHitStop = { 150, 250, 350, 500 };

    /// {X, Y} Force in ForceMode
    public static readonly int[] LightLaunchForce = { 5, 20 }; // 200 500
}

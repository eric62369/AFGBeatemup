using System.Collections;
using System.Collections.Generic;

public class AttackConstants
{
    /// AttackLevel to Pushback Force
    public static readonly int[] AttackLevelPushback = { 10, 10, 10, 10, 10 };

    /// AttackLevel to Hit / Block stun time (ms),  animation version in animation speed (f)
    // public static readonly int[] AttackLevelHitStun = { 150, 250, 350, 500, 700 };
    public static readonly float[] AttackLevelStunSpeed = { 1f, 0.8f, 0.6f, 0.5f, 0.1f };

    /// AttackLevel to HitStop in (ms)
    public static readonly int[] AttackLevelHitStop = { 116, 116, 133, 150, 166 };

    /// AttackLevel to Counter Hit HitStop in (ms)
    // public static readonly int[] AttackLevelCHHitStop = { 116, 116, 133.3, 150, 166.6 };
    public static readonly int CHHitstopModifier = 2;

    /// {X, Y} Force in ForceMode
    public static readonly int[] LightLaunchForce = { 1, 15 }; // 200 500

    /// {X, Y} Force in ForceMode
    public static readonly int[] HeavyLaunchForce = { 8, 15 }; // 200 500

    /// {X, Y} Force in ForceMode
    public static readonly int[] DunkForce = { 2, -10 }; // 200 500
}

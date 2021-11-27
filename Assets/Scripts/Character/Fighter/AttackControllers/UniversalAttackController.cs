using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public enum CancelAction
{
    Jump,
    Attack
}

/// <summary>
/// Hub for all combat related resources
/// </summary>
public class UniversalAttackController : IAttackController {
    // Should be a universal feature: No fighting entity goes without hitstop
    private HitStopTracker InternalHitStopTracker;
    
}

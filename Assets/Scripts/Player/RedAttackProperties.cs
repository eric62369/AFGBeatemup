using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RedAttackProperties", menuName = "RedAttackProperties", order = 0)]
public class RedAttackProperties : ScriptableObject {
    /// Animation Parameter names
    private ISet<string> JumpCancellable;

    public void OnEnable()
    {
        InitializeJumpCancellable();
    }

    private void InitializeJumpCancellable()
    {
        JumpCancellable = new HashSet<string>();
        JumpCancellable.Add("5B");
        JumpCancellable.Add("J5A");
        JumpCancellable.Add("J5B");
    }

    public bool CanJumpCancel(string move) {
        return JumpCancellable.Contains(move);
    }
}

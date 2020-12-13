using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RedAttackProperties", menuName = "RedAttackProperties", order = 0)]
public class RedAttackProperties : ScriptableObject {
    /// Animation Parameter names
    public ISet<string> JumpCancellable { get; private set; }

    public void OnEnable()
    {
        Debug.Log("OnEnable");
        InitializeJumpCancellable();
    }

    private void InitializeJumpCancellable()
    {
        JumpCancellable = new HashSet<string>();
        JumpCancellable.Add("5B");
    }

    public bool CanJumpCancel(string move) {
        return JumpCancellable.Contains(move);
    }
}

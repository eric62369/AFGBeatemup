using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAttackProperties : MonoBehaviour
{
    /// Animation Parameter names
    public static ISet<string> JumpCancellable { get; private set; }

    void Start()
    {
        InitializeJumpCancellable();
    }

    private void InitializeJumpCancellable()
    {
        JumpCancellable = new HashSet<string>();
        JumpCancellable.Add("5B");
    }
}

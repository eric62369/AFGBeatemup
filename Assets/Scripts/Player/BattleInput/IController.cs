using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleInput {
    public interface IController {
        long GetCurrentInput();
    }
}
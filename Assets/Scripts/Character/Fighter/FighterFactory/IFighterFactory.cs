using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public interface IFighterFactory
    {
        IFighterData build();
    }
}

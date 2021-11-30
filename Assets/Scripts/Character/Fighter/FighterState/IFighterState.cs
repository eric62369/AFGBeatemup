using System;

namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public interface IFighterState
    {
        bool isGrounded { get; set; }
    }
}
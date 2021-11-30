using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterType {
    PlayerFighter,
    EnemyFighter,
    EnemySubFighter
}

namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public class FighterFactory : IFighterFactory
    {
        private IAttackController attackController;
        private ICrossFighterCommunication fighterComms;
        private IMovementModule movementModule;

        public FighterFactory()
        {
            attackController = new UniversalAttackController();
            fighterComms = new CrossFighterCommunication();
            movementModule = new MovementModule();
        }

        private void getHitEvents() {
            fighterComms.GetHitEvent += movementModule.PushBackGetHit;
            fighterComms.SendHitEvent += movementModule.PushBackSendHit;
        }

        public IFighterData build() {
            // a
            IFighterData compile = new FighterData(attackController, fighterComms, movementModule);
            return compile;
        }
    }
}

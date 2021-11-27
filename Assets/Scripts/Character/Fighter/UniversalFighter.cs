using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public class UniversalFighter : MonoBehaviour, IFighter
    {
        private IAttackController attackController;
        private ICrossFighterCommunication fighterComms;
        private IFighterState fighterState;
        private IMovementModule movementModule;

        // Start is called before the first frame update
        void Start()
        {
            IFighterFactory fighterFactory = new FighterFactory();
            fighterFactory.
            fighterFactory.build();
            attackController = fighterFactory.getAttackController();
        }


        ///////////////////////
        // TODO: Create FighterFactory to set up event listeners?
        ///////////////////////
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

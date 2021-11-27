namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public class FighterData : IFighterData
    {
        private IAttackController attackController;
        private ICrossFighterCommunication crossFighterCommunication;
        private IMovementModule movementModule;

        public FighterData(
                IAttackController attackController,
                ICrossFighterCommunication crossFighterCommunication,
                IMovementModule movementModule) {
            this.attackController = attackController;
            this.crossFighterCommunication = crossFighterCommunication;
            this.movementModule = movementModule;
        }
        public IAttackController GetAttackController() {
            return attackController;
        }
        public ICrossFighterCommunication GetCrossFighterCommunication() {
            return crossFighterCommunication;
        }
        public IMovementModule GetMovementModule() {
            return movementModule;
        }
    }
}

namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public class SendHitEventArgs
    {
        public Attack attackData { get; private set; }

        public float victimXPosition { get; private set; }

        public SendHitEventArgs(Attack attackData_, float victimXPosition_) {
            attackData = attackData_;
            victimXPosition = victimXPosition_;
        }
    }

    public class GetHitEventArgs
    {
        public Attack attackData { get; private set; }

        public GetHitEventArgs(Attack attackData_) {
            attackData = attackData_;
        }
    }

    public delegate void SendHit(object sender, SendHitEventArgs args);
    public delegate void GetHit(object sender, GetHitEventArgs args);

    public interface ICrossFighterCommunication
    {
        
        event SendHit SendHitEvent;
        event GetHit GetHitEvent;
        void OnOtherFighterStrike(Attack AttackData, float victimXPosition);
    }
}
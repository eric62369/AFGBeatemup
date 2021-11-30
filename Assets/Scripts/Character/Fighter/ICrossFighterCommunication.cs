namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public class SendHitEventArgs
    {
        public Attack attackData { get; private set; }
        public float victimXPosition { get; private set; }
        public bool isGrounded { get; private set; }
        public SendHitEventArgs(Attack attackData_, float victimXPosition_, bool isGrounded_) {
            attackData = attackData_;
            victimXPosition = victimXPosition_;
            isGrounded = isGrounded_;
        }
    }

    public class GetHitEventArgs
    {
        public Attack attackData { get; private set; }
        public float xPosition { get; private set; }
        public bool isGrounded { get; private set; }
        public GetHitEventArgs(Attack attackData_, float xPosition_, bool isGrounded_) {
            attackData = attackData_;
            xPosition = xPosition_;
            isGrounded = isGrounded_;
        }
    }

    public delegate void SendHit(object sender, SendHitEventArgs args);
    public delegate void GetHit(object sender, GetHitEventArgs args);

    public interface ICrossFighterCommunication
    {       
        event SendHit SendHitEvent;
        event GetHit GetHitEvent;
        void OnFighterSendHit(Attack AttackData, float victimXPosition, bool isGrounded);
        void OnFighterGetHit(Attack AttackData, float senderXPosition, bool isGrounded);
    }
}
namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public class CrossFighterCommunication : ICrossFighterCommunication
    {
        public event SendHit SendHitEvent;
        public event GetHit GetHitEvent;

        public CrossFighterCommunication() {
        }

        // Call from someone who needs to raise this event
        public void OnFighterSendHit(Attack AttackData, float victimXPosition, bool isGrounded) {
            RaiseSendHitEvent(new SendHitEventArgs(AttackData, victimXPosition, isGrounded));
        }
        // Call from someone who needs to raise this event
        public void OnFighterGetHit(Attack AttackData, float senderXPosition, bool isGrounded) {
            RaiseGetHitEvent(new GetHitEventArgs(AttackData, senderXPosition, isGrounded));
        }

        // 
        protected virtual void RaiseSendHitEvent(SendHitEventArgs e) {
            SendHit raiseEvent = SendHitEvent;
            if (raiseEvent != null) {
                raiseEvent(this, e);
            }
        }

        protected virtual void RaiseGetHitEvent(GetHitEventArgs e) {
            GetHit raiseEvent = GetHitEvent;
            if (raiseEvent != null) {
                raiseEvent(this, e);
            }
        }
    }
}
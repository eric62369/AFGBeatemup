namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public interface IMovementModule
    {
        // Standard movements
        void Jump(object sender, JumpEventArgs e);
        void Walk();
        void Run();
        void AirDash();

        void PushBackGetHit(object sender, GetHitEventArgs e);
        void PushBackSendHit(object sender, SendHitEventArgs e);
    }
}

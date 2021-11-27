namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public interface IMovementModule
    {
        // Standard movements
        void Jump();
        void Walk();
        void Run();
        void AirDash();

        // Get hit trajectories
        void Launch();
        void Dunk();
        void PushBackGetHit(object sender, GetHitEventArgs e);
        void PushBackSendHit(object sender, SendHitEventArgs e);
    }
}

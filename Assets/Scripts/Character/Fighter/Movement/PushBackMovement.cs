using UnityEngine;
namespace animeFGBeatEmUp.Assets.Scripts.Character.Fighter
{
    public class PushBackMovement
    {
        public static void PushBackSendHit(SendHitEventArgs eventArgs, Rigidbody2D rigidbody2D) {
            AttackerPushBack(eventArgs.isGrounded, eventArgs.victimXPosition, eventArgs.attackData, rigidbody2D);
        }

        public static void PushBackGetHit(GetHitEventArgs eventArgs, Rigidbody2D rigidbody2D) {
            if (eventArgs.attackData.Type == AttackType.Launcher)
            {
                // Launch enemy uP!
                GetLaunched(eventArgs.xPosition, eventArgs.attackData, rigidbody2D);
            }
            else if (eventArgs.attackData.Type == AttackType.Dunk) {
                // Launch enemy Down!
                GetDunked(eventArgs.xPosition, eventArgs.attackData, rigidbody2D);
            }
            else if (eventArgs.attackData.Type == AttackType.HeavyLauncher) {
                // Wall send enemy!
                GetHeavyLaunched(eventArgs.xPosition, eventArgs.attackData, rigidbody2D);
            }
            else
            {
                // normal attack
                if (eventArgs.isGrounded) {
                    GroundedPushBack(eventArgs.xPosition, eventArgs.attackData, rigidbody2D);
                } else {
                    GetLaunched(eventArgs.xPosition, eventArgs.attackData, rigidbody2D);
                }
            }
        }

        private static void AttackerPushBack(bool isGrounded, float victimXPosition, Attack attackData, Rigidbody2D rigidbody2D) {
            if (isGrounded) {
                rigidbody2D.velocity = new Vector2(
                    attackData.GetPushback() * attackData.GetPushBackDirection(victimXPosition) / 4.0f,
                    0f);
            } else {
                rigidbody2D.velocity = new Vector2(
                    attackData.GetPushback() * attackData.GetPushBackDirection(victimXPosition) / 8.0f,
                    0f);
            }
        }

        private static void GroundedPushBack(float victimXPosition, Attack attackData, Rigidbody2D rigidbody2D) {
            rigidbody2D.velocity = new Vector2(
                attackData.GetPushback() * attackData.GetPushBackDirection(victimXPosition),
                0f);
        }
            
        private static void GetLaunched(float victimXPosition, Attack attackData, Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = new Vector2(
                attackData.GetPushback() * attackData.GetPushBackDirection(victimXPosition),
                AttackConstants.LightLaunchForce[1]);
        }

        private static void GetHeavyLaunched(float victimXPosition, Attack attackData, Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = new Vector2(
                AttackConstants.HighLaunchForce[0] * attackData.GetPushBackDirection(victimXPosition),
                AttackConstants.HighLaunchForce[1]);
        }

        private static void GetDunked(float victimXPosition, Attack attackData, Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = new Vector2(
                AttackConstants.DunkForce[0] * attackData.GetPushBackDirection(victimXPosition),
                AttackConstants.DunkForce[1]);
        }
    }
}
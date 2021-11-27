using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentHurtbox : MonoBehaviour
{
    private HealthManager HpManager;
    private IMovementController Movement;
    private IStateManager State;
    private Rigidbody2D Rigidbody;
    private IDictionary<string, int> currRegisteredAttacks;
    private readonly object registerAttackLock = new object();

    // Start is called before the first frame update
    void Start()
    {
        HpManager = GetComponent<HealthManager>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Movement = GetComponent<IMovementController>();
        State = GetComponent<IStateManager>();
        currRegisteredAttacks = new Dictionary<string, int>();
    }

    public void RegisterAttack(Attack attackData)
    {
        lock (registerAttackLock)
        {
            if (!currRegisteredAttacks.ContainsKey(attackData.Id))
            {
                // Attack landed!
                if (attackData.Type == AttackType.Throw)
                {
                    if (Movement.isGrounded) {
                        State.TakeThrow(attackData.playerState);
                        Movement.FreezeCharacter();
                        currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                    }
                } else if (attackData.Type == AttackType.AntiAirThrow) {
                    if (!Movement.isGrounded) {
                        State.TakeThrow(attackData.playerState);
                        Movement.FreezeCharacter();
                        currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                    }
                }
                else
                {
                    // Strike Based
                    if (State.isBlocking) {
                        // Chip Damage?
                        // HpManager.DealDamage(attackData.Damage);
                        // Block Sound
                        // SoundManagerController.playSFX(SoundManagerController.hitLvl1Sound);
                        // BlockStun
                        Movement.TriggerBlockStun(attackData);
                    } else {
                        SoundManagerController.playSFX(SoundManagerController.hitLvl1Sound);
                        HpManager.DealDamage(attackData.Damage);
                        Movement.TriggerHitStun(attackData);
                    }
                    attackData.fighterComms.OnOtherFighterStrike(attackData, Movement.xPosition);
                    // attackData.playerState.GetAttackController().TriggerHitStop(attackData, Movement.xPosition);
                    currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                    
                }
            }
        }
    }

    public void UnregisterAttack(Attack attackData)
    {
        lock (registerAttackLock)
        {
            if (currRegisteredAttacks.ContainsKey(attackData.Id))
            {
                currRegisteredAttacks.Remove(attackData.Id);
            }
        }
    }
}

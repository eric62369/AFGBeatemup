using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitEventArgs
{
    public Attack attackData { get; private set; }

    public GetHitEventArgs(Attack attackData_) {
        attackData = attackData_;
    }
}

public class ParentHurtbox : MonoBehaviour
{
    private EnemyHealthManager HpManager;
    private IMovementController EnemyMovement;
    private EnemyStateManager EnemyState;
    private Rigidbody2D Rigidbody;
    private IDictionary<string, int> currRegisteredAttacks;
    private readonly object registerAttackLock = new object();

    // Start is called before the first frame update
    void Start()
    {
        HpManager = GetComponent<EnemyHealthManager>();
        Rigidbody = GetComponent<Rigidbody2D>();
        EnemyMovement = GetComponent<IMovementController>();
        EnemyState = GetComponent<EnemyStateManager>();
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
                    if (EnemyState.isGrounded) {
                        EnemyState.TakeThrow(attackData.playerState);
                        EnemyMovement.FreezeCharacter();
                        currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                    }
                } else if (attackData.Type == AttackType.AntiAirThrow) {
                    if (!EnemyState.isGrounded) {
                        EnemyState.TakeThrow(attackData.playerState);
                        EnemyMovement.FreezeCharacter();
                        currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                    }
                }
                else
                {
                    SoundManagerController.playSFX(SoundManagerController.hitLvl1Sound);
                    attackData.playerState.GetAttackController().TriggerHitStop(attackData);
                    EnemyMovement.TriggerHitStun(attackData);
                    currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                    HpManager.DealDamage(attackData.Damage);
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

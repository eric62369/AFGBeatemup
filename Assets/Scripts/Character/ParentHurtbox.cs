using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentHurtbox : MonoBehaviour
{
    private EnemyHealthManager HpManager;
    private IMovementController EnemyMovement;
    private EnemyStateManager EnemyState;
    private Rigidbody2D Rigidbody;
    private IDictionary<string, int> currRegisteredAttacks;
    private readonly object registerAttackLock = new object();

    // public delegate void Land(object sender, LandEventArgs args);
    // public event Land LandEvent;

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
                    } else {

                    }
                    
                }
                else
                {
                    SoundManagerController.playSFX(SoundManagerController.hitLvl1Sound);
                    attackData.playerState.GetAttackController().TriggerHitStop(attackData);
                    EnemyMovement.TriggerHitStun(attackData);
                    currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                    HpManager.DealDamage(attackData.Damage);
                    // Trigger pushback event in pushbackscript
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

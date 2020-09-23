using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentHurtbox : MonoBehaviour
{
    private EnemyHealthManager HpManager;
    private EnemyMovementController EnemyMovement;
    private Rigidbody2D Rigidbody;
    private IDictionary<string, int> currRegisteredAttacks;
    private readonly object registerAttackLock = new object();
    private PlayerAttackController player1;

    // Start is called before the first frame update
    void Start()
    {
        HpManager = GetComponent<EnemyHealthManager>();
        Rigidbody = GetComponent<Rigidbody2D>();
        EnemyMovement = GetComponent<EnemyMovementController>();
        player1 = GameObject.FindWithTag("Player").GetComponent<PlayerAttackController>();
        currRegisteredAttacks = new Dictionary<string, int>();
    }

    public void RegisterAttack(Attack attackData)
    {
        lock (registerAttackLock)
        {
            if (!currRegisteredAttacks.ContainsKey(attackData.Id))
            {
                // Attack landed!
                player1.TriggerHitStop(attackData);
                EnemyMovement.TriggerHitStun(attackData);
                currRegisteredAttacks.Add(attackData.Id, attackData.Damage);
                HpManager.DealDamage(attackData.Damage);
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

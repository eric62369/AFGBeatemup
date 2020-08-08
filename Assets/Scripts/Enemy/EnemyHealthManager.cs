using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int MaxEnemyHealth;
    public int CurrentEnemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnemyHealth = MaxEnemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEnemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DealDamage(int damage)
    {
        CurrentEnemyHealth -= damage;
    }
}

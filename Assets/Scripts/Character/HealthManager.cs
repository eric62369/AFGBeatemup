using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Defeated(object sender, DefeatedEventArgs args);

public class DefeatedEventArgs
{
    // public Attack attackData { get; private set; }

    public DefeatedEventArgs() {
        // attackData = attackData_;
    }
}
public class HealthManager : MonoBehaviour
{
    public event Defeated DefeatedEvent;
    
    public int MaxHealth;
    private int CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    protected virtual void RaiseDefeatedEvent(DefeatedEventArgs e) {
        Defeated raiseEvent = DefeatedEvent;

        if (raiseEvent != null) {
            raiseEvent(this, e);
        }
    }

    public void DealDamage(int damage)
    {
        int previousHealth = CurrentHealth;
        CurrentHealth -= damage;
        if (previousHealth > 0 && CurrentHealth <= 0) {
            RaiseDefeatedEvent(new DefeatedEventArgs());
        }
    }
}

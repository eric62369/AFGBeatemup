using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HurtboxState
{
    Open,
    Colliding
}

public class HurtboxController : MonoBehaviour
{
    // public HurtboxState ColliderState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerHitbox")
        {
            HitBoxController hitbox = other.gameObject.GetComponent<HitBoxController>();
            Attack attack = hitbox.AttackData;
            this.gameObject.transform.root.GetComponent<ParentHurtbox>().RegisterAttack(attack);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerHitbox")
        {
            HitBoxController hitbox = other.gameObject.GetComponent<HitBoxController>();
            Attack attack = hitbox.AttackData;
            this.gameObject.transform.root.GetComponent<ParentHurtbox>().UnregisterAttack(attack);
        }
    }
}

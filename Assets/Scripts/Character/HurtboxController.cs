using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the hitbox is opposite type of hitbox
        if (isOtherOpponent(other))
        {
            HitBoxController hitbox = other.gameObject.GetComponent<HitBoxController>();
            Attack attack = hitbox.AttackData;
            this.gameObject.transform.root.GetComponent<ParentHurtbox>().RegisterAttack(attack);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isOtherOpponent(other))
        {
            HitBoxController hitbox = other.gameObject.GetComponent<HitBoxController>();
            Attack attack = hitbox.AttackData;
            this.gameObject.transform.root.GetComponent<ParentHurtbox>().UnregisterAttack(attack);
        }
    }

    private bool isOtherOpponent(Collider2D other) {
        return (other.gameObject.tag == "EnemyHitbox" && this.gameObject.tag == "PlayerHurtbox") ||
        (other.gameObject.tag == "PlayerHitbox" && this.gameObject.tag == "EnemyHurtbox");
    }
}

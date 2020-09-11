using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/*
https://www.gamasutra.com/blogs/NahuelGladstein/20180514/318031/Hitboxes_and_Hurtboxes_in_Unity.php
*/

public class HitBoxController : MonoBehaviour
{
    public int AttackDamage;
    public int AttackLevel;
    /// Example: P1-5B
    public string AttackId;
    public Attack AttackData;
    private Animator Anim;
    private Rigidbody2D Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Anim = this.gameObject.transform.root.GetComponent<Animator>();
        Rigidbody = this.gameObject.transform.root.GetComponent<Rigidbody2D>();
        PlayerStateManager playerState = this.gameObject.transform.root.GetComponent<PlayerStateManager>();
        AttackData = new Attack(AttackId, AttackLevel, AttackDamage, playerState);
    }
}
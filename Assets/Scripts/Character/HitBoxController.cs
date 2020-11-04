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
    /**
     Example: 5B
     Based on playerIndex, P1- or P2- will be appended onto the attack id
     */
    public string AttackId;
    public AttackType Type;
    public Attack AttackData;
    protected Animator Anim;
    protected Rigidbody2D Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Anim = this.gameObject.transform.root.GetComponent<Animator>();
        Rigidbody = this.gameObject.transform.root.GetComponent<Rigidbody2D>();
        PlayerStateManager playerState = this.gameObject.transform.root.GetComponent<PlayerStateManager>();
        string attackIdPrefix = "P" + (playerState.GetPlayerIndex() + 1) + "-";
        AttackId = attackIdPrefix + AttackId;
        AttackData = new Attack(AttackId, AttackLevel, AttackDamage, Type, playerState);
    }
}
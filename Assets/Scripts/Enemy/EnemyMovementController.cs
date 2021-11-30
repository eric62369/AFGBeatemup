// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;
// using animeFGBeatEmUp.Assets.Scripts.Character.Fighter;

// public class EnemyMovementController : MonoBehaviour, IMovementController
// {
//     private Rigidbody2D rb2d;
//     public Animator animator;

//     private EnemyStateManager enemyState;

//     public event Land LandEvent;

//     public bool isGrounded { get; private set; }

//     public float xPosition {
//         get {
//             return gameObject.transform.position.x;
//         }
//     }

//     public Transform groundCheck;
//     public float groundCheckRadius;
//     public LayerMask groundLayers;

//     // Use this for initialization
//     void Start()
//     {
//         //Get and store a reference to the Rigidbody2D component so that we can access it.
//         rb2d = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         enemyState = GetComponent<EnemyStateManager>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         bool newGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

//         if (newGrounded != isGrounded && newGrounded == true)
//         {
//             // Landed!
//             RaiseLandEvent(new LandEventArgs());
//         }
//         isGrounded = newGrounded;
//     }
    

//     public void Pushback(Vector2 force) {
//         rb2d.AddForce(force, ForceMode2D.Impulse);
//     }

//     public async Task TriggerHitStun(Attack attackData)
//     {
//         // Trigger animation's hitstun 
//         FreezeCharacter();
//         // TODO: Do we need to be able to interrupt hitstop? Probably
//         // await Task.Delay(attackData.GetHitStop());
//         UnFreezeCharacter();
//         RaiseGetHitEvent(new GetHitEventArgs(attackData));
//     }

//     public async Task TriggerBlockStun(Attack attackData)
//     {
//         // Trigger animation's hitstun 
//         FreezeCharacter();
//         // TODO: Do we need to be able to interrupt hitstop? Probably
//         // await Task.Delay(attackData.GetHitStop());
//         UnFreezeCharacter();
//         RaiseGetHitEvent(new GetHitEventArgs(attackData));
//     }

//     protected virtual void RaiseLandEvent(LandEventArgs e) {
//         Land raiseEvent = LandEvent;
//         if (raiseEvent != null) {
//             raiseEvent(this, e);
//         }
//     }

//     /// Returns the velocity before freezing
//     public Vector2 FreezeCharacter()
//     {
//         animator.enabled=false;
//         rb2d.bodyType = RigidbodyType2D.Kinematic;
//         Vector2 oldVelocity = rb2d.velocity;
//         rb2d.velocity = new Vector2(0f, 0f);
//         return oldVelocity;
//     }

//     public void UnFreezeCharacter(Vector2 oldVelocity)
//     {
//         animator.enabled=true;
//         rb2d.bodyType = RigidbodyType2D.Dynamic;
//         rb2d.velocity = oldVelocity;
//     }
//     public void UnFreezeCharacter()
//     {
//         animator.enabled=true;
//         rb2d.bodyType = RigidbodyType2D.Dynamic;
//         rb2d.velocity = new Vector2(0f, 0f);
//     }

//     public void Launch(int direction)
//     {
//         rb2d.velocity = new Vector2(
//             AttackConstants.LightLaunchForce[0] * direction,
//             AttackConstants.LightLaunchForce[1]);
//     }

//     public void HeavyLaunch(int direction)
//     {
//         rb2d.velocity = new Vector2(
//             AttackConstants.HeavyLaunchForce[0] * direction,
//             AttackConstants.HeavyLaunchForce[1]);
//     }

//     public void HighLaunch() {
//         rb2d.AddForce(new Vector2(AttackConstants.HighLaunchForce[0], AttackConstants.HighLaunchForce[1]), ForceMode2D.Impulse);
//     }

//     public void Dunk(int direction)
//     {
//         rb2d.velocity = new Vector2(
//             AttackConstants.DunkForce[0] * direction,
//             AttackConstants.DunkForce[1]);
//     }

//     // Must always be called before Recovery frames
//     // public async Task TriggerHitStop(Attack attackData)
//     // {
//     //     // Get animator
//     //     // Pause animator for x seconds
//     //     Vector2 oldVelocity = FreezeCharacter();
//     //     // TODO: Do we need to be able to interrupt hitstop? Probably
//     //     await Task.Delay(attackData.GetHitStop());
//     //     UnFreezeCharacter(oldVelocity);
//     //     // resume animation
//     // }
// }

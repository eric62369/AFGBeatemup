using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatProcess : MonoBehaviour
{
    public bool isDefeated { get; private set; }
    private IMovementController movement;
    private CharacterAnimationController animationController;
    
    public GameObject nextboss;
    void Start()
    {
        HealthManager hp = GetComponent<HealthManager>();
        movement = GetComponent<IMovementController>();
        animationController = GetComponent<CharacterAnimationController>();
        hp.DefeatedEvent += StartDefeatProcess;
        isDefeated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDefeated && movement.isGrounded) {
            DeleteEnemy();
        }
    }
    private void StartDefeatProcess(object sender, DefeatedEventArgs e) {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        // render.color = new Color(255, 150, 150);
        render.color = Color.blue;
        isDefeated = true;
        animationController.AnimationSetFloat("StunAnimationSpeed", 0f);
        movement.HighLaunch(); // TODO: Figure out how to get this high launch off?
    }

    private void DeleteEnemy() {
        if (gameObject.tag == "Boss") {
            // Spawn next level door / boss
            Instantiate(nextboss);
        }
        Destroy(gameObject);
    }
}

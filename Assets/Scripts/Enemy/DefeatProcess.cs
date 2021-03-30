using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatProcess : MonoBehaviour
{
    private bool isDefeated;
    private IMovementController movement;
    // Start is called before the first frame update
    public GameObject nextboss;
    void Start()
    {
        HealthManager hp = GetComponent<HealthManager>();
        movement = GetComponent<IMovementController>();
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
        movement.HighLaunch();
    }

    private void DeleteEnemy() {
        if (gameObject.tag == "Boss") {
            // Spawn next level door / boss
            Instantiate(nextboss);
        }
        Destroy(gameObject);
    }
}

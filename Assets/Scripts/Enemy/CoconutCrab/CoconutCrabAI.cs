﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutCrabAI : MonoBehaviour
{
    public float StepForce;
    private EnemyMovementController movementController;
    private IStateManager stateManager;
    
    // Start is called before the first frame update
    void Start()
    {
        movementController = GetComponent<EnemyMovementController>();
        stateManager = GetComponent<IStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void StepForward() {
        float direction = 1;
        if (!stateManager.GetIsP1Side()) {
            direction *= -1;
        }
        movementController.Pushback(new Vector2(direction * StepForce, 0));
    }
}

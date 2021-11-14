using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScript : MonoBehaviour {
    private static UpdateScript instance;
    public static void AddUpdateCallback(Action updateMethod) {
        if (instance == null) {
            instance = new GameObject("[Update Caller]").AddComponent<UpdateScript>();
        }
        instance.updateCallback += updateMethod;
    }
    
    private Action updateCallback;
    
    private void Update() {
        updateCallback();
    }
}

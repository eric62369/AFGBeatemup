using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMonoBehaviourTest : MonoBehaviour
{
    public int frameWait;
    private int frameCount;

    // Start is called before the first frame update

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount >= frameWait) {
            frameCount = 0;
        }
        frameCount++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMonoBehaviourTest : MonoBehaviour
{
    public int frameWait;
    private int frameCount;

    // Start is called before the first frame update


    private RefillClock clock;
    void Start()
    {
        clock = new RefillClock();
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount >= frameWait) {
            clock.ResetClock(100);
            frameCount = 0;
        }
        frameCount++;
    }
}

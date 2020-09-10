using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        SearchForBoss();
    }

    /// Set reference to the current boss enemy
    public void SearchForBoss()
    {
        boss = GameObject.FindWithTag("Boss");
        if (boss == null)
        {
            throw new InvalidOperationException("Tried to search for boss when no boss found");
        }
    }

    /// If on same x position, player is on P1 side
    public bool GetIsP1Side()
    {
        if (boss == null)
        {
            throw new InvalidOperationException(
                "Player state must have a boss enemy reference first (currently null)"
            );

        }
        float posDiff = this.gameObject.transform.position.x - boss.transform.position.x;
        return posDiff <= 0;
    }

    public void UpdateFacingDirection()
    {
        Vector3 newScale = this.gameObject.transform.localScale;
        newScale.x *= -1;
        this.gameObject.transform.localScale = newScale;
    }
}

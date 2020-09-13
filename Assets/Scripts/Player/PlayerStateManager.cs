using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private GameObject boss;
    /// i.e. I jumped and crossed up, I airdash forward (the direction I'm facing)
    private bool isFacingRight;

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
    /// Gets absolute p1 or p2 side, not facing direction
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

    /// Return last updated facing direction
    /// True is Right facing (P1) False is Left Facing
    public bool GetCurrentFacingDirection()
    {
        return isFacingRight;
    }

    public void UpdateFacingDirection()
    {
        Vector3 newScale = this.gameObject.transform.localScale;
        newScale.x = Math.Abs(newScale.x);
        if (!GetIsP1Side())
        {
            newScale.x *= -1;
            isFacingRight = false;
        }
        else
        {
            isFacingRight = true;
        }
        this.gameObject.transform.localScale = newScale;
    }
}

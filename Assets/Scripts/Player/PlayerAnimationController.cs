using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Player's Unity animator
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }


    public void AnimationSetBool(string animationId, bool setValue)
    {
        animator.SetBool(animationId, setValue);
    }
    public bool AnimationGetBool(string animationId)
    {
        return animator.GetBool(animationId);
    }
    public void AnimationSetTrigger(string animationId)
    {
        animator.SetTrigger(animationId);
    }
    public void AnimationResetTrigger(string animationId)
    {
        animator.ResetTrigger(animationId);
    }

    public void AnimatorEnable(bool state) {
        animator.enabled = state;
    }

    public bool GetAnimatorEnable() {
        return animator.enabled;
    }
}

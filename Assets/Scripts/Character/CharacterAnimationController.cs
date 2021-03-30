using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    // Player's Unity animator
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AnimationSetFloat(string animationId, float data)
    {
        animator.SetFloat(animationId, data);
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

using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpAniPopup : PopupBase
{
    [SerializeField, Tooltip("AnimationEventCallback")]
    AnimationEventCallback Anim_Event;

    [SerializeField, Tooltip("Animator")]
    Animator Anim;
    protected override bool Initialize(object[] data)
    {
        Anim_Event?.SetAnimationEventCallback(AnimationCompleteCallback);
        Anim.Play("LevelUpAni");

        return true;
    }

    void AnimationCompleteCallback(string key)
    {
        HidePopup();
    }
}

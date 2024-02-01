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
    public override void ShowPopup(params object[] data)
    {
        Anim_Event?.SetAnimationEventCallback(AnimationCompleteCallback);

        base.ShowPopup(data);
        Anim.Play("LevelUpAni");
    }


    void AnimationCompleteCallback(string key)
    {
        HidePopup();
    }
}

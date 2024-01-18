using UnityEngine;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;

public enum ScreenEffect
{
    NONE = 0,
    FADE_IN,
    FADE_OUT,
    MAX,
}

public class ScreenEffectManager : Singleton<ScreenEffectManager>
{
    ScreenEffectUIContainer Container;
    Queue<(ScreenEffect effect, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease)> Sequence_Actions;

    ScreenEffectManager() { }

    protected override void Initialize()
    {
        Sequence_Actions = new Queue<(ScreenEffect effect, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease)> ();
        Container = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Canvas_ScreenEffect", null).GetComponent<ScreenEffectUIContainer>();
    }

    public void StartAction(ScreenEffect effect, Action cb = null, float duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        switch (effect)
        {
            case ScreenEffect.FADE_OUT:
                FadeOut(cb, duration, pre_delay, post_delay, ease);
                break;
            case ScreenEffect.FADE_IN:
                FadeIn(cb, duration, pre_delay, post_delay, ease);
                break;
        }
    }

    public void AddSequenceAction(ScreenEffect effect, Action cb = null, float duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        Sequence_Actions.Enqueue((effect, cb, duration, pre_delay, post_delay, ease));
    }

    public void StartSequenceActions()
    {
        StartSequenceRemainActions(Sequence_Actions);
    }

    public void FadeOut(Action cb, float fade_duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        Container.Cover_Image.color = new Color(0, 0, 0, 0);
        Container.Cover_Image_EaseAlpha.SetEasing(ease, pre_delay, fade_duration, post_delay);
        Container.Cover_Image_EaseAlpha.StartEasing(1.0f, cb);
    }

    public void FadeIn(Action cb, float fade_duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        Container.Cover_Image.color = new Color(0, 0, 0, 1);
        Container.Cover_Image_EaseAlpha.SetEasing(ease, pre_delay, fade_duration, post_delay);
        Container.Cover_Image_EaseAlpha.StartEasing(0.0f, cb);
    }

    void StartSequenceRemainActions(Queue<(ScreenEffect effect, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease)> actions)
    {
        var delay = new WaitForSeconds(0.05f);

        (ScreenEffect effect, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease) action = default;

        if (actions.TryDequeue(out action))
        {
            StartAction(action.effect, () => {
                action.cb?.Invoke(); 
                StartSequenceRemainActions(actions);
            }, action.duration, action.pre_delay, action.post_delay, action.ease);
        }
    }
}

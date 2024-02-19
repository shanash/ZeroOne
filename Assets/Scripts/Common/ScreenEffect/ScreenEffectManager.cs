using UnityEngine;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public enum ScreenEffect
{
    NONE = 0,
    FADE_IN,
    FADE_OUT,
    DAMAGE_INDICATOR, // 데미지 받았을때 주변이 빨갛게 변한다
    FLASH, // 특정색(밝은색)으로 잠깐동안 깜박이게한다. 강력한 공격이나 폭발시 사용

    //VIGNETTE, // 가장자리를 어둡게 해서 중앙에 초점을 맞춤
    //GLOW, // 화면에 특정 좌표에 발광효과를 추가
    //LETTER_BOX, // 화면 상단과 하단에 검은 막대를 추가
    //BLUR, // 메뉴나 다이얼로그가 활성화될때 카메라를 흐리게 한다
    MAX,
}

public class ScreenEffectManager : MonoSingleton<ScreenEffectManager>
{
    ScreenEffectUIContainer Container;
    Queue<(InnerActionType action_type, object param, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease)> Sequence_Actions;
    bool DoingSequenceActions;

    protected override bool ResetInstanceOnChangeScene => false;
    protected override bool Is_DontDestroyOnLoad => true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CallInstance()
    {
        _ = Instance;
    }

    protected override void Initialize()
    {
        DoingSequenceActions = false;
        Sequence_Actions = new Queue<(InnerActionType action_type, object param, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease)>();
        Container = GameObjectPoolManager.Instance.GetGameObject("Prefabs/Canvas_ScreenEffect", this.transform).GetComponent<ScreenEffectUIContainer>();
    }

    public void StartAction(ScreenEffect effect, Action cb = null, float duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        AddSequenceAction(effect, cb, duration, pre_delay, post_delay, ease);
        StartSequenceActions();
    }

    public async UniTask StartActionAsync(ScreenEffect effect, Action cb = null, float duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        AddSequenceAction(effect, cb, duration, pre_delay, post_delay, ease);
        await StartSequenceActionsAsync();
    }

    public void AddSequenceAction(ScreenEffect effect, Action cb = null, float duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        switch (effect)
        {
            case ScreenEffect.FADE_IN:
                Sequence_Actions.Enqueue((InnerActionType.CHANGE_COVER_ALPHA, (new Color(0, 0, 0, 1), 0f), cb, duration, pre_delay, post_delay, ease)); // 검은색 알파 1 -> 0
                break;
            case ScreenEffect.FADE_OUT:
                Sequence_Actions.Enqueue((InnerActionType.CHANGE_COVER_ALPHA, (new Color(0, 0, 0, 0), 1f), cb, duration, pre_delay, post_delay, ease)); // 검은색 알파 0 -> 1
                break;
            case ScreenEffect.DAMAGE_INDICATOR:
                Sequence_Actions.Enqueue((InnerActionType.CHANGE_COVER_ALPHA, (new Color(1, 0, 0, 0), 1f), cb, duration/2, pre_delay, 0, ease)); // 빨간색 알파 0 -> 1
                Sequence_Actions.Enqueue((InnerActionType.CHANGE_COVER_ALPHA, (new Color(1, 0, 0, 1), 0f), cb, duration/2, 0, post_delay, ease)); // 빨간색 알파 1 -> 0
                break;
            case ScreenEffect.FLASH:
                Sequence_Actions.Enqueue((InnerActionType.CHANGE_COVER_ALPHA, (new Color(1, 1, 1, 0), 1f), cb, duration / 2, pre_delay, 0, ease)); // 하얀색 알파 0 -> 1
                Sequence_Actions.Enqueue((InnerActionType.CHANGE_COVER_ALPHA, (new Color(1, 1, 1, 1), 0f), cb, duration / 2, 0, post_delay, ease)); // 하얀색 알파 1 -> 0
                break;
        }
    }

    public void StartSequenceActions()
    {
        if (DoingSequenceActions)
        {
            return;
        }
        DoingSequenceActions = true;
        StartSequenceRemainActions(Sequence_Actions);
    }

    public async UniTask StartSequenceActionsAsync()
    {
        if (DoingSequenceActions)
        {
            return;
        }
        DoingSequenceActions = true;
        await StartSequenceActionsAsync(Sequence_Actions);
    }

    /// <summary>
    /// 재귀호출시켜서 큐에 남아있는 액션이 없을때까지 Ease액션을 실행시킨다
    /// </summary>
    /// <param name="actions"></param>
    void StartSequenceRemainActions(Queue<(InnerActionType action_type, object param, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease)> actions)
    {
        (InnerActionType action_type, object param, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease) action = default;

        if (actions.TryDequeue(out action))
        {
            StartInnerAction(action.action_type, action.param, () => {
                action.cb?.Invoke(); 
                StartSequenceRemainActions(actions);
            }, action.duration, action.pre_delay, action.post_delay, action.ease);
        }
        else
        {
            DoingSequenceActions = false;
        }
    }

    /// <summary>
    /// 재귀호출시켜서 큐에 남아있는 액션이 없을때까지 Ease액션을 실행시킨다
    /// </summary>
    /// <param name="actions"></param>
    async UniTask StartSequenceActionsAsync(Queue<(InnerActionType action_type, object param, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease)> actions)
    {
        (InnerActionType action_type, object param, Action cb, float duration, float pre_delay, float post_delay, EasingFunction.Ease ease) action = default;

        bool is_continued = false;

        while (actions.TryDequeue(out action))
        {
            StartInnerAction(action.action_type, action.param, () => {
                action.cb?.Invoke();
                is_continued = true;
            }, action.duration, action.pre_delay, action.post_delay, action.ease);

            await UniTask.WaitUntil(() => is_continued);

            is_continued = false;
        }

        DoingSequenceActions = false;
    }

    /// <summary>
    /// 실질적인 Ease액션을 실행하기 위한 메소드
    /// </summary>
    /// <param name="type"></param>
    /// <param name="param"></param>
    /// <param name="cb"></param>
    /// <param name="duration"></param>
    /// <param name="pre_delay"></param>
    /// <param name="post_delay"></param>
    /// <param name="ease"></param>
    void StartInnerAction(InnerActionType type, object param, Action cb = null, float duration = 1, float pre_delay = 0, float post_delay = 0, EasingFunction.Ease ease = EasingFunction.Ease.Linear)
    {
        switch (type)
        {
            case InnerActionType.CHANGE_COVER_ALPHA:
                var tuple = ((Color color, float dest_alpha))param;
                ChangeCoverAlpha(tuple.color, tuple.dest_alpha, cb, duration, pre_delay, post_delay, ease);
                break;
        }
    }

    /// <summary>
    /// 알파 체인지 Ease액션
    /// </summary>
    /// <param name="origin_color"></param>
    /// <param name="dest_alpha"></param>
    /// <param name="cb"></param>
    /// <param name="fade_duration"></param>
    /// <param name="pre_delay"></param>
    /// <param name="post_delay"></param>
    /// <param name="ease"></param>
    void ChangeCoverAlpha(Color origin_color, float dest_alpha, Action cb, float fade_duration, float pre_delay, float post_delay, EasingFunction.Ease ease)
    {
        Container.Cover_Image.color = origin_color;
        Container.Cover_Image_EaseAlpha.SetEasing(ease, pre_delay, fade_duration, post_delay);
        Container.Cover_Image_EaseAlpha.StartEasing(dest_alpha, cb);
    }

    public enum InnerActionType
    {
        NONE = 0,
        CHANGE_COVER_ALPHA,
    }
}

using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class UIInteractiveButton : UIInteractiveButtonBase
{
    [SerializeField, Tooltip("Touch Callback")]
    UnityEvent<TOUCH_RESULT_TYPE, object> _Touch_Callback = new UnityEvent<TOUCH_RESULT_TYPE, object>();

    [SerializeField, Tooltip("Touch Tooltip Callback")]
    UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> _Touch_Tooltip_Callback = new UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object>();

    public UnityEvent<TOUCH_RESULT_TYPE, object> Touch_Callback => _Touch_Callback;
    public UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> Touch_Tooltip_Callback => _Touch_Tooltip_Callback;
    public object Tooltip_Data { private get; set; }

    protected override UnityEventBase Touch_Callback_Base => _Touch_Tooltip_Callback;

    /// 전투씬은 해상도를 별도로(2340 x 1080) 사용하고 있어서 특수한 계산을 더 해줘야 한다
    Func<bool, Rect> Func_Rect => (is_battle_scene) => GameObjectUtils.GetScreenRect(GetComponent<RectTransform>(),
    is_battle_scene ? new Vector2(GameDefine.RESOLUTION_SCREEN_WIDTH, GameDefine.RESOLUTION_SCREEN_HEIGHT)
    : default);

    protected override void OnTouchEvent(TOUCH_RESULT_TYPE type)
    {
        Touch_Callback?.Invoke(type, Tooltip_Data);
        Touch_Tooltip_Callback?.Invoke(type, Func_Rect, Tooltip_Data);
    }
}

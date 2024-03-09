using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class UITooltipButton : UILongTouchButtonBase
{
    [SerializeField, Tooltip("Touch Tooltip Callback")]
    public UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> Touch_Tooltip_Callback = new UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object>();

    Dictionary<TOUCH_RESULT_TYPE, object[]> Parameters = new Dictionary<TOUCH_RESULT_TYPE, object[]>();

    /// 전투씬은 해상도를 별도로(2340 x 1080) 사용하고 있어서 특수한 계산을 더 해줘야 한다
    Func<bool, Rect> Func_Rect => (is_battle_scene) => GameObjectUtils.GetScreenRect(GetComponent<RectTransform>(),
    is_battle_scene ? new Vector2(GameDefine.RESOLUTION_SCREEN_WIDTH, GameDefine.RESOLUTION_SCREEN_HEIGHT)
    : default);

    protected override UnityEventBase Touch_Callback_Base => Touch_Tooltip_Callback;
    protected override Dictionary<TOUCH_RESULT_TYPE, object[]> Parameters_OnPointer => Parameters;
    public UnityEvent<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> ABC => Touch_Tooltip_Callback;

    public void SetTooltipData(object data)
    {
        if (Parameters.Count > 0)
        {
            Parameters.Clear();
        }

        Parameters.Add(TOUCH_RESULT_TYPE.LONG_PRESS, new object[] { TOUCH_RESULT_TYPE.LONG_PRESS, Func_Rect, data });
        Parameters.Add(TOUCH_RESULT_TYPE.RELEASE, new object[] { TOUCH_RESULT_TYPE.RELEASE, null, null });
    }
}

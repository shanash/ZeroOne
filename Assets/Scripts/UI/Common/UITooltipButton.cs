using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITooltipButton : UILongTouchButtonBase
{
    [SerializeField, Tooltip("Touch Tooltip Callback")]
    UnityEvent<TOUCH_RESULT_TYPE, Rect> Touch_Tooltip_Callback = null;

    /// <summary>
    /// 전투씬은 해상도를 별도로(2340 x 1080) 사용하고 있어서 특수한 계산을 더 해줘야 한
    /// </summary>
    [SerializeField, Tooltip("Is Battle Scene")]
    bool Is_Battle_Scene = false;

    Dictionary<TOUCH_RESULT_TYPE, object[]> Parameters = new Dictionary<TOUCH_RESULT_TYPE, object[]>();

    Rect ScreenRect => GameObjectUtils.GetScreenRect(GetComponent<RectTransform>(),
    Is_Battle_Scene ? new Vector2(GameDefine.RESOLUTION_SCREEN_WIDTH, GameDefine.RESOLUTION_SCREEN_HEIGHT)
    : default);

    protected override UnityEventBase Touch_Callback_Base => Touch_Tooltip_Callback;
    protected override Dictionary<TOUCH_RESULT_TYPE, object[]> Parameters_OnPointer => Parameters;

    protected override void Start()
    {
        base.Start();
        Parameters.Add(TOUCH_RESULT_TYPE.CLICK, new object[] { TOUCH_RESULT_TYPE.CLICK, ScreenRect });
        Parameters.Add(TOUCH_RESULT_TYPE.LONG_PRESS, new object[] { TOUCH_RESULT_TYPE.LONG_PRESS, ScreenRect });
        Parameters.Add(TOUCH_RESULT_TYPE.RELEASE, new object[] { TOUCH_RESULT_TYPE.RELEASE, ScreenRect });
    }
}

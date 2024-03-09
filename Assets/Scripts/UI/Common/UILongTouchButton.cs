using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UILongTouchButton : UILongTouchButtonBase
{
    [SerializeField, Tooltip("Touch Callback")]
    UnityEvent<TOUCH_RESULT_TYPE> Touch_Callback;

    Dictionary<TOUCH_RESULT_TYPE, object[]> Parameters = new Dictionary<TOUCH_RESULT_TYPE, object[]>()
    {
         {TOUCH_RESULT_TYPE.CLICK, new object[] { TOUCH_RESULT_TYPE.CLICK }},
         {TOUCH_RESULT_TYPE.LONG_PRESS, new object[] { TOUCH_RESULT_TYPE.LONG_PRESS }},
         {TOUCH_RESULT_TYPE.RELEASE, new object[] { TOUCH_RESULT_TYPE.RELEASE }},
    };
    protected override UnityEventBase Touch_Callback_Base => Touch_Callback;
    protected override Dictionary<TOUCH_RESULT_TYPE, object[]> Parameters_OnPointer => Parameters;
}

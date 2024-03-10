using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UILongTouchButton : UILongTouchButtonBase
{
    [SerializeField, Tooltip("Touch Callback")]
    UnityEvent<TOUCH_RESULT_TYPE> Touch_Callback;

    protected override UnityEventBase Touch_Callback_Base => Touch_Callback;

    protected override void OnTouchEvent(TOUCH_RESULT_TYPE type)
    {
        Touch_Callback?.Invoke(type);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class UITooltipButton : MonoBehaviour
{
    [SerializeField, Tooltip("Touch Callback")]
    protected UnityEvent<TOUCH_RESULT_TYPE, Rect> Touch_Tooltip_Callback;

    //protected override UnityEventBase Touch_Callback => Touch_Tooltip_Callback;

    public void AddTouchCallback(UnityAction<TOUCH_RESULT_TYPE, Rect> cb)
    {
        Touch_Tooltip_Callback.AddListener(cb);
    }

    public void RemovTouchCallback(UnityAction<TOUCH_RESULT_TYPE, Rect> cb)
    {
        Touch_Tooltip_Callback.RemoveListener(cb);
    }
}

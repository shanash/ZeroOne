using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RewardItemCard : RewardCardBase
{
    [SerializeField, Tooltip("Recv Complete Check Box")]
    protected RectTransform Recv_Complete;

    [SerializeField, Tooltip("Button")]
    UIInteractiveButton _Button;

    Vector2 Init_Scale = new Vector2(0.66f, 0.66f);

    public UIInteractiveButton Button => _Button;

    public override void InitializeData(Reward_Set_Data d, UnityAction<TOUCH_RESULT_TYPE, Func<bool, Rect>, object> callback_tooltip)
    {
        base.InitializeData(d, callback_tooltip);

        if (Button != null)
        {
            Button.Tooltip_Data = Data;
            Button.Touch_Tooltip_Callback.AddListener(callback_tooltip);
        }
    }

    public void SetRecvComplete(bool complete)
    {
        Recv_Complete.gameObject.SetActive(complete);
    }

    public void SetScale(Vector2 scale)
    {
        this.transform.localScale = scale;
    }


    public override void Spawned()
    {
        base.Spawned();
        Recv_Complete.gameObject.SetActive(false);

        this.transform.localScale = Init_Scale;
    }

}

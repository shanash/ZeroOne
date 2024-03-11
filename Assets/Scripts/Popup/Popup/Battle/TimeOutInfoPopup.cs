using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeOutInfoPopup : PopupBase
{
    [SerializeField, Tooltip("Time Out")]
    TMP_Text Time_Out_Text;

    float Delay_Time;
    bool Is_Action;

    protected override bool Initialize(object[] data)
    {
        if (data == null || data.Length != 1)
        {
            return false;
        }
        Delay_Time = (float)data[0];
        SetEnableEscKeyExit(false);
        return true;
    }

    protected override void ShowPopupAniEndCallback()
    {
        Is_Action = true;
    }

    protected override void OnUpdatePopup()
    {
        if (Is_Action)
        {
            Delay_Time -= Time.deltaTime;
            if (Delay_Time < 0f)
            {
                HidePopup();
                Is_Action = false;
            }
        }
    }
}

using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveInfoUI : PopupBase
{
    [SerializeField, Tooltip("Wave Info Text")]
    TMP_Text Wave_Info_Text;

    Wave_Data Data;

    float Delay_Time;
    bool Is_Action;

    public override void ShowPopup(params object[] data)
    {
        if (data == null || data.Length < 2)
        {
            HidePopup();
            return;
        }
        Data = (Wave_Data)data[0];
        Delay_Time = (float)data[1];
        base.ShowPopup(data);


    }

    protected override void ShowPopupAniEndCallback()
    {
        Is_Action = true;
        FixedUpdatePopup();
    }


    protected override void FixedUpdatePopup()
    {
        Wave_Info_Text.text = $"Wave {Data.wave_sequence}";
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

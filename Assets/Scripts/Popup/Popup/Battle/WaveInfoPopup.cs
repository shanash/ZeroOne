using FluffyDuck.UI;
using TMPro;
using UnityEngine;

public class WaveInfoPopup : PopupBase
{
    [SerializeField, Tooltip("Wave Info Text")]
    TMP_Text Wave_Info_Text;

    Wave_Data Data;

    float Delay_Time;
    bool Is_Action;

    protected override bool Initialize(object[] data)
    {
        if (data == null || data.Length < 2)
        {
            return false;
        }
        Data = (Wave_Data)data[0];
        Delay_Time = (float)data[1];
        SetEnableEscKeyExit(false);
        return true;
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

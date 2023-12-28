using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;

public class NotiTimerPopup : PopupBase
{
    [SerializeField, Tooltip("Message")]
    TMP_Text Message_Text;

    float Duration;

    bool Show_Message;

    public override void ShowPopup(params object[] data)
    {
        if (data.Length < 2)
        {
            Duration = 3f;
        }
        else
        {
            Duration = (float)data[0];
        }
        Show_Message = true;
        string msg = (string)data[1];
        Message_Text.text = msg;

        SetEnableEscKeyExit(false);
        base.ShowPopup(data);
    }


    protected override void OnUpdatePopup()
    {
        if (Show_Message)
        {
            Duration -= Time.deltaTime;
            if (Duration < 0)
            {
                HidePopup();
                Show_Message = false;
            }
        }
        
    }
}

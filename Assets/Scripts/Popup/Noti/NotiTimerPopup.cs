using FluffyDuck.UI;
using TMPro;
using UnityEngine;

public class NotiTimerPopup : PopupBase
{
    [SerializeField, Tooltip("Message")]
    TMP_Text Message_Text;

    float Duration;

    bool Show_Message;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2)
        {
            return false;
        }

        Show_Message = true;
        Duration = (float)data[0];
        string msg = (string)data[1];
        Message_Text.text = msg;

        SetEnableEscKeyExit(false);

        return true;
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

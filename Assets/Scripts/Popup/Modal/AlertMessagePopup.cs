using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertMessagePopup : PopupBase
{
    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("Message")]
    TMP_Text Message;

    protected override bool Initialize(object[] data)
    {
        if (data == null)
        {
            return false;
        }

        if (data.Length == 1)
        {
            //  title
            Popup_Title.text = GameDefine.GetLocalizeString("system_information");
            //  message
            Message.text = (string)data[0];
        }
        else if (data.Length == 2)
        {
            //  title
            Popup_Title.text = (string)data[0];
            //  message
            Message.text = (string)data[1];
        }
        else
        {
            Debug.Assert(false);
        }

        SetEnableEscKeyExit(false);

        return true;
    }

    /// <summary>
    /// 이어 하기
    /// </summary>
    public void OnClickDim()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        HidePopup();
    }
}

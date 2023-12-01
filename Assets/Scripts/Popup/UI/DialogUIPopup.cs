using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogUIPopup : PopupBase
{
    [SerializeField, Tooltip("Talker")]
    TMP_Text Talker_Text;
    [SerializeField, Tooltip("Message")]
    TMP_Text Message_Text;

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);
    }
}

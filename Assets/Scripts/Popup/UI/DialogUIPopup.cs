using FluffyDuck.UI;
using TMPro;
using UnityEngine;

public class DialogUIPopup : PopupBase
{
    [SerializeField, Tooltip("Talker")]
    TMP_Text Talker_Text;
    [SerializeField, Tooltip("Message")]
    TMP_Text Message_Text;
}

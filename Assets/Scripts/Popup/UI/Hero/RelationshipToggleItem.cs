
using FluffyDuck.UI;
using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RelationshipToggleItem : InfiniteScrollItem
{
    [SerializeField]
    TMP_Text UI_Text;

    [SerializeField]
    Toggle UI_Toggle;

    [SerializeField]
    GameObject Diable_Msg_Button;

    [SerializeField]
    Image UI_Enable;

    [SerializeField]
    GameObject UI_Selected;

    RelationshipToggleItemData Data;

    public override void UpdateData(InfiniteScrollData scroll_data)
    {
        base.UpdateData(scroll_data);

        Data = (RelationshipToggleItemData)scroll_data;
        UI_Text.text = Data.Subject;
        UI_Toggle.group = Data.Toggle_Group;
        Diable_Msg_Button.SetActive(!Data.Enable);
        UI_Enable.color = Data.Enable ? Color.white : Color.grey;
        UI_Selected.SetActive(Data.Selected);
    }

    public void OnToggleChanged(bool isOn)
    {
        Data.OnToggleChanged(Data.Index, isOn);
    }

    public void OnClickDisableMsg()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Hero.FormatLimitLoveLevel(Data.Index));
        });
    }
}

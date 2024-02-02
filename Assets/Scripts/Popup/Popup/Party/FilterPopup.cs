using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterPopup : PopupBase
{
    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("Cancel Btn")]
    UIButtonBase Cancel_Btn;
    [SerializeField, Tooltip("Confirm Btn")]
    UIButtonBase Confirm_Btn;

    [SerializeField, Tooltip("Toggle Group")]
    ToggleGroup Toggle_Group;

    [SerializeField, Tooltip("Toggle Btn List")]
    List<FilterRadioBtn> Filter_Radio_Btn_List;

    CHARACTER_SORT Filter_Type = CHARACTER_SORT.NAME;

    protected override bool Initialize(object[] data)
    {
        Filter_Type = (CHARACTER_SORT)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, CHARACTER_SORT.NAME);
        FixedUpdatePopup();

        return true;
    }

    protected override void FixedUpdatePopup()
    {
        var found_filter = Filter_Radio_Btn_List.Find(x => x.GetCharacterFilterType() == Filter_Type);
        if (found_filter != null)
        {
            found_filter.SetFilterOn();
        }
    }

    public void OnClickCancel()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public void OnClickConfirm()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        var select_toggle = Toggle_Group.ActiveToggles().FirstOrDefault();
        if (select_toggle != null)
        {
            var filter_btn = select_toggle.GetComponent<FilterRadioBtn>();
            if (filter_btn != null)
            {
                GameConfig.Instance.SetGameConfig<CHARACTER_SORT>(GAME_CONFIG_KEY.CHARACTER_FILTER_TYPE, filter_btn.GetCharacterFilterType());
            }
        }
        HidePopup();
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Ease_Base != null)
        {
            Ease_Base.transform.localScale = new Vector2(0f, 0f);
        }

    }
}

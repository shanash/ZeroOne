using FluffyDuck.UI;
using TMPro;
using UnityEngine;

public class EssenceInfoPopup : PopupBase
{
    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("Essence Info Desc")]
    TMP_Text Essence_Info_Desc;

    protected override bool Initialize(object[] data)
    {
        FixedUpdatePopup();
        return true;
    }

    protected override void FixedUpdatePopup()
    {
        Popup_Title.text = GameDefine.GetLocalizeString("system_essence_info_title");
        Essence_Info_Desc.text = GameDefine.GetLocalizeString("system_essence_info_desc");
    }

    public void OnClickDim()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}

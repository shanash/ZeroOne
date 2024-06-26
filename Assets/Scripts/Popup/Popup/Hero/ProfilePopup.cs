using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePopup : PopupBase
{
    [SerializeField]
    Image Photo;

    [SerializeField]
    TMP_Text Name;

    [SerializeField]
    TMP_Text Age_Subject;

    [SerializeField, Tooltip("not only int")]
    TMP_Text Age;

    [SerializeField]
    TMP_Text Birthday_Subject;

    [SerializeField, Tooltip("not only int")]
    TMP_Text Birthday;

    [SerializeField]
    TMP_Text Height_Subject;

    [SerializeField, Tooltip("not only int")]
    TMP_Text Height;

    [SerializeField]
    TMP_Text Hobby_Subject;

    [SerializeField]
    TMP_Text Hobby;

    [SerializeField, Tooltip("Descriptions of the character")]
    TMP_Text Descriptions;

    Player_Character_Data Data;

    void Reset()
    {
        Data = null;
    }

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1 || data[0] is not Player_Character_Data)
        {
            return false;
        }

        Data = data[0] as Player_Character_Data;

        FixedUpdatePopup();

        return true;
    }

    protected override void FixedUpdatePopup()
    {
        Age_Subject.text = GameDefine.GetLocalizeString("system_age");
        Birthday_Subject.text = GameDefine.GetLocalizeString("system_birthday");
        Height_Subject.text = GameDefine.GetLocalizeString("system_height");
        Hobby_Subject.text = GameDefine.GetLocalizeString("system_hobby");

        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.icon_path, (spr) =>
        {
            Photo.sprite = spr;
        });
        

        Name.text = GameDefine.GetLocalizeString(Data.name_id);
        Age.text = ZString.Format(GameDefine.GetLocalizeString("system_age_format"), Data.profile_age);
        Birthday.text = ZString.Format(GameDefine.GetLocalizeString("system_birthday_format"), Data.profile_birthday[0], Data.profile_birthday[1]);
        Height.text = ZString.Format(GameDefine.GetLocalizeString("system_height_format"), Data.profile_high);
        Hobby.text = Data.profile_habby;
        Descriptions.text = Data.script;
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
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

    public override void Spawned()
    {
        base.Spawned();
        Reset();
    }
}

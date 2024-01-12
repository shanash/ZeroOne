using FluffyDuck.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePopup : PopupBase
{
    [SerializeField]
    Image Photo;

    [SerializeField]
    TMP_Text Name;

    [SerializeField, Tooltip("not only int")]
    TMP_Text Age;

    [SerializeField, Tooltip("not only int")]
    TMP_Text Birthdate;

    [SerializeField, Tooltip("not only int")]
    TMP_Text Height;

    [SerializeField]
    TMP_Text Hobby;

    [SerializeField, Tooltip("Descriptions of the character")]
    TMP_Text Descriptions;

    Player_Character_Data Data;

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);

        if (data.Length != 1 || data[0] is not Player_Character_Data)
        {
            Debug.Assert(false, $"잘못된 ProfilePopup 팝업 호출!!");
            HidePopup();
            return;
        }

        Data = data[0] as Player_Character_Data;

        FixedUpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        Name.text = Data.name_kr;
        Age.text = ConstString.FormatHeroAge(Data.profile_age);
        Birthdate.text = ConstString.FormatHeroBirthdate(Data.profile_birthday);
        Height.text = ConstString.FormatHeroHeight(Data.profile_high);
        Hobby.text = Data.profile_habby;

        Descriptions.text = Data.script;
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
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

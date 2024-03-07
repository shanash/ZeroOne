using FluffyDuck.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : PopupBase
{
    [SerializeField, Tooltip("제목 텍스트")]
    TMP_Text Title = null;

    [SerializeField, Tooltip("메시지 텍스트")]
    TMP_Text Message = null;

    protected override bool Initialize(object[] data)
    {
        if (data.Length < 2 || data[0] is not string || data[1] is not string)
        {
            return false;
        }

        Title.text = data[0] as string;
        Message.text = data[1] as string;

        return true;
    }

    protected void SetContainerScale(float scale)
    {
        Ease_Base.transform.localScale = new Vector3(scale, scale, 1);
    }

    public void OnClick(UIButtonBase button)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}

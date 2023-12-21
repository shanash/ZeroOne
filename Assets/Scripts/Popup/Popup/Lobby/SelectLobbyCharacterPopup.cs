using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLobbyCharacterPopup : PopupBase
{
    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);
    }


    public void OnClickConfirm()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        HidePopup();
    }
}

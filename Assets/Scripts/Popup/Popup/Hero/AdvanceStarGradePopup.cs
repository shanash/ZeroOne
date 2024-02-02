using FluffyDuck.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdvanceStarGradePopup : PopupBase
{
    [SerializeField, Tooltip("팝업창 제목")]
    TMP_Text Title = null;

    Action Confirm = null;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2 || data[0] is not Action || data[1] is not UserHeroData)
        {
            return false;
        }

        FixedUpdatePopup();

        return true;
    }

    protected override void FixedUpdatePopup()
    {
    }

    public void OnClickComfirm()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Confirm?.Invoke();
        HidePopup();
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}

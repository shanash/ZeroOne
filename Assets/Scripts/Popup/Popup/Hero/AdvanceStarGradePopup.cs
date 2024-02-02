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

    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);

        if (data.Length != 2 || data[0] is not Action || data[1] is not List<StatusItemData>)
        {
            Debug.Assert(false, $"잘못된 ProfilePopup 팝업 호출!!");
            HidePopup();
            return;
        }

        FixedUpdatePopup();
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

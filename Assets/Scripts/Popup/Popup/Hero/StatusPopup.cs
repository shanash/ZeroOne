using FluffyDuck.UI;
using FluffyDuck.Util;
using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusPopup : PopupBase
{
    [SerializeField, Tooltip("팝업창 제목")]
    TMP_Text Title;

    [SerializeField, Tooltip("Status List View")]
    InfiniteScroll Status_LIst_View;

    List<StatusItemData> Datas = null;

    void Reset()
    {
        Datas = null;
    }

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2 || data[0] is not string || data[1] is not List<StatusItemData>)
        {
            return false;
        }

        Title.text = data[0] as string;
        Datas = data[1] as List<StatusItemData>;

        FixedUpdatePopup();

        return true;
    }

    protected override void FixedUpdatePopup()
    {
        Status_LIst_View.Clear();

        foreach (var data in Datas)
        {
            Status_LIst_View.InsertData(data);
        }
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}

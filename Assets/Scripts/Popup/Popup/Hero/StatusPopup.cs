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
    int Focus_Index = 0;

    void Reset()
    {
        Datas = null;
    }

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 3 || data[0] is not string || data[1] is not List<StatusItemData>)
        {
            return false;
        }

        Title.text = data[0] as string;
        Datas = data[1] as List<StatusItemData>;
        Focus_Index = (int)data[2];

        return true;
    }

    protected override void ShowPopupAniEndCallback()
    {
        Status_LIst_View.gameObject.SetActive(true);

        foreach (var data in Datas)
        {
            Status_LIst_View.InsertData(data);
        }

        if (Focus_Index > -1)
        {
            Status_LIst_View.MoveTo(Focus_Index, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        }
    }

    protected override void OnUpdatePopup()
    {
        //Debug.Log($"Status_LIst_View : {Status_LIst_View.GetScrollPosition()}");
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public override void Despawned()
    {
        Status_LIst_View.Clear();
        Status_LIst_View.gameObject.SetActive(false);
    }
}

using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStageUI : PopupBase
{
    [Space()]
    [SerializeField, Tooltip("Stage List View")]
    InfiniteScroll Stage_List_View;

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);
    }
}

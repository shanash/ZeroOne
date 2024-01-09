using FluffyDuck.UI;
using UnityEngine;

public class TestSlidePopup : PopupBase
{
    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);
    }

    protected override void ShowPopupAniEndCallback()
    {
        Debug.Log("ShowPopupAniEndCallback");
    }


}

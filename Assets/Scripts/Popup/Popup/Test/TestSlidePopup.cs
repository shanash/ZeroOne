using FluffyDuck.UI;
using UnityEngine;

public class TestSlidePopup : PopupBase
{
    protected override void ShowPopupAniEndCallback()
    {
        Debug.Log("ShowPopupAniEndCallback");
    }
}

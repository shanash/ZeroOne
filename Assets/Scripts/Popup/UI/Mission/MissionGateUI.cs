using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGateUI : PopupBase
{

    public void OnClickMainMission()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/SelectStageUI", (popup) =>
        {
            popup.ShowPopup();
        });
    }

    public void OnClickSpecialMission()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
    public void OnClickSearch()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
    public void OnClickDungeon()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
    public void OnClickClanBattle()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
    public void OnClickBossBattle()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", (popup) =>
        {
            popup.ShowPopup(3f, "준비중 입니다.");
        });
    }
}

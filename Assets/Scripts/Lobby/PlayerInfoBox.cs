using Cysharp.Text;
using FluffyDuck.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBox : MonoBehaviour
{
    [SerializeField, Tooltip("BG")]
    Image Player_Info_BG;

    [SerializeField, Tooltip("Level Text")]
    TMP_Text Player_Level_Text;

    [SerializeField, Tooltip("Nickname Text")]
    TMP_Text Player_Nickname_Text;


    [SerializeField, Tooltip("Exp Bar")]
    Image Exp_Bar;

    [SerializeField, Tooltip("Exp Current Text. Current Color #80BCFF")]
    TMP_Text Exp_Point;


    private void Start()
    {
        UpdatePlayerInfo();
    }

    void UpdateEventCallback(UPDATE_EVENT_TYPE etype)
    {
        if (etype == UPDATE_EVENT_TYPE.UPDATE_TOP_PLAYER_INFO)
        {
            UpdatePlayerInfo();
        }
    }

    void UpdatePlayerInfo()
    {
        var gd = GameData.Instance;
        var player_info_mng = gd.GetUserGameInfoDataManager();
        var player_info = player_info_mng.GetCurrentPlayerInfoData();

        //  level
        Player_Level_Text.text = player_info.GetLevel().ToString();
        //  nick

        //  exp     <color=#80BCFF>700</color> / 1,000
        double cur_exp = player_info.GetLevelExp();
        double need_exp = player_info.GetNextExp();
        Exp_Point.text = ZString.Format("<color=#80BCFF>{0:N0}</color> / {1:N0}", cur_exp, need_exp);
        //  exp gauge
        float per = player_info.GetExpPercentage();
        Exp_Bar.fillAmount = per;
    }

    private void OnEnable()
    {
        var evt_dispatcher = UpdateEventDispatcher.Instance;
        evt_dispatcher.AddEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_PLAYER_INFO, UpdateEventCallback);
    }
    private void OnDisable()
    {
        if (UpdateEventDispatcher.Instance == null)
        {
            return;
        }
        var evt_dispatcher = UpdateEventDispatcher.Instance;
        evt_dispatcher.RemoveEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_PLAYER_INFO, UpdateEventCallback);
    }

    public void OnClickPlayerProfile()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

    }

}

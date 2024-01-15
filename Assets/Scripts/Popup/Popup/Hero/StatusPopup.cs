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

    [SerializeField]
    StatusTextUI Status_Base;

    BattleUnitData Data;
    List<StatusTextUI> Statuses;

    [SerializeField, Tooltip("Status List View")]
    InfiniteScroll Status_LIst_View;

    protected override void Initialize()
    {
        base.Initialize();
        Data = null;
        Statuses = new List<StatusTextUI>();
    }

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);

        if (data.Length != 1 || data[0] is not BattleUnitData)
        {
            Debug.Assert(false, $"잘못된 ProfilePopup 팝업 호출!!");
            HidePopup();
            return;
        }

        Data = data[0] as BattleUnitData;

        FixedUpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        Title.text = ConstString.StatusPopup.TITLE;

        Queue<StatusItemData> status_queue = new Queue<StatusItemData>();
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.COMBAT_POWER, Data.GetCombatPoint().ToString("N0")));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.LIFE_POINT, Data.GetLifePoint().ToString("N0")));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.ATTACK_POWER, Data.GetAttackPoint().ToString("N0")));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.DEFENCE_POINT, Data.GetDefensePoint().ToString("N0")));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.APPROACH_DISTANCE, Data.GetApproachDistance().ToString("N0")));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.ATTACK_RECOVERY, Data.GetAttackRecovery().ToPercentage()));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.EVASION_POINT, Data.GetEvationPoint().ToString("N0")));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.ACCURACY_POINT, Data.GetAccuracyPoint().ToString("N0")));
        status_queue.Enqueue(new StatusItemData(ConstString.Hero.AUTO_RECORVERY, Data.GetAutoRecoveryLife().ToPercentage()));

        while (status_queue.TryDequeue(out StatusItemData data))
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

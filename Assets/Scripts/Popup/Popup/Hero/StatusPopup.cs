using FluffyDuck.UI;
using FluffyDuck.Util;
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

        (string subject, string value)[] tuple_status_data = new (string, string)[]
        {
            (ConstString.Hero.COMBAT_POWER, Data.GetCombatPoint().ToString("N0")),
            (ConstString.Hero.LIFE_POINT, Data.GetLifePoint().ToString("N0")),
            (ConstString.Hero.ATTACK_POWER, Data.GetAttackPoint().ToString("N0")),
            (ConstString.Hero.DEFENCE_POINT, Data.GetDefensePoint().ToString("N0")),
            (ConstString.Hero.APPROACH_DISTANCE, Data.GetApproachDistance().ToString("N0")),
            (ConstString.Hero.ATTACK_RECOVERY, Data.GetAttackRecovery().ToPercentage()),
            (ConstString.Hero.EVASION_POINT, Data.GetEvationPoint().ToString("N0")),
            (ConstString.Hero.ACCURACY_POINT, Data.GetAccuracyPoint().ToString("N0")),
            (ConstString.Hero.AUTO_RECORVERY, Data.GetAutoRecoveryLife().ToPercentage()),
        };

        while (Statuses.Count < tuple_status_data.Length)
        {
            var ui = Instantiate(Status_Base, Status_Base.transform.parent, false);

            ui.Subject.text = tuple_status_data[Statuses.Count].subject;
            ui.Value.text = tuple_status_data[Statuses.Count].value;

            ui.gameObject.SetActive(true);
            Statuses.Add(ui);
        }
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}

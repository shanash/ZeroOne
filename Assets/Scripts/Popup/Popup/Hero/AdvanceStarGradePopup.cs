using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdvanceStarGradePopup : PopupBase
{
    //[SerializeField, Tooltip("팝업창 제목")]
    //TMP_Text Title = null;
    [SerializeField, Tooltip("List View")]
    InfiniteScroll Status_LIst_View;

    BattlePcData Battle_Pc_Data = null;
    BattlePcData After_Advance_Battle_Pc_Data = null;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1 || data[0] is not BattlePcData)
        {
            return false;
        }

        Battle_Pc_Data = (BattlePcData)data[0];

        FixedUpdatePopup();

        return true;
    }

    protected override void FixedUpdatePopup()
    {
        Status_LIst_View.Clear();

        After_Advance_Battle_Pc_Data = Battle_Pc_Data.GetNextStarGradeData();
        List<StatusItemData> list = new List<StatusItemData>
            {
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_physics_attack"), $"{Battle_Pc_Data.GetPhysicsAttackPoint():N0} -> {After_Advance_Battle_Pc_Data.GetPhysicsAttackPoint():N0}"),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_magic_attack"), $"{Battle_Pc_Data.GetMagicAttackPoint():N0} -> {After_Advance_Battle_Pc_Data.GetMagicAttackPoint():N0}"),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_physics_defence"), $"{Battle_Pc_Data.GetPhysicsDefensePoint():N0} -> {After_Advance_Battle_Pc_Data.GetPhysicsDefensePoint():N0}"),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_magic_defence"), $"{Battle_Pc_Data.GetMagicDefensePoint():N0} -> {After_Advance_Battle_Pc_Data.GetMagicDefensePoint():N0}"),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_evasion"), $"{Battle_Pc_Data.GetEvasionPoint():N0} -> {After_Advance_Battle_Pc_Data.GetEvasionPoint():N0}"),
                new StatusItemData(GameDefine.GetLocalizeString("system_stat_accuracy"), $"{Battle_Pc_Data.GetAccuracyPoint():N0} -> {After_Advance_Battle_Pc_Data.GetAccuracyPoint():N0}"),
            };

        foreach (var data in list)
        {
            Status_LIst_View.InsertData(data);
        }
    }

    public void OnClickComfirm()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Closed_Delegate?.Invoke();
        HidePopup();
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public void OnClickDim()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}

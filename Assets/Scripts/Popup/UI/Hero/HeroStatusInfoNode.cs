using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroStatusInfoNode : MonoBehaviour
{
    public enum HERO_STATUS_TYPE
    {
        NONE = 0,
        LIFE,
        PHYSICS_ATTACK,
        MAGIC_ATTACK,
        PHYSICS_DEFENSE,
        MAGIC_DEFENSE,
    }

    [SerializeField, Tooltip("Status Type")]
    HERO_STATUS_TYPE Status_Type = HERO_STATUS_TYPE.NONE;

    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Status Title")]
    TMP_Text Status_Title;
    [SerializeField, Tooltip("Current Status Value")]
    TMP_Text Current_Status_Value;
    [SerializeField, Tooltip("Result Status Value")]
    TMP_Text Result_Status_Value;

    BattlePcData Cur_Unit_Data;
    BattlePcData Result_Unit_Data;

    public void SetBattleUnitData(BattlePcData cur, BattlePcData result)
    {
        Cur_Unit_Data = cur;
        Result_Unit_Data= result;
        UpdateStatus();
    }

    void UpdateStatus()
    {
        if (Cur_Unit_Data == null || Result_Status_Value == null)
        {
            return;
        }
        switch (Status_Type)
        {
            case HERO_STATUS_TYPE.LIFE:
                UpdateStatusLife();
                break;
            case HERO_STATUS_TYPE.PHYSICS_ATTACK:
                UpdateStatusPhysicsAttack();
                break;
            case HERO_STATUS_TYPE.MAGIC_ATTACK:
                UpdateStatusMagicAttack();
                break;
            case HERO_STATUS_TYPE.PHYSICS_DEFENSE:
                UpdateStatusPhysicsDefense();
                break;
            case HERO_STATUS_TYPE.MAGIC_DEFENSE:
                UpdateStatusMagicDefense();
                break;
            default:
                Debug.Assert(false);
                break;
        }

    }

    void UpdateStatusLife()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_life");

        //  cur value
        Current_Status_Value.text = Cur_Unit_Data.GetMaxLifePoint().ToString("N0");

        //  result value
        Result_Status_Value.text = Result_Unit_Data.GetMaxLifePoint().ToString("N0");
    }
    void UpdateStatusPhysicsAttack()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_physics_attack");
        //  cur value
        Current_Status_Value.text = Cur_Unit_Data.GetPhysicsAttackPoint().ToString("N0");

        //  result value
        Result_Status_Value.text = Result_Unit_Data.GetPhysicsAttackPoint().ToString("N0");
    }
    void UpdateStatusMagicAttack()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_magic_attack");
        //  cur value
        Current_Status_Value.text = Cur_Unit_Data.GetMagicAttackPoint().ToString("N0");

        //  result value
        Result_Status_Value.text = Result_Unit_Data.GetMagicAttackPoint().ToString("N0");
    }
    void UpdateStatusPhysicsDefense()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_physics_defence");
        //  cur value
        Current_Status_Value.text = Cur_Unit_Data.GetPhysicsDefensePoint().ToString("N0");

        //  result value
        Result_Status_Value.text = Result_Unit_Data.GetPhysicsDefensePoint().ToString("N0");
    }
    void UpdateStatusMagicDefense()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_magic_defence");
        //  cur value
        Current_Status_Value.text = Cur_Unit_Data.GetMagicDefensePoint().ToString("N0");

        //  result value
        Result_Status_Value.text = Result_Unit_Data.GetMagicDefensePoint().ToString("N0");
    }
}

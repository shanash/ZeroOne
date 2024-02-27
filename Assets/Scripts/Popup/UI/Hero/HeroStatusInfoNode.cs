using Cysharp.Text;
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
        ACCURACY,
        PHYSICS_CRITICAL_CHANCE,
        MAGIC_CRITICAL_CHANCE,
        PHYSICS_CRITICAL_ADD,
        MAGIC_CRITICAL_ADD,
        EVASION,
        HEAL,
        VAMPIRE,
        RESIST,

    }

    [SerializeField, Tooltip("Status Type")]
    HERO_STATUS_TYPE Status_Type = HERO_STATUS_TYPE.NONE;

    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("스탯 이름")]
    TMP_Text Status_Title;
    [SerializeField, Tooltip("현재 스탯")]
    TMP_Text Current_Status_Value;

    [SerializeField, Tooltip("추가 스탯")]
    TMP_Text Add_Status_Value;

    [SerializeField, Tooltip("상승시 결과 스탯")]
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
            case HERO_STATUS_TYPE.ACCURACY:
                UpdateStatusAccuracy();
                break;
            case HERO_STATUS_TYPE.PHYSICS_CRITICAL_CHANCE:
                UpdateStatusPhysicsCriticalChance();
                break;
            case HERO_STATUS_TYPE.MAGIC_CRITICAL_CHANCE:
                UpdateStatusMagicCriticalChance();
                break;
            case HERO_STATUS_TYPE.PHYSICS_CRITICAL_ADD:
                UpdateStatusPhysicsCriticalPowerAdd();
                break;
            case HERO_STATUS_TYPE.MAGIC_CRITICAL_ADD:
                UpdateStatusMagicCriticalPowerAdd();
                break;
            case HERO_STATUS_TYPE.EVASION:
                UpdateStatusEvasion();
                break;
            case HERO_STATUS_TYPE.HEAL:
                UpdateStatusHeal();
                break;
            case HERO_STATUS_TYPE.VAMPIRE:
                UpdateStatusVampire();
                break;
            case HERO_STATUS_TYPE.RESIST:
                UpdateStatusResist();
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

        double cur_val = Cur_Unit_Data.GetMaxLifePoint();
        double result_val = Result_Unit_Data.GetMaxLifePoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");

        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }
    }
    void UpdateStatusPhysicsAttack()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_physics_attack");
        double cur_val = Cur_Unit_Data.GetPhysicsAttackPoint();
        double result_val = Result_Unit_Data.GetPhysicsAttackPoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }
    }
    void UpdateStatusMagicAttack()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_magic_attack");

        double cur_val = Cur_Unit_Data.GetMagicAttackPoint();
        double result_val = Result_Unit_Data.GetMagicAttackPoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }
    }
    void UpdateStatusPhysicsDefense()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_physics_defence");
        double cur_val = Cur_Unit_Data.GetPhysicsDefensePoint();
        double result_val = Result_Unit_Data.GetPhysicsDefensePoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusMagicDefense()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_magic_defence");
        double cur_val = Cur_Unit_Data.GetMagicDefensePoint();
        double result_val = Result_Unit_Data.GetMagicDefensePoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }
    }

    void UpdateStatusAccuracy()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_accuracy");
        double cur_val = Cur_Unit_Data.GetAccuracyPoint();
        double result_val = Result_Unit_Data.GetAccuracyPoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusPhysicsCriticalChance()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_physics_critical_chance");
        double cur_val = Cur_Unit_Data.GetPhysicsCriticalChance();
        double result_val = Result_Unit_Data.GetPhysicsCriticalChance();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusMagicCriticalChance()
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_magic_critical_chance");
        double cur_val = Cur_Unit_Data.GetMagicCriticalChance();
        double result_val = Result_Unit_Data.GetMagicCriticalChance();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusPhysicsCriticalPowerAdd() 
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_physics_critical_power_add");
        double cur_val = Cur_Unit_Data.GetPhysicsCriticalPowerAdd();
        double result_val = Result_Unit_Data.GetPhysicsCriticalPowerAdd();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusMagicCriticalPowerAdd() 
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_magic_critical_power_add");
        double cur_val = Cur_Unit_Data.GetMagicCriticalPowerAdd();
        double result_val = Result_Unit_Data.GetMagicCriticalPowerAdd();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }

    void UpdateStatusEvasion() 
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_evasion");
        double cur_val = Cur_Unit_Data.GetEvasionPoint();
        double result_val = Result_Unit_Data.GetEvasionPoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusHeal() 
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_heal");
        double cur_val = Cur_Unit_Data.GetLifeRecoveryInc();
        double result_val = Result_Unit_Data.GetLifeRecoveryInc();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusVampire() 
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_attack_life_recovery");
        double cur_val = Cur_Unit_Data.GetAttackLifeRecovery();
        double result_val = Result_Unit_Data.GetAttackLifeRecovery();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }
    void UpdateStatusResist() 
    {
        //  title
        Status_Title.text = GameDefine.GetLocalizeString("system_stat_resist");
        double cur_val = Cur_Unit_Data.GetResistPoint();
        double result_val = Result_Unit_Data.GetResistPoint();
        //  cur value
        Current_Status_Value.text = cur_val.ToString("N0");

        //  result value
        Result_Status_Value.text = result_val.ToString("N0");
        //  plus value
        if (Add_Status_Value != null)
        {
            double add_value = result_val - cur_val;
            Add_Status_Value.text = ZString.Format("(+{0:N0})", add_value);
        }

    }

}
    



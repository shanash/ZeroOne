using System;
using UnityEngine;

public class BattlePcOnetimeSkillData_LifeRecovery : BattlePcOnetimeSkillData
{
    BattlePcOnetimeSkillData_LifeRecovery() { }

    public override void ExecSkill(BATTLE_SEND_DATA data)
    {
        double recovery_hp = 0;

        int weight = data.Skill.GetEffectWeightValue(data.Effect_Weight_Index);

        BATTLE_SEND_DATA send_data = data.Clone();


        int t_cnt = data.Targets.Count;

        for (int i = 0; i < t_cnt; i++)
        {
            send_data.ClearTargets();

            var t = data.Targets[i];
            send_data.AddTarget(t);

            switch (Data.multiple_type)
            {
                case STAT_MULTIPLE_TYPE.ATTACK_VALUE:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.ATTACK_RATE:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.DEFENSE_VALUE:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.MAX_LIFE:
                    recovery_hp = Data.value;
                    break;
                case STAT_MULTIPLE_TYPE.MAX_LIFE_RATE:
                    recovery_hp = t.Max_Life * Data.multiple;
                    break;
                case STAT_MULTIPLE_TYPE.LIFE:
                    recovery_hp = Data.value;
                    break;
                case STAT_MULTIPLE_TYPE.LIFE_RATE:
                    recovery_hp = t.Life * Data.multiple;
                    break;
                case STAT_MULTIPLE_TYPE.HEAL_RATE:
                    recovery_hp = t.Life_Recovery_Inc * Data.multiple;
                    break;
                case STAT_MULTIPLE_TYPE.HEAL_VALUE:
                    recovery_hp = Data.value;
                    break;
                case STAT_MULTIPLE_TYPE.CRITICAL_CHANCE:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.CRITICAL_POWER_ADD:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.ACCURACY_VALUE:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.EVASION_VALUE:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.DAMAGE:
                    Debug.Assert(false);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            //  비중만큼의 회복력만 준다
            recovery_hp *= (double)weight * 0.01;
            if (recovery_hp < 1)
            {
                recovery_hp = 1;
            }
            t.AddLifeRecovery(Math.Truncate(recovery_hp));
        }
    }
}

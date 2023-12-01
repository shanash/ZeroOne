using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePcOnetimeSkillData_Damage : BattlePcOnetimeSkillData
{
    public override void ExecSkill(BATTLE_SEND_DATA data)
    {
        int weight = data.Skill.GetEffectWeightValue(data.Effect_Weight_Index);

        BATTLE_SEND_DATA send_data = data.Clone();

        int cnt = data.Targets.Count;

        for (int i = 0; i < cnt; i++)
        {
            send_data.ClearTargets();

            var t = data.Targets[i];
            send_data.AddTarget(t);

            switch (Data.multiple_type)
            {
                case STAT_MULTIPLE_TYPE.ATTACK_VALUE:   //  절대값
                    send_data.Damage = Data.value;
                    break;
                case STAT_MULTIPLE_TYPE.ATTACK:
                    send_data.Damage = send_data.Caster.Attack * Data.multiple;
                    break;
                case STAT_MULTIPLE_TYPE.DEFENSE:
                    send_data.Damage = send_data.Caster.Defense * Data.multiple;
                    break;
                case STAT_MULTIPLE_TYPE.MAX_LIFE:
                    send_data.Damage = send_data.Caster.Max_Life * Data.multiple;
                    break;
                case STAT_MULTIPLE_TYPE.LIFE:
                    send_data.Damage = send_data.Caster.Life * Data.multiple;
                    break;
                case STAT_MULTIPLE_TYPE.CRITICAL_RATE:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.CRITICAL_POWER:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.ACCURACY:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.EVASION:
                    Debug.Assert(false);
                    break;
                case STAT_MULTIPLE_TYPE.DAMAGE:
                    Debug.Assert(false);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            //  비중만큼의 데미지만 준다
            double damage = send_data.Damage * (double)weight * 0.01;
            if (damage < 1)
                damage = 1;
            send_data.Damage = Math.Truncate(damage);
            t.AddDamage(send_data);
        }
        
    }
    
}

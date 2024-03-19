using System;
using UnityEngine;

public class BattlePcOnetimeSkillData_MagicDamage : BattlePcOnetimeSkillData
{
    BattlePcOnetimeSkillData_MagicDamage() { }

    public override void ExecSkill(BATTLE_SEND_DATA data)
    {
        int weight = data.Skill.GetEffectWeightValue(data.Effect_Weight_Index);

        BATTLE_SEND_DATA send_data = data.Clone();

        int cnt = data.Targets.Count;
        double damage_point = 0;

        for (int i = 0; i < cnt; i++)
        {
            send_data.ClearTargets();

            var t = data.Targets[i];
            send_data.AddTarget(t);

            switch (GetStatMultipleType())
            {
                case STAT_MULTIPLE_TYPE.ATTACK_RATE:
                    damage_point = send_data.Caster.Magic_Attack * GetMultiple() + GetValue();
                    break;
                case STAT_MULTIPLE_TYPE.DEFENSE_RATE:
                    damage_point = send_data.Caster.Magic_Defense * GetMultiple() + GetValue();
                    break;
                case STAT_MULTIPLE_TYPE.MAX_LIFE_RATE:
                    damage_point = send_data.Caster.Max_Life * GetMultiple() + GetValue();
                    break;
                case STAT_MULTIPLE_TYPE.LIFE_RATE:
                    damage_point = send_data.Caster.Life * GetMultiple() + GetValue();
                    break;
                case STAT_MULTIPLE_TYPE.DAMAGE:
                    Debug.Assert(false);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            //  비중만큼의 데미지만 준다
            double damage = damage_point * (double)weight * 0.01;
            if (damage < 1)
                damage = 1;
            send_data.Magic_Attack_Point = Math.Truncate(damage);
            t.AddDamage(send_data);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNpcDurationSkillData_Poison : BattleNpcDurationSkillData
{
    public override void ExecSkill(BATTLE_SEND_DATA data)
    {
        base.ExecSkill(data);

        BATTLE_SEND_DATA send_data = data.Clone();
        int cnt = data.Targets == null ? 0 : data.Targets.Count;
        for (int i = 0; i < cnt; i++)
        {
            send_data.ClearTargets();

            var t = data.Targets[i];
            send_data.AddTarget(t);

            var clone = (BattleDurationSkillData)Clone();
            clone.SetBattleSendData(send_data);

            data.Targets[i].AddDurationSkillEffect(clone);
        }
    }
}

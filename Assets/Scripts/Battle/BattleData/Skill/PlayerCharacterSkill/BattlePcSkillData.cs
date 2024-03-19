using Cysharp.Text;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class BattlePcSkillData : BattleSkillData, FluffyDuck.Util.Factory.IProduct
{
    Player_Character_Skill_Data Data;

    protected BattlePcSkillData() : base(UNIT_SKILL_TYPE.PC_SKILL) { }

    protected virtual bool Initialize(Player_Character_Skill_Data data)
    {
        SetSkillData(data);
        return true;
    }

    public override void SetSkillID(int skill_id)
    {
        var m = MasterDataManager.Instance;
        SetSkillData(m.Get_PlayerCharacterSkillData(skill_id));
    }

    void SetSkillData(Player_Character_Skill_Data data)
    {
        Data = data;
        //  pc onetime skill
        int len = 0;
        if (Data.onetime_effect_ids != null)
        {
            len = Data.onetime_effect_ids.Length;
            for (int i = 0; i < len; i++)
            {
                int onetime_skill_id = Data.onetime_effect_ids[i];
                if (onetime_skill_id == 0)
                {
                    continue;
                }

                var battle_onetime = BattleSkillDataFactory.CreatePcBattleOnetimeSkillData(onetime_skill_id);
                if (battle_onetime != null)
                {
                    AddOnetimeSkillData(battle_onetime);
                }
            }
        }

        //  pc duration skill
        if (Data.duration_effect_ids != null)
        {
            len = Data.duration_effect_ids.Length;
            for (int i = 0; i < len; i++)
            {
                int duration_skill_id = Data.duration_effect_ids[i];
                if (duration_skill_id == 0)
                {
                    continue;
                }
                var battle_duration = BattleSkillDataFactory.CreatePcBattleDurationSkillData(duration_skill_id);
                if (battle_duration != null)
                {
                    AddDurationSkillData(battle_duration);
                }
            }
        }

        //  pc second target onetime skill
        if (Data.second_target_onetime_effect_ids != null)
        {
            len = Data.second_target_onetime_effect_ids.Length;
            for (int i = 0; i < len; i++)
            {
                int onetime_skill_id = Data.second_target_onetime_effect_ids[i];
                if (onetime_skill_id == 0)
                {
                    continue;
                }
                var battle_onetime = BattleSkillDataFactory.CreatePcBattleOnetimeSkillData(onetime_skill_id);
                if (battle_onetime != null)
                {
                    AddSecondTargetOnetimeSkillData(battle_onetime);
                }
            }
        }
        //  pc second target duration skill
        if (Data.second_target_duration_effect_ids != null)
        {
            len = Data.second_target_duration_effect_ids.Length;
            for (int i = 0; i < len; i++)
            {
                int duration_skill_id = Data.second_target_duration_effect_ids[i];
                if (duration_skill_id == 0)
                {
                    continue;
                }
                var battle_duration = BattleSkillDataFactory.CreatePcBattleDurationSkillData(duration_skill_id);
                if (battle_duration != null)
                {
                    AddSecondTargetDurationSkillData(battle_duration);
                }
            }
        }


        //  스킬 효과 비중 횟수
        Max_Effect_Count = Data.effect_weight.Length;
        Effect_Weight_Index = 0;
    }


    #region Getter

 

    public override BattleSkillData GetExecutableCloneSkillData(string evt_name)
    {
        if (Data.event_name.Equals(evt_name))
        {
            if (Effect_Weight_Index < Max_Effect_Count)
            {
                var clone = (BattleSkillData)Clone();
                clone.SetEffectWeightIndex(Effect_Weight_Index);
                clone.AddFindTargets(GetFindTargets());
                Effect_Weight_Index++;
                return clone;
            }
        }
        return null;
    }

    public override BattleSkillData GetExecutableSkillData(string evt_name)
    {
        if (Data.event_name.Equals(evt_name))
        {
            return this;
        }
        return null;
    }

    public override object GetSkillData()
    {
        return Data;
    }

    public override string GetTriggerEffectPrefabPath()
    {
        return Data.trigger_effect_path;
    }
    

    public override EFFECT_COUNT_TYPE GetEffectCountType()
    {
        return Data.effect_count_type;
    }

    public override TARGET_TYPE GetTargetType()
    {
        return Data.target_type;
    }
    public override TARGET_RULE_TYPE GetTargetRuleType()
    {
        return Data.target_rule_type;
    }
    public override int GetTargetOrder()
    {
        return Data.target_order;
    }
    public override int GetTargetCount()
    {
        return Data.target_count;
    }

    public override float GetTargetRange()
    {
        return (float)Data.target_range;
    }

    public override int GetEffectWeightValue(int weight_index)
    {
        int weight = 100;
        if (weight_index < Data.effect_weight.Length)
        {
            weight = Data.effect_weight[weight_index];
        }
        return weight;
    }


    public override SECOND_TARGET_RULE_TYPE GetSecondTargetRuleType()
    {
        if (Data != null)
        {
            return Data.second_target_rule;
        }
        return SECOND_TARGET_RULE_TYPE.NONE;
    }
    public override int GetMaxSecondTargetCount()
    {
        if (Data != null)
        {
            return Data.max_second_target_count;
        }
        return 0;
    }

    public override double GetSecondTargetRange()
    {
        if (Data != null)
        {
            return Data.second_target_range;
        }
        return 0;
    }

    public override int GetSkillID()
    {
        return Data.pc_skill_id;
    }

    #endregion

    public override string ToString()
    {
        return Data.ToString();
    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        Effect_Weight_Index = 0;
    }

    public override string GetSkillDesc()
    {
        var sb = ZString.CreateStringBuilder();
        string pattern = @"\[[^\]]+\]";
        if (Data.pc_skill_desc_id != null)
        {
            List<object> skill_effect_list = new List<object>();
            skill_effect_list.AddRange(Onetime_Skill_List);
            skill_effect_list.AddRange(Duration_Skill_List);

            for (int i = 0; i < Data.pc_skill_desc_id.Length; i++)
            {
                string skill_desc_id = Data.pc_skill_desc_id[i];
                if (string.IsNullOrEmpty(skill_desc_id))
                {
                    continue;
                }
                string desc = GameDefine.GetLocalizeString(Data.pc_skill_desc_id[i]);
                UnityEngine.Debug.Log(desc);
                MatchCollection matches = Regex.Matches(desc, pattern);
                int cnt = matches.Count;
                if (cnt > 0)
                {
                    for (int c = 0; c < cnt; c++)
                    {
                        var match = matches[c];
                        string replace_str = ConvertSkillValue(match.Value, skill_effect_list[i]);
                        desc = desc.Replace(match.Value, replace_str);
                    }
                }
                sb.AppendLine(desc);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 패턴 문자열에 따라 구분<br/>
    /// [0]: onetime시트의 multiple 값을 백분률(*100)화 하여 출력(피해량 퍼센테이지 표현 등)<br/>
    /// [1]: onetime시트의 value 값을 그대로 출력<br/>
    /// [2]: duriation시트의 rate 값을 백분률(*100)화 하여 출력(상태이상 적중률)<br/>
    /// [3]: duriation시트의 time 값을 출력(상태 지속시간 초)<br/>
    /// [4]: duriation시트의 count 값을 출력(도트 데미지를 몇 회 준다, 등)<br/>
    /// [5]: duriation시트의 multiple 값을 백분률(*100)화 하여 출력<br/>
    /// [6]: duriation시트의 value 값을 출력<br/>
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="skill">일회성 또는 지속성 효과 데이터</param>
    /// <returns></returns>
    string ConvertSkillValue(string pattern, object skill)
    {
        string convert = $"Test => {pattern}";
        BattlePcOnetimeSkillData onetime_skill = null;
        BattlePcDurationSkillData duration_skill = null;

        if (skill is BattleOnetimeSkillData)
        {
            onetime_skill = skill as BattlePcOnetimeSkillData;
        }
        else if (skill is BattleDurationSkillData)
        {
            duration_skill = skill as BattlePcDurationSkillData;
        }

        try
        {
            switch (pattern)
            {
                case "[0]":
                    convert = onetime_skill.GetMultiple().ToPercentage();
                    break;
                case "[1]":
                    convert = onetime_skill.GetValue().ToString("N0");
                    break;
                case "[2]":
                    convert = duration_skill.GetRate().ToPercentage();
                    break;
                case "[3]":
                    convert = duration_skill.GetTime().ToString("N1");
                    break;
                case "[4]":
                    convert = duration_skill.Effect_Count.ToString("N0");
                    break;
                case "[5]":
                    convert = duration_skill.GetMultiple().ToPercentage();
                    break;
                case "[6]":
                    convert = duration_skill.GetValue().ToString("N0");
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
        catch(Exception e)
        {
            UnityEngine.Debug.LogWarning(e);
            convert = $"Failed => {pattern}";
        }

        return convert;
    }


    

    public override object Clone()
    {
        return FluffyDuck.Util.Factory.Instantiate<BattlePcSkillData>(Data);
    }
}

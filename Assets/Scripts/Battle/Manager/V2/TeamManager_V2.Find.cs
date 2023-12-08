using DocumentFormat.OpenXml.Office2010.ExcelAc;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class TeamManager_V2
{
    /// <summary>
    /// 지정한 거리내에 가장 가까운 적이 있는지 지정 숫자만큼 찾아준다
    /// 이 함수의 사용용도는 처음 맵에 진입할 시 타겟 룰에 상관없이
    /// 일정 지역안에 들어와야 하기 때문에 각각의 사거리에 맞춰 가장 가까운 적을 기준으로 맵 진입을 위한 검색임
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_type"></param>
    /// <param name="approach_distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    public void FindTargetInRangeAtApproach(HeroBase_V2 self, TARGET_TYPE target_type, float approach_distance, ref List<HeroBase_V2> targets)
    {
        int count = 1;  //  무조건 1명만 찾는다

        targets.Clear();
        if (target_type == TARGET_TYPE.MY_TEAM)
        {
            FindTargetRuleExec(self, TARGET_RULE_TYPE.APPROACH, approach_distance, count, ref targets);
        }
        else
        {
            var enemy_team = GetEnemyTeam();
            enemy_team.FindTargetRuleExec(self, TARGET_RULE_TYPE.APPROACH, approach_distance, count, ref targets);
        }

        //  요청 타겟수 보다 많을 경우 타겟수 만큼만 반환해준다.
        if (targets.Count > 0)
        {
            targets.RemoveRange(count, targets.Count - count);
        }
    }

    /// <summary>
    /// 지정한 거리내에 있는 적을 찾아 지정한 숫자만큼 찾아준다.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_type"></param>
    /// <param name="approach_distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    public void FindTargetInRange(HeroBase_V2 self, TARGET_TYPE target_type, TARGET_RULE_TYPE rule_type, float approach_distance, int target_order, int count, ref List<HeroBase_V2> targets)
    {
        targets.Clear();

        //  아군에서 찾기
        if (target_type == TARGET_TYPE.MY_TEAM)
        {
            FindTargetRuleExec(self, rule_type, approach_distance, count, ref targets);
        }
        else // 적군에서 찾기
        {
            var enemy_team = GetEnemyTeam();
            enemy_team.FindTargetRuleExec(self, rule_type, approach_distance, count, ref targets);
        }

        //  요청 타겟수 보다 많을 경우 타겟수 만큼만 반환해준다.
        if (targets.Count > 0)
        {
            if (count == 1) //  타겟 수가 1인 경우, 타겟 오더를 이용하여 해당 순번의 타겟을 선택한다.
            {
                if (target_order < targets.Count)
                {
                    var t = targets[target_order];
                    targets.Clear();
                    targets.Add(t);
                }
                else
                {
                    targets.RemoveRange(count, targets.Count - count);
                }
            }
            else
            {
                if (targets.Count > count)
                {
                    targets.RemoveRange(count, targets.Count - count);
                }
            }
        }
        
    }
    /// <summary>
    /// 지정한 거리내에 있는 적을 찾아 지정한 숫자만큼 찾아준다.
    /// 타겟 룰을 배열로 요청할 경우, 우선순위 대로 타겟을 찾아서 반환
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_type"></param>
    /// <param name="rule_types"></param>
    /// <param name="target_order"></param>
    /// <param name="approach_distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    public void FindTargetInRange(HeroBase_V2 self, TARGET_TYPE target_type, TARGET_RULE_TYPE[] rule_types, int target_order, float approach_distance, int count, ref List<HeroBase_V2> targets)
    {
        targets.Clear();
        int len = rule_types.Length;
        //  아군에서 찾기
        if (target_type == TARGET_TYPE.MY_TEAM)
        {
            int cnt = 0;
            //  타겟 룰 타입의 우선순위에 따라 순서대로 타겟을 찾아낸다
            for (int i = 0; i < len; i++)
            {
                TARGET_RULE_TYPE rule = rule_types[i];
                FindTargetRuleExec(self, rule, approach_distance, count, ref targets);
                cnt += targets.Count;
                if (cnt >= count)
                {
                    break;
                }
            }

        }
        else // 적군에서 찾기
        {
            var enemy_team = GetEnemyTeam();
            int cnt = 0;
            //  타겟 룰 타입의 우선순위에 따라 순서대로 적 타겟을 찾아낸다.
            for (int i = 0;i < len; i++)
            {
                TARGET_RULE_TYPE rule = rule_types[i];
                enemy_team.FindTargetRuleExec(self, rule, approach_distance, count, ref targets);
                cnt += targets.Count;
                if(cnt >= count)
                {
                    break;
                }
            }
        }

        //  요청 타겟수 보다 많을 경우 타겟수 만큼만 반환해준다.
        if (targets.Count > 0)
        {
            if (count == 1) //  타겟 수가 1인 경우, 타겟 오더를 이용하여 해당 순번의 타겟을 선택한다.
            {
                if (target_order < targets.Count)
                {
                    var t = targets[target_order];
                    targets.Clear();
                    targets.Add(t);
                }
                else
                {
                    targets.RemoveRange(count, targets.Count - count);
                }
            }
            else
            {
                if (targets.Count > count)
                {
                    targets.RemoveRange(count, targets.Count - count);
                }
            }
        }
    }

   void FindTargetRuleExec(HeroBase_V2 self, TARGET_RULE_TYPE rule_type, float approach_distance, int count, ref List<HeroBase_V2> targets)
    {
        switch (rule_type)
        {
            case TARGET_RULE_TYPE.RANDOM:   //  임의의 타겟을 선택
                FindTargetRuleRandom(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.SELF:
                FindTargetRuleSelf(self, ref targets);
                break;
            case TARGET_RULE_TYPE.ALL:
                FindTargetRuleAll(self, ref targets);
                break;
            case TARGET_RULE_TYPE.ALLY_WITHOUT_ME_NEAREST:
                FindTargetRuleAllyWithouSelfNearest(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.ALLY_WITHOUT_ME_FURTHEST:
                FindTargetRuleAllyWithoutSelfFurthest(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.NEAREST:
                FindTargetRuleNearest(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.FURTHEST:
                FindTargetRuleFurthest(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.APPROACH:
                FindTargetRuleApproach(self, approach_distance, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGHEST_LIFE_VALUE:
                break;
            case TARGET_RULE_TYPE.HIGHEST_LIFE_RATE:
                break;
        }
    }

    /// <summary>
    /// 사거리 내의 임의의 멤버 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleRandom(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        GetTargetsRandomFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 자기 자신을 타겟으로 추가
    /// </summary>
    /// <param name="self"></param>
    /// <param name="targets"></param>
    void FindTargetRuleSelf(HeroBase_V2 self, ref List<HeroBase_V2> targets)
    {
        targets.Add(self);
    }

    void FindTargetRuleAll(HeroBase_V2 self, ref List<HeroBase_V2> targets)
    {
        targets.AddRange(GetAliveMembers());
    }

    /// <summary>
    /// 기준으로부터 가장 가까운 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleNearest(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembersDistanceAsc(self);
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 나를 제외하고 아군중 가장 가까운 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleAllyWithouSelfNearest(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembersDistanceAsc(self);
        temp_list.Remove(self);
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 나를 제외하고 아군 중 가장 먼 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleAllyWithoutSelfFurthest(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembersDistanceDesc(self);
        temp_list.Remove(self);
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 기준으로부터 가장 먼 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleFurthest(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembersDistanceDesc(self);
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 기준으로 부터 접근 거리내에 가장 가까운 타겟.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="approach_distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleApproach(HeroBase_V2 self, float approach_distance, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetInRangeMembersAsc(self, approach_distance);
        temp_list.Remove(self);
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 임의의 멤버들 중 랜덤한 멤버를 추출하기 위한 함수
    /// </summary>
    /// <param name="temp_list"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void GetTargetsRandomFromTempList(List<HeroBase_V2> temp_list, int count, ref List<HeroBase_V2> targets)
    {
        if (temp_list.Count == 0)
        {
            return;
        }
        //  지정된 수 만큼 반환
        int max_count = count;
        int alive_count = temp_list.Count;
        //  살아있는 멤버 수 보다 클 수는 없음
        if (max_count > alive_count)
        {
            max_count = alive_count;
        }
        int cnt = targets.Count;

        while (cnt < max_count)
        {
            int r = UnityEngine.Random.Range(0, temp_list.Count);
            if (r < temp_list.Count)
            {
                HeroBase_V2 hero = temp_list[r];
                if (!targets.Exists(x => object.ReferenceEquals(x, hero)))
                {
                    targets.Add(hero);
                    cnt++;
                }
            }
        }
    }

    /// <summary>
    /// 호출하는 곳에서 이미 순서를 정해두고, 순서대로 원하는 숫자만큼의 멤버를 추가
    /// </summary>
    /// <param name="temp_list"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void GetTargetsFromTempList(List<HeroBase_V2> temp_list, int count, ref List<HeroBase_V2> targets)
    {
        if (temp_list.Count == 0)
        {
            return;
        }
        //  지정한 수 만큼 반환
        int max_count = count;
        int alive_count = temp_list.Count;
        if (max_count > alive_count)
        {
            max_count = alive_count;
        }
        int cnt = targets.Count;
        for (int i = 0; i < alive_count; i++)
        {
            HeroBase_V2 hero = temp_list[i];
            if (!targets.Exists(x => object.ReferenceEquals(x, hero)))
            {
                targets.Add(hero);
                cnt++;
                if (cnt >= max_count)
                {
                    break;
                }
            }
        }
    }
}

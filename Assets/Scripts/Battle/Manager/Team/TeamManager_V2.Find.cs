using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TeamManager_V2
{

    #region Second Find Target Rules
    /// <summary>
    /// 타겟을 중심으로 해당 타겟의 팀 매니저를 이용하여 주변의 다른 타켓을 추가로 찾아주는 함수
    /// 룰 타입에 따라 주변 반경/후위 반경 등 조건에 의해 다양하게 검색한다.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="rule"></param>
    /// <param name="range"></param>
    /// <param name="targets"></param>
    public void FindSecondTargetInRange(HeroBase_V2 center, SECOND_TARGET_RULE_TYPE rule, float range, int max_count, ref List<HeroBase_V2> targets)
    {
        FindSecondTargetRuleExec(center, rule, range, max_count, ref targets);

        //  요청 타겟수 보다 많을 경우 타겟수 만큼만 반환해준다.
        if (targets.Count > 0 && max_count > 0)
        {
            targets.RemoveRange(max_count, targets.Count - max_count);
        }
    }

    /// <summary>
    /// 각 룰 타입에 따라 검색 분기
    /// </summary>
    /// <param name="center"></param>
    /// <param name="rule"></param>
    /// <param name="range"></param>
    /// <param name="targets"></param>
    void FindSecondTargetRuleExec(HeroBase_V2 center, SECOND_TARGET_RULE_TYPE rule, float range, int max_count, ref List<HeroBase_V2> targets)
    {
        switch (rule)
        {
            case SECOND_TARGET_RULE_TYPE.AROUND_SPLASH:
                FindSecondTargetRuleAroundSplash(center, range, max_count, ref targets);
                break;
            case SECOND_TARGET_RULE_TYPE.BACK_SPLASH:
                FindSecondTargetRuleBackSplash(center, range, max_count, ref targets);
                break;
        }
    }
    /// <summary>
    /// center를 중심으로 지정된 영역 내에 있는 모든 타겟 검출
    /// </summary>
    /// <param name="center"></param>
    /// <param name="range"></param>
    /// <param name="targets"></param>
    void FindSecondTargetRuleAroundSplash(HeroBase_V2 center, float range, int max_count, ref List<HeroBase_V2> targets) 
    {
        var members = GetAliveMembers();
        members.Remove(center); //  본인 제외
        //  센터를 중심으로 
        var temp_list = members.FindAll(x => x.GetDistanceFromCenter(center) <= range);

        //  오름 차순
        temp_list.Sort((a, b) => a.GetDistanceFromCenter(center).CompareTo(b.GetDistanceFromCenter(center)));

        GetTargetsFromTempList(temp_list, max_count, ref targets);

    }
    /// <summary>
    /// center를 중심으로 센터 뒷편의 영역에서 지정된 거리내에 있는 모든 타겟 검출
    /// Left 팀인 경우 x가 center 보다 작다.
    /// Right 팀인경우 x가 center 보다 크다
    /// </summary>
    /// <param name="center"></param>
    /// <param name="range"></param>
    /// <param name="max_count"></param>
    /// <param name="targets"></param>
    void FindSecondTargetRuleBackSplash(HeroBase_V2 center, float range, int max_count, ref List<HeroBase_V2> targets) 
    {
        List<HeroBase_V2> temp_list = new List<HeroBase_V2>();
        var members = GetAliveMembers();
        members.Remove(center); //  본인 제외
        if (Team_Type == TEAM_TYPE.LEFT)
        {
            temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x <= center.transform.localPosition.x && x.GetDistanceFromCenter(center) <= range));
        }
        else
        {
            temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x >= center.transform.localPosition.x && x.GetDistanceFromCenter(center) <= range));
        }
        //  오름 차순
        temp_list.Sort((a, b) => a.GetDistanceFromCenter(center).CompareTo(b.GetDistanceFromCenter(center)));

        GetTargetsFromTempList(temp_list, max_count, ref targets);
    }

    #endregion


    #region Find Target Rules
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
            for (int i = 0; i < len; i++)
            {
                TARGET_RULE_TYPE rule = rule_types[i];
                enemy_team.FindTargetRuleExec(self, rule, approach_distance, count, ref targets);
                cnt += targets.Count;
                if (cnt >= count)
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
    /// <summary>
    /// 각 룰타입에 따라 분기해주는 함수
    /// </summary>
    /// <param name="self"></param>
    /// <param name="rule_type"></param>
    /// <param name="approach_distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleExec(HeroBase_V2 self, TARGET_RULE_TYPE rule_type, float approach_distance, int count, ref List<HeroBase_V2> targets)
    {
        switch (rule_type)
        {
            case TARGET_RULE_TYPE.RANDOM:   //  임의의 타겟을 선택
                FindTargetRuleRandom(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.SELF:                     //  자기 자신 선택
                FindTargetRuleSelf(self, ref targets);
                break;
            case TARGET_RULE_TYPE.ALL:                      //  전체 선택
                FindTargetRuleAll(self, ref targets);
                break;
            case TARGET_RULE_TYPE.ALL_WITHOUT_ME:                       //  자신을 뺀 전체 선택(아군일 경우)
                FindTargetRuleAllWithoutMe(self, ref targets);
                break;
            case TARGET_RULE_TYPE.ALLY_WITHOUT_ME_NEAREST:                          //  자신을 뺀 가까운 타겟 선택
                FindTargetRuleAllyWithoutSelfNearest(self, count, ref targets);
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
                FindTargetRuleHighestLifeValue(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGHEST_LIFE_RATE:
                FindTargetRuleHighestLifeRate(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOWEST_LIFE_VALUE:
                FindTargetRuleLowestLifeValue(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOWEST_LIFE_RATE:
                FindTargetRuleLowestLifeRate(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOWEST_ATTACK:
                FindTargetRuleLowestAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOWEST_DEFENSE:
                FindTargetRuleLowestDefense(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGHEST_ATTACK:
                FindTargetRuleHighestAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGHEST_DEFENSE:
                FindTargetRuleHighestDefense(self, count, ref targets);
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
    /// <summary>
    /// 모든 타겟
    /// </summary>
    /// <param name="self"></param>
    /// <param name="targets"></param>
    void FindTargetRuleAll(HeroBase_V2 self, ref List<HeroBase_V2> targets)
    {
        targets.AddRange(GetAliveMembers());
    }

    void FindTargetRuleAllWithoutMe(HeroBase_V2 self, ref List<HeroBase_V2> targets)
    {
        targets.AddRange(GetAliveMembers());
        targets.Remove(self);
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
        var temp_list = GetAliveMembers();
        //  오름 차순
        temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));

        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 나를 제외하고 아군중 가장 가까운 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="distance"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleAllyWithoutSelfNearest(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        temp_list.Remove(self);

        //  오름 차순
        temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));
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
        var temp_list = GetAliveMembers();
        temp_list.Remove(self);

        //  내림 차순
        temp_list.Sort((a, b) => b.GetDistanceFromCenter(self).CompareTo(a.GetDistanceFromCenter(self)));
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
        var temp_list = GetAliveMembers();
        //  내림 차순
        temp_list.Sort((a, b) => b.GetDistanceFromCenter(self).CompareTo(a.GetDistanceFromCenter(self)));
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
    /// 체력 수치가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighestLifeValue(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  내림 차순
        temp_list.Sort((a, b) => b.Life.CompareTo(a.Life));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 체력 비율이 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighestLifeRate(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  내림 차순
        temp_list.Sort((a, b) => b.GetLifePercentage().CompareTo(a.GetLifePercentage()));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 체력이 가장 적게 남아있는 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowestLifeValue(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  오름 차순
        temp_list.Sort((a, b) => a.Life.CompareTo(b.Life));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 체력 비율이 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowestLifeRate(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();

        //  오름 차순
        temp_list.Sort((a, b) => a.GetLifePercentage().CompareTo(b.GetLifePercentage()));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 공격력 수치가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowestAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  오름 차순
        temp_list.Sort((a, b) => a.Attack.CompareTo(b.Attack));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 공격력 수치가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighestAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  내림 차순
        temp_list.Sort((a, b) => b.Attack.CompareTo(a.Attack));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 방어력 수치가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowestDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  오름 차순
        temp_list.Sort((a, b) => a.Defense.CompareTo(b.Defense));
        GetTargetsFromTempList(temp_list, count, ref targets);

    }
    /// <summary>
    /// 방어력 수치가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighestDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  내림 차순
        temp_list.Sort((a, b) => b.Defense.CompareTo(a.Defense));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    #endregion




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

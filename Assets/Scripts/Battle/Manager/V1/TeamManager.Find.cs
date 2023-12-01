using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TeamManager 
{
    /// <summary>
    /// 원하는 팀의 원하는 타겟 룰을 이용하여 지정된 숫자만큼의 타겟을 찾아준다.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_type"></param>
    /// <param name="rule_type"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    public void FindTarget(HeroBase self, TARGET_TYPE target_type, TARGET_RULE_TYPE rule_type, int count, ref List<HeroBase> targets)
    {
        targets.Clear();
        if (target_type == TARGET_TYPE.MY_TEAM) //  아군에서 찾기
        {
            //  타겟 룰 타입의 우선순위에 따라 타겟을 찾아냄
            FindTargetRuleExec(self, rule_type, count, ref targets);
        }
        else // 적군에서 찾기
        {
            var enemy_team = GetEnemyTeam();
            enemy_team.FindTargetRuleExec(self, rule_type, count, ref targets);
        }

        //  요청 타겟수보다 많을 경우 타겟수 만큼만 반환해준다.
        if (targets.Count > count)
        {
            targets.RemoveRange(count, targets.Count - count);
        }

    }

    /// <summary>
    /// 원하는 팀의 원하는 타겟 룰을 이용하여 지정된 숫자만큼의 타겟을 찾아주는 함수
    /// 타겟룰의 우선순위에 따라 타겟을 찾아준다.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_type"></param>
    /// <param name="rule_types"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    public void FindTarget(HeroBase self, TARGET_TYPE target_type, TARGET_RULE_TYPE[] rule_types, int count, ref List<HeroBase> targets)
    {
        targets.Clear();
        if (target_type == TARGET_TYPE.MY_TEAM) //  아군에서 찾기
        {
            int cnt = 0;
            //  타겟 룰 타입의 우선순위에 따라 순서대로 타겟을 찾아낸다.
            for (int i = 0; i < rule_types.Length; i++)
            {
                TARGET_RULE_TYPE rt = rule_types[i];
                FindTargetRuleExec(self, rt, count, ref targets);
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
            for (int i = 0; i < rule_types.Length; i++)
            {
                TARGET_RULE_TYPE rt = rule_types[i];
                enemy_team.FindTargetRuleExec(self, rt, count, ref targets);
                cnt += targets.Count;
                if(cnt >= count)
                {
                    break;
                }
            }
        }

        //  요청 타겟수보다 많을 경우 타겟수 만큼만 반환해준다.
        if (targets.Count > count)
        {
            targets.RemoveRange(count, targets.Count - count);
        }
    }

    /// <summary>
    /// 타겟 룰에 따라 타겟을 찾아서 반환
    /// </summary>
    /// <param name="self"></param>
    /// <param name="rule_type"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    public void FindTargetRuleExec(HeroBase self, TARGET_RULE_TYPE rule_type, int count, ref List<HeroBase> targets)
    {
        //targets.Clear();
        switch(rule_type)
        {
            case TARGET_RULE_TYPE.RANDOM:
                FindTargetRuleRandom(self, count, ref targets);
                break;
        }
    }


    /// <summary>
    /// 임의의 타겟을 개수 만큼 추출
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleRandom(HeroBase self, int count, ref List<HeroBase> targets)
    {
        var temp_list = GetAliveMembers();
        GetTargetsRandomFromTempList(temp_list, count, ref targets);

    }

    /// <summary>
    /// 임의의 멤버들 중 랜덤한 멤버를 추출하기 위한 함수
    /// </summary>
    /// <param name="temp_list"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void GetTargetsRandomFromTempList(List<HeroBase> temp_list, int count, ref List<HeroBase> targets)
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
                HeroBase hero = temp_list[r];
                if (!targets.Exists(x => object.ReferenceEquals(x, hero)))
                {
                    targets.Add(hero);
                    cnt++;
                }
            }
        }
    }
    /// <summary>
    /// 임의의 멤버들 중 순서대로 멤버를 추출하기
    /// 우선순위는 이 함수를 호출하기 전에 정렬해야 함
    /// </summary>
    /// <param name="temp_list"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void GetTargetsFromTempList(List<HeroBase> temp_list, int count, ref List<HeroBase> targets)
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
        for (int i = 0; i < cnt; i++)
        {
            HeroBase hero = temp_list[i];
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

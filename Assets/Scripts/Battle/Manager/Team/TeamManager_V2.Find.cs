using System.Collections.Generic;
using System.Linq;

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
            if (targets.Count >= max_count)
            {
                targets.RemoveRange(max_count, targets.Count - max_count);
            }

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
            temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x < center.transform.localPosition.x && x.GetDistanceFromCenter(center) <= range));
        }
        else
        {
            temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x > center.transform.localPosition.x && x.GetDistanceFromCenter(center) <= range));
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
    /// <param name="targets"></param>
    public void FindTargetInRangeAtApproach(HeroBase_V2 self, TARGET_TYPE target_type, float approach_distance, ref List<HeroBase_V2> targets)
    {
        int count = 1;  //  무조건 1명만 찾는다

        targets.Clear();
        if (target_type == TARGET_TYPE.MY_TEAM)
        {
            FindTargetRuleExec(self, TARGET_RULE_TYPE.APPROACH, approach_distance, 0, count, 0, ref targets);
        }
        else
        {
            var enemy_team = GetEnemyTeam();
            enemy_team.FindTargetRuleExec(self, TARGET_RULE_TYPE.APPROACH, approach_distance, 0, count, 0, ref targets);
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
    /// <param name="self">타겟을 요청하는 요청자</param>
    /// <param name="target_type">아군/적군 타입</param>
    /// <param name="rule_type">타겟을 지정하는 타입</param>
    /// <param name="approach_distance">접근 거리</param>
    /// <param name="target_order">순번</param>
    /// <param name="count">타겟수</param>
    /// <param name="target_range">추가 타겟이 필요한 경우, 범위 정보</param>
    /// <param name="targets">검색 결과</param>
    public void FindTargetInRange(HeroBase_V2 self, TARGET_TYPE target_type, TARGET_RULE_TYPE rule_type, float approach_distance, int target_order, int count, float target_range, ref List<HeroBase_V2> targets)
    {
        targets.Clear();

        //  아군에서 찾기
        if (target_type == TARGET_TYPE.MY_TEAM)
        {
            FindTargetRuleExec(self, rule_type, approach_distance, target_order, count, target_range, ref targets);
        }
        else // 적군에서 찾기
        {
            var enemy_team = GetEnemyTeam();
            enemy_team.FindTargetRuleExec(self, rule_type, approach_distance, target_order, count, target_range, ref targets);
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
    public void FindTargetInRange(HeroBase_V2 self, TARGET_TYPE target_type, TARGET_RULE_TYPE[] rule_types, int target_order, float approach_distance, int count, float target_range, ref List<HeroBase_V2> targets)
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
                FindTargetRuleExec(self, rule, approach_distance, target_order, count, target_range, ref targets);
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
                enemy_team.FindTargetRuleExec(self, rule, approach_distance, target_order, count, target_range, ref targets);
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
    /// <param name="target_order"></param>
    /// <param name="count"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleExec(HeroBase_V2 self, TARGET_RULE_TYPE rule_type, float approach_distance, int target_order, int count, float target_range, ref List<HeroBase_V2> targets)
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
            case TARGET_RULE_TYPE.NEAREST_ADD_BACK_RANGE:           //  가장 가까운 타겟 우선 찾고, 해당 타겟을 기준으로 후방 범위내에 있는 타겟 추가
                FindTargetRuleNearestAddBackRange(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.FURTHEST_ADD_FRONT_RANGE:         //  가장 먼 타겟 우선 찾고, 해당 타겟을 기준으로 전방 범위내에 있는 타겟 추가
                FindTargetRuleFurthestAddFrontRange(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.NEAREST_ADD_ARROUND_RANGE:        //  가장 가까운 타겟 우선 찾고, 해당 타겟을 기준으로 주변 범위내에 있는 타겟 추가
                FindTargetRuleNearestAddArroundRange(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.FURTHEST_ADD_ARROUND_RANGE:       //  가장 먼 타겟 우선 찾고, 해당 타겟을 기준으로 주변 범위내에 있는 타겟 추가
                FindTargetRuleFurthestAddArroundRange(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.ARROUND_RANGE_WITH_ME:            //  나를 포함한 내 주변 지정 거리 이내 아군 선택
                FindTargetRuleArroundRangeWithMe(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.BACK_RANGE_WITH_ME:               //  나를 포함한 내 뒤 지정 거리 이내 아군 선택
                FindTargetRuleBackRangeWithMe(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.FRONT_RANGE_WITH_ME:              //  나를 포함한 내 앞 지정 거리 이내 아군 선택
                FindTargetRuleFrontRangeWithMe(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.ARROUND_RANGE_WITHOUT_ME:         //  나를 제외한 내 주변 지정 거리 이내 아군 선택
                FindTargetRuleArroundRangeWithoutMe(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.BACK_RANGE_WITHOUT_ME:            //  나를 제외한 내 뒤 지정 거리 이내 아군 선택
                FindTargetRuleBackRangeWithoutMe(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.FRONT_RANGE_WITHOUT_ME:           //  나를 제외한 내 앞 지정 거리 이내 아군 선택
                FindTargetRuleFrontRangeWithoutMe(self, target_order, target_range, ref targets);
                break;
            case TARGET_RULE_TYPE.APPROACH:
                FindTargetRuleApproach(self, approach_distance, count, ref targets);
                break;

            case TARGET_RULE_TYPE.HIGH_HP_VALUE:    //  완료
                FindTargetRuleHighHPValue(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_HP_RATE:     //  완료
                FindTargetRuleHighHPRate(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_HP_VALUE:     //  완료
                FindTargetRuleLowHPValue(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_HP_RATE:      //  완료
                FindTargetRuleLowHPRate(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_P_ATK:        //  완료
                FindTargetRuleLowPhysicsAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_P_ATK:       //  완료
                FindTargetRuleHighPhysicsAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_M_ATK:        //  완료
                FindTargetRuleLowMagicAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_M_ATK:       //  완료
                FindTargetRuleHighMagicAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_PM_ATK:       //  완료
                FindTargetRuleLowBothAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_PM_ATK:      //  완료
                FindTargetRuleHighBothAttack(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_P_DEF:        //  완료
                FindTargetRuleLowPhysicsDefense(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_P_DEF:       //  완료
                FindTargetRuleHighPhysicsDefense(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_M_DEF:        //  완료
                FindTargetRuleLowMagicDefense(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_M_DEF:       //  완료
                FindTargetRuleHighMagicDefense(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_PM_DEF:       //  완료
                FindTargetRuleLowBothDefense(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_PM_DEF:      //  완료
                FindTargetRuleHighBothDefense(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_MAX_HP:       //  완료
                FindTargetRuleLowMaxHP(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_MAX_HP:      //  완료
                FindTargetRuleHighMaxHP(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_ACCURACY:     //  완료
                FindTargetRuleLowAccuracy(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_ACCURACY:    //  완료
                FindTargetRuleHighAccuracy(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_EVASION:      //  완료
                FindTargetRuleLowEvation(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_EVASION:     //  완료
                FindTargetRuleHighEvation(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_P_CRI_INC:    //  완료
                FindTargetRuleLowPhysicsCriticalChance(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_P_CRI_INC:   //  완료
                FindTargetRuleHighPhysicsCriticalChance(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_M_CRI_INC:    //  완료
                FindTargetRuleLowMagicCriticalChance(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_M_CRI_INC:   //  완료
                FindTargetRuleHighMagicCriticalChance(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_P_CRI_ADD:    //  완료
                FindTargetRuleLowPhysicsCriticalPower(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_P_CRI_ADD:   //  완료
                FindTargetRuleHighPhysicsCriticalPower(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_M_CRI_ADD:    //  완료
                FindTargetRuleLowMagicCriticalPower(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_M_CRI_ADD:   //  완료
                FindTargetRuleHighMagicCriticalPower(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_ATK_RECOVERY: //  완료
                FindTargetRuleLowVampirePoint(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_ATK_RECOVERY:    //  완료
                FindTargetRuleHighVampirePoint(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.LOW_RESIST:       //  완료
                FindTargetRuleLowResistPoint(self, count, ref targets);
                break;
            case TARGET_RULE_TYPE.HIGH_RESIST:      //  완료
                FindTargetRuleHighResistPoint(self, count, ref targets);
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
        //  기준을 중심으로 가까운 거리 오름차순
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

        // 기준을 중심으로 거리가 먼 우선순위 (내림차순) 
        temp_list.Sort((a, b) => b.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));

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

        // 기준을 중심으로 거리가 먼 우선순위 (내림차순) 
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
    void FindTargetRuleHighHPValue(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  현재 체력 내림 차순
        temp_list.Sort((a, b) => b.Life.CompareTo(a.Life));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 체력 비율이 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighHPRate(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  현재 체력 비율 내림 차순
        temp_list.Sort((a, b) => b.GetLifePercentage().CompareTo(a.GetLifePercentage()));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 체력이 가장 적게 남아있는 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowHPValue(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  현재 체력 오름 차순
        temp_list.Sort((a, b) => a.Life.CompareTo(b.Life));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 체력 비율이 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowHPRate(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();

        //  현재 체력 비율 오름 차순
        temp_list.Sort((a, b) => a.GetLifePercentage().CompareTo(b.GetLifePercentage()));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 물리 공격력 수치가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowPhysicsAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 공격력 오름 차순
        temp_list.Sort((a, b) => a.Physics_Attack.CompareTo(b.Physics_Attack));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 물리 공격력 수치가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighPhysicsAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 공격력 내림 차순
        temp_list.Sort((a, b) => b.Physics_Attack.CompareTo(a.Physics_Attack));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 공격력 수치가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowMagicAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 공격력 오름 차순
        temp_list.Sort((a, b) => a.Magic_Attack.CompareTo(b.Magic_Attack));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 공격력 수치가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighMagicAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 공격력 내림 차순
        temp_list.Sort((a, b) => b.Magic_Attack.CompareTo(a.Magic_Attack));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 물리/마법 공격력 수치 중 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowBothAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리/마법 중 낮은 공격력 오름 차순
        temp_list.Sort((a, b) => a.GetLowAttackPoint().CompareTo(b.GetLowAttackPoint()));
        GetTargetsFromTempList(temp_list, count, ref targets);

    }
    /// <summary>
    /// 물리/마법 공격력 수치 중 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighBothAttack(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리/마법 중 높은 공격력 내림 차순
        temp_list.Sort((a, b) => b.GetHighAttackPoint().CompareTo(a.GetHighAttackPoint()));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 물리 방어력 수치가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowPhysicsDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 방어력 오름 차순
        temp_list.Sort((a, b) => a.Physics_Defense.CompareTo(b.Physics_Defense));
        GetTargetsFromTempList(temp_list, count, ref targets);

    }
    /// <summary>
    /// 물리 방어력 수치가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighPhysicsDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 방어력 내림 차순
        temp_list.Sort((a, b) => b.Physics_Defense.CompareTo(a.Physics_Defense));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 방어력 수치가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowMagicDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 방어력 오름 차순
        temp_list.Sort((a, b) => a.Magic_Defense.CompareTo(b.Magic_Defense));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 방어력 수치가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighMagicDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 방어력 내림 차순
        temp_list.Sort((a, b) => b.Magic_Defense.CompareTo(a.Magic_Defense));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }

    /// <summary>
    /// 물리/마법 방어력 수치 중 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowBothDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리/마법 방어력 수치 중 낮은 수치의 오름 차순
        temp_list.Sort((a, b) => a.GetLowDefensePoint().CompareTo(b.GetLowDefensePoint()));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 물리/마법 방어력 수치 중 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighBothDefense(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리/마법 방어력 수치 중 높은 수치의 내림 차순
        temp_list.Sort((a, b) => b.GetLowDefensePoint().CompareTo(a.GetLowDefensePoint()));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 최대 체력이 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowMaxHP(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  최대 체력 오름 차순
        temp_list.Sort((a, b) => a.Max_Life.CompareTo((b.Max_Life)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 최대 체력이 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighMaxHP(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  최대 체력 내림 차순
        temp_list.Sort((a, b) => b.Max_Life.CompareTo(a.Max_Life));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
   
    /// <summary>
    /// 명중이 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowAccuracy(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  명중 오름 차순
        temp_list.Sort((a, b) => a.Accuracy.CompareTo((b.Accuracy)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 명중이 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighAccuracy(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  명중 내림 차순
        temp_list.Sort((a, b) => b.Accuracy.CompareTo(a.Accuracy));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 회피가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowEvation(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  회피 오름 차순
        temp_list.Sort((a, b) => a.Evasion.CompareTo((b.Evasion)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 회피가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighEvation(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  회피 내림 차순
        temp_list.Sort((a, b) => b.Evasion.CompareTo(a.Evasion));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 물리 치명타 확률 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowPhysicsCriticalChance(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 치명타 확률 오름 차순
        temp_list.Sort((a, b) => a.Physics_Critical_Chance.CompareTo((b.Physics_Critical_Chance)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 물리 치명타 확률 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighPhysicsCriticalChance(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 치명타 확률 내림 차순
        temp_list.Sort((a, b) => b.Physics_Critical_Chance.CompareTo(a.Physics_Critical_Chance));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 치명타 확률 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowMagicCriticalChance(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 치명타 확률 오름 차순
        temp_list.Sort((a, b) => a.Magic_Critical_Chance.CompareTo((b.Magic_Critical_Chance)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 치명타 확률 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighMagicCriticalChance(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 치명타 확률 내림 차순
        temp_list.Sort((a, b) => b.Magic_Critical_Chance.CompareTo(a.Magic_Critical_Chance));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 물리 치명타 파워 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowPhysicsCriticalPower(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 치명타 추가 데미지 오름 차순
        temp_list.Sort((a, b) => a.Physics_Critical_Power_Add.CompareTo((b.Physics_Critical_Power_Add)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 물리 치명타 파워 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighPhysicsCriticalPower(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  물리 치명타 추가 데미지 내림 차순
        temp_list.Sort((a, b) => b.Physics_Critical_Power_Add.CompareTo(a.Physics_Critical_Power_Add));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 치명타 파워 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowMagicCriticalPower(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 치명타 추가 데미지 오름 차순
        temp_list.Sort((a, b) => a.Magic_Critical_Power_Add.CompareTo((b.Magic_Critical_Power_Add)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 마법 치명타 파워 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighMagicCriticalPower(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  마법 치명타 추가 데미지 내림 차순
        temp_list.Sort((a, b) => b.Magic_Critical_Power_Add.CompareTo(a.Magic_Critical_Power_Add));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 흡혈(타격 시 체력 회복량) 포인트가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowVampirePoint(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  흠혈 포인트 오름 차순
        temp_list.Sort((a, b) => a.Attack_Life_Recovery.CompareTo((b.Attack_Life_Recovery)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 흡혈(타격 시 체력 회복량) 포인트가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighVampirePoint(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  흡혈 포인트 내림 차순
        temp_list.Sort((a, b) => b.Attack_Life_Recovery.CompareTo(a.Attack_Life_Recovery));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 강인함(상태이상 확률 저항 및 상태이상 지속 시간 저항) 포인트가 가장 낮은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleLowResistPoint(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  강인함(상태 저항) 포인트 오름 차순
        temp_list.Sort((a, b) => a.Resist_Point.CompareTo((b.Resist_Point)));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }
    /// <summary>
    /// 강인함(상태이상 확률 저항 및 상태이상 지속 시간 저항) 포인트가 가장 높은 타겟 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="count"></param>
    /// <param name="targets"></param>
    void FindTargetRuleHighResistPoint(HeroBase_V2 self, int count, ref List<HeroBase_V2> targets)
    {
        var temp_list = GetAliveMembers();
        //  강인함(상태 저항) 포인트 내림 차순
        temp_list.Sort((a, b) => b.Resist_Point.CompareTo(a.Resist_Point));
        GetTargetsFromTempList(temp_list, count, ref targets);
    }


    /// <summary>
    /// 가장 가까운 적 우선 선택(순번 컬럼 연동하여 순서대로) 후 일정 영역내의 뒤에있는 타겟의 추가 선택
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleNearestAddBackRange(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        List<HeroBase_V2> temp_list = new List<HeroBase_V2>();
        var members = GetAliveMembers();
        if (members.Count > 0)
        {
            //  오름 차순
            members.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));

            HeroBase_V2 target = null;
            if (target_order < members.Count)
            {
                target = members[target_order];
            }
            else
            {
                target = members.First();

            }
            if (target == null)
            {
                return;
            }
            members.Remove(target);
            targets.Add(target);

            if (Team_Type == TEAM_TYPE.LEFT)
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x <= target.transform.localPosition.x && x.GetDistanceFromCenter(target) <= target_range));
            }
            else
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x >= target.transform.localPosition.x && x.GetDistanceFromCenter(target) <= target_range));
            }
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(target).CompareTo(b.GetDistanceFromCenter(target)));

            GetTargetsFromTempList(temp_list, 5, ref targets);
        }

    }
    /// <summary>
    /// 가장 거리가 먼 적 우선 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로) 후 일정 영역내의 앞에 있는 타겟 추가 선택
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleFurthestAddFrontRange(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        List<HeroBase_V2> temp_list = new List<HeroBase_V2>();
        var members = GetAliveMembers();
        if (members.Count > 0)
        {
            //  내림 차순
            members.Sort((a, b) => b.GetDistanceFromCenter(self).CompareTo(a.GetDistanceFromCenter(self)));

            HeroBase_V2 target = null;
            if (target_order < members.Count)
            {
                target = members[target_order];
            }
            else
            {
                target = members.First();

            }
            if (target == null)
            {
                return;
            }
            members.Remove(target);
            targets.Add(target);

            if (Team_Type == TEAM_TYPE.LEFT)
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x >= target.transform.localPosition.x && x.GetDistanceFromCenter(target) <= target_range));
            }
            else
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x <= target.transform.localPosition.x && x.GetDistanceFromCenter(target) <= target_range));
            }
            temp_list.Sort((a, b) => b.GetDistanceFromCenter(target).CompareTo(a.GetDistanceFromCenter(target)));

            GetTargetsFromTempList(temp_list, 5, ref targets);
        }

    }
    /// <summary>
    /// 나를 포함한 내 주변 지정 거리 이내의 아군 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_order"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleArroundRangeWithMe(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets) 
    {
        targets.Add(self);
        var members = GetAliveMembers();
        members.Remove(self);

        if (members.Count > 0)
        {
            //  오름 차순
            var temp_list = members.FindAll(x => x.GetDistanceFromCenter(self) <= target_range);
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));

            GetTargetsFromTempList(temp_list, 5, ref targets);
        }
    }
    /// <summary>
    /// 나를 포함한 내 뒤 지정 거리 이내의 아군 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_order"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleBackRangeWithMe(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        List<HeroBase_V2> temp_list = new List<HeroBase_V2>();
        var members = GetAliveMembers();
        targets.Add(self);
        members.Remove(self);
        if (members.Count > 0)
        {
            if (Team_Type == TEAM_TYPE.LEFT)
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x <= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            else
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x >= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));
            GetTargetsFromTempList(temp_list, 5, ref targets);
        }
    }
    /// <summary>
    /// 나를 포함한 내 앞 지정 거리 이내의 아군 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_order"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleFrontRangeWithMe(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        List<HeroBase_V2> temp_list = new List<HeroBase_V2>();
        var members = GetAliveMembers();
        targets.Add(self);
        members.Remove(self);
        if (members.Count > 0)
        {
            if (Team_Type == TEAM_TYPE.LEFT)
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x >= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            else
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x <= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));
            GetTargetsFromTempList(temp_list, 5, ref targets);
        }
    }
    /// <summary>
    /// 나를 제외한 내 주변 지정 거리 이내의 아군 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_order"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleArroundRangeWithoutMe(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        var members = GetAliveMembers();

        if (members.Count > 0)
        {
            //  오름 차순
            var temp_list = members.FindAll(x => x.GetDistanceFromCenter(self) <= target_range);
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));

            GetTargetsFromTempList(temp_list, 5, ref targets);
        }
        targets.Remove(self);
    }
    /// <summary>
    /// 나를 제외한 내 뒤 지정 거리 이내의 아군 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_order"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleBackRangeWithoutMe(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        List<HeroBase_V2> temp_list = new List<HeroBase_V2>();
        var members = GetAliveMembers();
        if (members.Count > 0)
        {
            if (Team_Type == TEAM_TYPE.LEFT)
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x <= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            else
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x >= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));
            GetTargetsFromTempList(temp_list, 5, ref targets);
        }
        targets.Remove(self);
    }
    /// <summary>
    /// 나를 제외한 내 앞 지정 거리 이내의 아군 찾기
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_order"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleFrontRangeWithoutMe(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        List<HeroBase_V2> temp_list = new List<HeroBase_V2>();
        var members = GetAliveMembers();
        if (members.Count > 0)
        {
            if (Team_Type == TEAM_TYPE.LEFT)
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x >= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            else
            {
                temp_list.AddRange(members.FindAll(x => x.transform.localPosition.x <= self.transform.localPosition.x && x.GetDistanceFromCenter(self) <= target_range));
            }
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));
            GetTargetsFromTempList(temp_list, 5, ref targets);
        }
        targets.Remove(self);
    }

    /// <summary>
    /// 가장 가까운 적 우선 선택(순번 컬럼 연동하여 순서대로) 후 일정 영역내의(주변) 타겟의 추가 선택
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleNearestAddArroundRange(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        var members = GetAliveMembers();

        if (members.Count > 0)
        {
            //  오름 차순
            members.Sort((a, b) => a.GetDistanceFromCenter(self).CompareTo(b.GetDistanceFromCenter(self)));

            HeroBase_V2 target = null;
            if (target_order < members.Count)
            {
                target = members[target_order];
            }
            else
            {
                target = members.First();

            }
            if (target == null)
            {
                return;
            }
            members.Remove(target);
            targets.Add(target);

            var temp_list = members.FindAll(x => x.GetDistanceFromCenter(target) <= target_range);
            temp_list.Sort((a, b) => a.GetDistanceFromCenter(target).CompareTo(b.GetDistanceFromCenter(target)));

            GetTargetsFromTempList(temp_list, 5, ref targets);

        }
    }
    /// <summary>
    /// 가장 거리가 먼 적 우선 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로) 후 일정 영역내의 (주변) 타겟 추가 선택
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target_range"></param>
    /// <param name="targets"></param>
    void FindTargetRuleFurthestAddArroundRange(HeroBase_V2 self, int target_order, float target_range, ref List<HeroBase_V2> targets)
    {
        var members = GetAliveMembers();

        if (members.Count > 0)
        {
            //  내림 차순
            members.Sort((a, b) => b.GetDistanceFromCenter(self).CompareTo(a.GetDistanceFromCenter(self)));

            HeroBase_V2 target = null;
            if (target_order < members.Count)
            {
                target = members[target_order];
            }
            else
            {
                target = members.First();

            }
            if (target == null)
            {
                return;
            }
            members.Remove(target);
            targets.Add(target);

            var temp_list = members.FindAll(x => x.GetDistanceFromCenter(target) <= target_range);
            temp_list.Sort((a, b) => b.GetDistanceFromCenter(target).CompareTo(a.GetDistanceFromCenter(target)));

            GetTargetsFromTempList(temp_list, 5, ref targets);

        }
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

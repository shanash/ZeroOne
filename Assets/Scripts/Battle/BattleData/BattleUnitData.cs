// TODO: 팩토리로 생성 가능할듯
/**/
using System.Collections.Generic;
using System.Linq;

public abstract class BattleUnitData : BattleDataBase
{
    protected HeroBase_V2 Hero;
    public BattleSkillManager Skill_Mng { get; protected set; }

    protected abstract int Level { get; set; }

    public CHARACTER_TYPE Character_Type { get; protected set; } = CHARACTER_TYPE.NONE;

    protected bool Is_Boss;

    /// <summary>
    /// 팀 속성 시너지 데이터
    /// </summary>
    protected List<Attribute_Synergy_Data> Team_Synergy_Data_List = new List<Attribute_Synergy_Data>();

    /// <summary>
    /// 보스전 등 기존 사거리 보다 조금 더 긴 사거리를 필요로 하는 경우에 추가될 거리를 지정할 수 있다.
    /// </summary>
    protected float Add_Approach_Distance;


    protected BattleUnitData(CHARACTER_TYPE ctype) : base()
    {
        Character_Type = ctype;
        Add_Approach_Distance = 0f;
        Is_Boss = false;
    }

    public abstract BattleUnitData SetUnitID(params int[] unit_ids);

    public void SetHeroBase(HeroBase_V2 h)
    {
        Hero = h;
        if (Skill_Mng != null)
        {
            Skill_Mng.SetHeroBase(h);
        }
    }

    /// <summary>
    /// 유닛 ID
    /// </summary>
    /// <returns></returns>
    public abstract int GetUnitID();

    /// <summary>
    /// 사용자 유닛의 경우 고유 Number가 있다.
    /// </summary>
    /// <returns></returns>
    public virtual int GetUnitNum() { return 0; }

    /// <summary>
    /// 팀 속성 시너지 추가<br/>
    /// 아군만 시너지 효과를 얻을 수 있다.
    /// </summary>
    /// <param name="synergy_list"></param>
    public void AddTeamAttributeSynergy(List<Attribute_Synergy_Data> synergy_list)
    {
        int cnt = synergy_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var s = synergy_list[i];
            if (!Team_Synergy_Data_List.Contains(s))
            {
                Team_Synergy_Data_List.Add(s);
            }
        }
    }

    /// <summary>
    /// 팀 속성 시너지 포인트 반환
    /// </summary>
    /// <param name="mtype"></param>
    /// <returns></returns>
    public double GetTeamAttributeAddPoint(STAT_MULTIPLE_TYPE mtype)
    {
        double point = 0;
        int cnt = Team_Synergy_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var s = Team_Synergy_Data_List[i];
            if (s.multiple_type == mtype)
            {
                point += s.add_damage_per;
            }
        }
        return point;
    }
    /// <summary>
    /// 표시 아이콘
    /// </summary>
    /// <returns></returns>
    public virtual string GetIconPath() { return string.Empty; }

    /// <summary>
    /// 유닛의 레벨 설정
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public abstract void SetLevel(int lv);
    
    /// <summary>
    /// 유닛의 레벨 반환
    /// </summary>
    /// <returns></returns>
    public abstract int GetLevel();
    /// <summary>
    /// 유닛의 성급 반환
    /// </summary>
    /// <returns></returns>
    public abstract int GetStarGrade();

    /// <summary>
    /// 유닛의 레벨 스텟 데이터 아이디
    /// </summary>
    /// <param name="stat_id"></param>
    public abstract void SetStatDataID(int stat_id);
    /// <summary>
    /// 유닛의 기본 데이터 반환
    /// </summary>
    /// <returns></returns>
    public abstract object GetUnitData();
    
    /// <summary>
    /// 유닛의 전투 데이터 반환
    /// </summary>
    /// <returns></returns>
    public abstract object GetBattleData();

    public void SetAddApproachDistance(float add_dist) { Add_Approach_Distance = add_dist; }
 
    /// <summary>
    /// 유닛의 사용자 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetUserUnitData() {  return null; }

    /// <summary>
    /// 유닛의 속성 타입 반환
    /// </summary>
    /// <returns></returns>
    public virtual ATTRIBUTE_TYPE GetAttributeType() {  return ATTRIBUTE_TYPE.NONE; }

    public void SetEnemyBoss(bool boss) { Is_Boss = boss; }
    public bool IsEnemyBoss() { return Is_Boss; }

    /// <summary>
    /// 전투력 포인트
    /// </summary>
    /// <returns></returns>
    public virtual double GetCombatPoint(List<Attribute_Synergy_Data> synergy_list = null)
    {
        double atk_synergy = 0;
        if (synergy_list != null)
        {
            atk_synergy = synergy_list.Sum(x => x.multiple_type == STAT_MULTIPLE_TYPE.ATTACK_RATE ? x.add_damage_per : 0);
        }
        double total_attack_point = GetTotalAttackPoint();
        if (atk_synergy != 0)
        {
            total_attack_point += total_attack_point * atk_synergy;
        }

        double total_defense_point = GetTotalDefensePoint();

        return GameCalc.GetCombatPoint(
            GetMaxLifePoint(),
            total_attack_point,
            total_defense_point,
            GetAutoRecoveryLife(),
            GetEvasionPoint(),
            GetAttackLifeRecovery(),
            GetAccuracyPoint(),
            GetSumSkillsLevel());
    }

    public virtual double GetTotalAttackPoint() { return 0; }
    public virtual double GetTotalDefensePoint() { return 0; }

    /// <summary>
    /// 물리 공격 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetPhysicsAttackPoint();

    /// <summary>
    /// 마법 공격 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetMagicAttackPoint();

    /// <summary>
    /// 물리 방어력 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetPhysicsDefensePoint();
    /// <summary>
    /// 마법 방어력 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetMagicDefensePoint();

    /// <summary>
    /// 최대 체력 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetMaxLifePoint();

    /// <summary>
    /// 흡혈량<br/>
    /// 회복량 = 내가 준 최종 데미지(적에게 최종적으로 가해진 데미지) * HP 흡수 포인트 / (HP 흡수 포인트 + 상대 레벨 + 100)<br/>
    /// </summary>
    /// <returns></returns>
    public abstract double GetAttackLifeRecovery();

    /// <summary>
    /// 명중 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetAccuracyPoint();
    /// <summary>
    /// 회피 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetEvasionPoint();
    /// <summary>
    /// 웨이브 이동시 자동 체력 회복
    /// </summary>
    /// <returns></returns>
    public virtual double GetAutoRecoveryLife() { return 0; }
    /// <summary>
    /// 물리 치명타 확률<br/>
    /// 치명타 확률 = 치명타 확률 값 * 0.05 * 0.01 * 캐스터 레벨 / 적 레벨
    /// </summary>
    /// <returns></returns>
    public abstract double GetPhysicsCriticalChance();
    /// <summary>
    /// 물리 치명타 데미지(파워)<br/>
    /// 치명타 데미지 = 최종 데미지 * 2(or 물리 크리티컬 데미지에 영향을 주는 스탯 값)
    /// </summary>
    /// <returns></returns>
    public abstract double GetPhysicsCriticalPowerAdd();
    /// <summary>
    /// 마법 치명타 확률<br/>
    /// 치명타 확률 = 치명타 확률 값 * 0.05 * 0.01 * 캐스터 레벨 / 적 레벨
    /// </summary>
    /// <returns></returns>
    public abstract double GetMagicCriticalChance();
    /// <summary>
    /// 마법 치명타 데미지(파워)<br/>
    /// 치명타 데미지 = 최종 데미지 * 2(or 마법 크리티컬 데미지에 영향을 주는 스탯 값)
    /// </summary>
    /// <returns></returns>
    public abstract double GetMagicCriticalPowerAdd();

    /// <summary>
    /// 강인함(상태이상 저항력)<br/>
    /// 상태이상 확률 및 상태이상 지속 시간에 영향을 주는 스탯<br/>
    /// 상태이상 확률 = 상태이상 확률 - (상태이상 확률 * 강인함 / 1000)
    /// </summary>
    /// <returns></returns>
    public abstract double GetResistPoint();

    /// <summary>
    /// 체력 회복량 증가(힐량 증가)
    /// </summary>
    /// <returns></returns>
    public abstract double GetLifeRecoveryInc();

    /// <summary>
    /// 무게<br/>
    /// 넉백 및 풀링의 변수로 사용
    /// </summary>
    /// <returns></returns>
    public abstract double GetWeight();

    /// <summary>
    /// 이동 속도
    /// </summary>
    /// <returns></returns>
    public abstract float GetMoveSpeed();

    /// <summary>
    /// 접근 거리
    /// </summary>
    /// <returns></returns>
    public abstract float GetApproachDistance();
    /// <summary>
    /// 스킬 패턴 id 리스트
    /// </summary>
    /// <returns></returns>
    public abstract int[] GetSkillPattern();

    /// <summary>
    /// 궁극기 스킬 id
    /// </summary>
    /// <returns></returns>
    public abstract int GetSpecialSkillID();

    /// <summary>
    /// 데이터가 없어서 임시로 만들어놓은 UI 표시용 레벨합계 값입니다
    /// </summary>
    /// <returns></returns>
    public virtual int GetSumSkillsLevel() { return Skill_Mng.GetSkillLevelSum(); }

    

    public virtual int GetSpecialSkillLevel()
    {
        var skill_group = Skill_Mng.GetSpecialSkillGroup();
        if (skill_group != null)
        {
            return skill_group.GetSkillLevel();
        }
        return 0;
    }

    /// <summary>
    /// 포지션 타입
    /// </summary>
    /// <returns></returns>
    public abstract POSITION_TYPE GetPositionType();
    /// <summary>
    /// 유닛의 프리팹 패스
    /// </summary>
    /// <returns></returns>
    public abstract string GetPrefabPath();
    /// <summary>
    /// 유닛의 썸네일 패스 (현재는 PC에는 썸네일이 있으나, NPC는 썸네일이 없음)
    /// </summary>
    /// <returns></returns>
    public virtual string GetThumbnailPath() { return null; }
    /// <summary>
    /// 유닛의 이름 
    /// </summary>
    /// <returns></returns>
    public abstract string GetUnitName();
    /// <summary>
    /// 유닛의 종족 타입
    /// </summary>
    /// <returns></returns>
    public abstract TRIBE_TYPE GetTribeType();
    /// <summary>
    /// NPC 타입(NPC일 경우에만 사용됨)
    /// </summary>
    /// <returns></returns>
    public virtual NPC_TYPE GetNpctype() { return NPC_TYPE.NONE; }

    /// <summary>
    /// 전투 캐릭 반환
    /// </summary>
    /// <returns></returns>
    public HeroBase_V2 GetHeroBase() { return Hero; }

    /// <summary>
    /// 유닛 스케일.<br/>
    /// 유닛 스케일에 맞춰서, 이펙트의 스케일도 조절해야 함
    /// </summary>
    /// <returns></returns>
    public virtual float GetUnitScale() { return 0.35f; }

    protected override void Destroy()
    {
        Hero = null;
        Skill_Mng?.Dispose();
        Skill_Mng = null;
    }
}

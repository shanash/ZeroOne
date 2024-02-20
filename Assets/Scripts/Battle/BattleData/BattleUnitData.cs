// TODO: 팩토리로 생성 가능할듯
/**/
using System.Collections.Generic;

public abstract class BattleUnitData : BattleDataBase
{
    protected HeroBase_V2 Hero;
    public BattleSkillManager Skill_Mng { get; protected set; }

    protected abstract int Level { get; set; }

    public CHARACTER_TYPE Character_Type { get; protected set; } = CHARACTER_TYPE.NONE;

    /// <summary>
    /// 팀 속성 시너지 데이터
    /// </summary>
    protected List<Attribute_Synergy_Data> Team_Synergy_Data_List = new List<Attribute_Synergy_Data>();

    //protected float Battle_Speed_Multiple = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];

    protected BattleUnitData(CHARACTER_TYPE ctype) : base()
    {
        Character_Type = ctype;
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

    /// <summary>
    /// 전투력 포인트
    /// </summary>
    /// <returns></returns>
    public virtual double GetCombatPoint()
    {
        return GameCalc.GetCombatPoint(
            GetMaxLifePoint(),
            GetPhysicsAttackPoint(),
            GetPhysicsDefensePoint(),
            GetAutoRecoveryLife(),
            GetEvasionPoint(),
            GetAttackLifeRecovery(),
            GetAccuracyPoint(),
            GetSumSkillsLevel());
    }

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
    public virtual int GetSumSkillsLevel() { return 3; }

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

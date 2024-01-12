// TODO: 팩토리로 생성 가능할듯
public abstract class BattleUnitData : BattleDataBase
{
    public CHARACTER_TYPE Character_Type { get; protected set; } = CHARACTER_TYPE.NONE;

    protected BattleUnitData(CHARACTER_TYPE ctype) : base()
    {
        Character_Type = ctype;
    }

    public abstract BattleUnitData SetUnitID(params int[] unit_ids);

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
    /// 유닛의 레벨 반환
    /// </summary>
    /// <returns></returns>
    public abstract int GetLevel();

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
    /// 전투력 포인트
    /// </summary>
    /// <returns></returns>
    public virtual double GetCombatPoint()
    {
        var hp = GetLifePoint();
        var ap = GetAttackPoint();
        var dp = GetDefensePoint();
        var al = GetAutoRecoveryLife();
        var ep = GetEvationPoint();
        var ar = GetAttackRecovery();
        var acp = GetAccuracyPoint();
        var sum_skills_level = GetSumSkillsLevel();

        var cp =
            hp * 0.1f
            + (ap + dp) * 4.5f
            + al * 0.1f
            + ep * 6f
            + ar * 4.5f
            + acp * 2f
            + sum_skills_level * 10f;

        return cp;
    }

    /// <summary>
    /// 공격 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetAttackPoint();

    /// <summary>
    /// 방어력 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetDefensePoint();

    /// <summary>
    /// 체력 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetLifePoint();

    /// <summary>
    /// 흡혈량
    /// </summary>
    /// <returns></returns>
    public virtual double GetAttackRecovery() { return 0; }

    /// <summary>
    /// 명중 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetAccuracyPoint();
    /// <summary>
    /// 회피 포인트
    /// </summary>
    /// <returns></returns>
    public abstract double GetEvationPoint();
    /// <summary>
    /// 웨이브 이동시 자동 체력 회복
    /// </summary>
    /// <returns></returns>
    public virtual double GetAutoRecoveryLife() { return 0; }
    /// <summary>
    /// 치명타 확률<br/>
    /// 치명타 확률 = 치명타 확률 값 * 0.05 * 0.01 * 캐스터 레벨 / 적 레벨
    /// </summary>
    /// <returns></returns>
    public virtual double GetCriticalChance() {  return 0; }
    /// <summary>
    /// 치명타 데미지(파워)<br/>
    /// 치명타 데미지 = 최종 데미지 * 2(or 크리티컬 데미지에 영향을 주는 스탯 값)
    /// </summary>
    /// <returns></returns>
    public virtual double GetCriticalPower() { return 0; }

    /// <summary>
    /// 이동 속도
    /// </summary>
    /// <returns></returns>
    public abstract float GetMoveSpeed();
    /// <summary>
    /// HP 흡수 포인트<br/>
    /// 회복량 = 내가 준 최종 데미지(적에게 최종적으로 가해진 데미지) * HP 흡수 포인트 / (HP 흡수 포인트 + 상대 레벨 + 100)<br/>
    /// 무조건 회복이지?
    /// </summary>
    /// <returns></returns>
    public virtual double GetVampirePoint() {  return 0; }

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
}

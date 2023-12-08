
public class BattleUnitData : BattleDataBase
{
    public virtual BattleUnitData SetUnitID(params int[] unit_ids)
    {
        return null;
    }
    /// <summary>
    /// 유닛 ID
    /// </summary>
    /// <returns></returns>
    public virtual int GetUnitID() { return 0; }
    /// <summary>
    /// 사용자 유닛의 경우 고유 Number가 있다.
    /// </summary>
    /// <returns></returns>
    public virtual int GetUnitNum() { return 0; }

    /// <summary>
    /// 유닛의 기본 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetUnitData() {  return null; }
    /// <summary>
    /// 유닛의 전투 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetBattleData() { return null; }
    /// <summary>
    /// 유닛의 사용자 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetUserUnitData() {  return null; }
    /// <summary>
    /// 공격 포인트
    /// </summary>
    /// <returns></returns>
    public virtual double GetAttackPoint() {  return 0; }
    /// <summary>
    /// 방어력 포인트
    /// </summary>
    /// <returns></returns>
    public virtual double GetDefensePoint() {  return 0; }
    /// <summary>
    /// 체력 포인트
    /// </summary>
    /// <returns></returns>
    public virtual double GetLifePoint() {  return 0; }
    /// <summary>
    /// 이동 속도
    /// </summary>
    /// <returns></returns>
    public virtual float GetMoveSpeed() { return 0f; }
    
    /// <summary>
    /// 접근 거리
    /// </summary>
    /// <returns></returns>
    public virtual float GetApproachDistance() { return 0f; }
    /// <summary>
    /// 스킬 패턴 id 리스트
    /// </summary>
    /// <returns></returns>
    public virtual int[] GetSkillPattern() { return null; }
    /// <summary>
    /// 포지션 타입
    /// </summary>
    /// <returns></returns>
    public virtual POSITION_TYPE GetPositionType() { return POSITION_TYPE.NONE; }
    /// <summary>
    /// 유닛의 프리팹 패스
    /// </summary>
    /// <returns></returns>
    public virtual string GetPrefabPath() { return null; }
    /// <summary>
    /// 유닛의 썸네일 패스 (현재는 PC에는 썸네일이 있으나, NPC는 썸네일이 없음)
    /// </summary>
    /// <returns></returns>
    public virtual string GetThumbnailPath() { return null; }
    /// <summary>
    /// 유닛의 이름 
    /// </summary>
    /// <returns></returns>
    public virtual string GetUnitName() {  return null; }
    /// <summary>
    /// 유닛의 종족 타입
    /// </summary>
    /// <returns></returns>
    public virtual TRIBE_TYPE GetTribeType() { return TRIBE_TYPE.NONE; }
    /// <summary>
    /// NPC 타입(NPC일 경우에만 사용됨)
    /// </summary>
    /// <returns></returns>
    public virtual NPC_TYPE GetNpctype() { return NPC_TYPE.NONE; }
    
}

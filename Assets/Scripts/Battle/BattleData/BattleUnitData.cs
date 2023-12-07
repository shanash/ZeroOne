using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public virtual object GetUnitData() {  return null; }

    public virtual object GetBattleData() { return null; }

    public virtual object GetUserUnitData() {  return null; }

    public virtual double GetAttackPoint() {  return 0; }
    public virtual double GetDefensePoint() {  return 0; }
    public virtual double GetLifePoint() {  return 0; }

    public virtual float GetMoveSpeed() { return 0f; }

    public virtual float GetDistance() {  return 0f; }
    public virtual float GetApproachDistance() { return 0f; }

    public virtual int[] GetSkillPattern() { return null; }

    public virtual POSITION_TYPE GetPositionType() { return POSITION_TYPE.NONE; }

    public virtual string GetPrefabPath() { return null; }
    
}

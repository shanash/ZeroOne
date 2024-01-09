

using System;

public class BattleOnetimeSkillData : BattleDataBase, ICloneable, FluffyDuck.Util.Factory.IProduct
{
    public virtual void SetOnetimeSkillDataID(int skill_onetime_id) { }

    public virtual object GetOnetimeSkillData() { return null; }

    public virtual string GetEffectPrefab() { return null; }

    public virtual PROJECTILE_TYPE GetProjectileType() { return PROJECTILE_TYPE.NONE; }

    public virtual double GetEffectDuration() { return 0; }

    /// <summary>
    /// 날아가는 발사체인지, 타겟에서 즉시 발생하는 이펙트인지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsThrowingNode()
    {
        bool is_throwing = false;

        switch (GetProjectileType())
        {
            case PROJECTILE_TYPE.THROW_FOOT:
            case PROJECTILE_TYPE.THROW_BODY:
            case PROJECTILE_TYPE.THROW_HEAD:
                is_throwing = true;
                break;
            case PROJECTILE_TYPE.INSTANT_TARGET_FOOT:
            case PROJECTILE_TYPE.INSTANT_TARGET_BODY:
            case PROJECTILE_TYPE.INSTANT_TARGET_HEAD:
                is_throwing = false;
                break;
            default:
                UnityEngine.Debug.Assert(false);
                break;
        }

        return is_throwing;
    }

    public virtual void ExecSkill(BATTLE_SEND_DATA data)
    {
    }

    public virtual object Clone()
    {
        return null;
    }
}

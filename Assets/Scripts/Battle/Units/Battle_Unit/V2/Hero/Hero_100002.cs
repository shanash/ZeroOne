using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 라일라
/// </summary>
public class Hero_100002 : HeroBase_V2
{

    #region States

    public override void UnitStateSkillReady01Begin()
    {
        SetPlayableDirector();
    }
    public override void UnitStateSkillReady01()
    {
        ChangeState(UNIT_STATES.SKILL_1);
    }

    #endregion


    #region Etc Funcs

    protected override void SetPlayableDirector()
    {
        base.SetPlayableDirector();
        StartPlayableDirector();
    }

    protected override void UnsetPlayableDirector()
    {
        base.UnsetPlayableDirector();
    }

    //public override void UltimateSkillExec()
    //{
    //    var battle_state = Battle_Mng.GetCurrentState();
    //    if (battle_state != GAME_STATES.PLAYING)
    //    {
    //        return;
    //    }
    //    //  궁극기를 사용할 수 없는 상태
    //    var state = GetCurrentState();
    //    if (state == UNIT_STATES.SKILL_1 || state == UNIT_STATES.SKILL_READY_1 || state == UNIT_STATES.SKILL_END || state == UNIT_STATES.ULTIMATE_PAUSE)
    //    {
    //        return;
    //    }

    //    //  여러가지 상황상 궁극기를 사용할 수 없는 상황을 체크
    //    //  체크 완료 후 궁극기를 사용할 수 있는 경우에만 궁극기 사용
    //    var skill_grp = GetSkillManager().GetSpecialSkillGroup();
    //    if (skill_grp == null)
    //    {
    //        return;
    //    }
    //    //  궁극기가 쿨타임이 남아있을 경우 안됨
    //    if (!skill_grp.IsPrepareCooltime())
    //    {
    //        return;
    //    }

    //    for (int i = 0; i < skill_grp.GetBattleSkillDataList().Count; i++)
    //    {
    //        var skill = skill_grp.GetBattleSkillDataList()[i];
    //        FindTargetsSkillAddTargets(skill);
    //    }

    //    var target_skill = skill_grp.GetSpecialSkillTargetSkill();
    //    //  타겟이 잡혀있지 않다면, 일단 스킬 사용 안되도록
    //    if (target_skill.IsEmptyFindTarget())
    //    {
    //        return;
    //    }

    //    //  주인공 이외의 모든 캐릭 pause
    //    Battle_Mng.AllPauseUnitWithoutHero(this);

    //    //  진행중이던 이펙트 모두 숨기기
    //    Battle_Mng.GetEffectFactory().OnPauseAndHide();

    //    //  캐릭터 숨기기는 이벤트를 받아서 하자.

    //    Skeleton.AnimationState.ClearTracks();
    //    if (state == UNIT_STATES.ATTACK_READY_1 || state == UNIT_STATES.ATTACK_1 || state == UNIT_STATES.ATTACK_END)
    //    {
    //        GetSkillManager().SetNextSkillPattern();
    //    }

    //    Battle_Mng.StartUltimateSkill();
    //    ChangeState(UNIT_STATES.SKILL_READY_1);
    //}

    //public override void TriggerEventListener(string trigger_id, EventTriggerValue evt_val)
    //{
    //    //  감추기
    //    if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_hide.ToString()))
    //    {
    //        HideCharacters(evt_val);
    //    }   //  보이기
    //    else if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_show.ToString()))
    //    {
    //        ShowCharacters(evt_val);
    //    }
    //}
    #endregion


}

using UnityEngine;

public class Monster_900001 : MonsterBase_V2
{

    #region States
    public override void UnitStateSkillReady01()
    {
        if (!IsAlive())
        {
            ChangeState(UNIT_STATES.DEATH);
            return;
        }
        ChangeState(UNIT_STATES.SKILL_1);
    }

    public override void UnitStateSkill01Begin()
    {
        string skill_action_name = GetSkillManager().GetSpecialSkillGroup().GetSkillActionName();
        var name_list = skill_action_name.Split('_');
        int track = 0;
        if (name_list.Length > 0)
        {
            track = int.Parse(name_list[0]);
        }
        PlayAnimation(track, skill_action_name, false);
    }


    #endregion




    #region Etc Funcs

    public override void UltimateSkillExec()
    {
        var battle_state = Battle_Mng.GetCurrentState();
        if (battle_state != GAME_STATES.PLAYING)
        {
            return;
        }
        var state = GetCurrentState();
        if (state == UNIT_STATES.SKILL_1 || state == UNIT_STATES.SKILL_READY_1 || state == UNIT_STATES.SKILL_END || state == UNIT_STATES.ULTIMATE_PAUSE)
        {
            return;
        }
        var ultimate_skill = GetSkillManager().GetSpecialSkillGroup();
        var target_skill = ultimate_skill.GetSpecialSkillTargetSkill();
        if (target_skill != null)
        {
            FindTargets(target_skill);
        }

        ChangeState(UNIT_STATES.SKILL_READY_1);
    }

    protected override void AttackAnimationComplete()
    {
        //  궁극기가 있는지 찾는다.
        var ultimate_skill = GetSkillManager().GetSpecialSkillGroup();
        if (ultimate_skill != null)
        {
            if (ultimate_skill.IsPrepareCooltime())
            {
                //  궁극기 사용 요청
                UltimateSkillExec();
            }
            else
            {
                //  궁극기 준비되어 있지 않으면 다음 순서의 일반 스킬 사용
                ChangeState(UNIT_STATES.ATTACK_READY_1);
            }
        }
        else
        {
            //  없으면 다음 순서의 일반 스킬 사용
            ChangeState(UNIT_STATES.ATTACK_READY_1);
        }
    }

    protected override void PlayAnimation(HERO_PLAY_ANIMATION_TYPE ani_type)
    {
        switch (ani_type)
        {
            case HERO_PLAY_ANIMATION_TYPE.NONE:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_01:
                PlayAnimation(0, "00_idle", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                PlayAnimation(0, "00_run", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                PlayAnimation(0, "00_damage", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.STUN:
                PlayAnimation(0, "00_stun", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                PlayAnimation(0, "00_death", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.ULTIMATE_01:
                PlayAnimation(0, "00_ultimate_enemy", false);
                break;
        }
    }



    #endregion


}

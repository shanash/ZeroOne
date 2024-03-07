public class Hero_100005 : HeroBase_V2
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
        }
    }
    #endregion

}

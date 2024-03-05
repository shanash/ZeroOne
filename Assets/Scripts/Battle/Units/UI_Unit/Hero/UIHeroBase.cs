using Spine.Unity;
using UnityEngine;

public class UIHeroBase : UnitBase_V2
{
    [SerializeField, Tooltip("Skeleton Grp")]
    protected SkeletonGraphic Skeleton;


    public void PlayAnimation(HERO_PLAY_ANIMATION_TYPE ani_type)
    {
        try
        {
            switch (ani_type)
            {
                case HERO_PLAY_ANIMATION_TYPE.NONE:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.PREPARE_01:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.IDLE_01:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                    Skeleton.AnimationState.SetAnimation(0, "00_run", true);
                    break;
                case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.STUN:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                    break;
                case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                    {

                        Skeleton.AnimationState.SetAnimation(0, "00_win", false);
                        Skeleton.AnimationState.AddAnimation(0, "00_win_loop", true, 0);
                    }
                    break;
            }
        }
        catch (System.Exception)
        {
            PlayAnimationV2(ani_type);
        }
        
    }
    public void PlayAnimationV2(HERO_PLAY_ANIMATION_TYPE ani_type)
    {
        switch (ani_type)
        {
            case HERO_PLAY_ANIMATION_TYPE.NONE:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.STUN:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                {

                    Skeleton.AnimationState.SetAnimation(0, "00_win", false);
                    Skeleton.AnimationState.AddAnimation(0, "00_win_loop", true, 0);
                }
                break;
        }
    }

    protected void PlayAnimation(int track, string anim_name, bool loop)
    {
        Skeleton.AnimationState.SetAnimation(track, anim_name, loop);
    }
}

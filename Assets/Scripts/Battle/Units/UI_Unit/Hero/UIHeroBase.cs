using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeroBase : UnitBase_V2
{
    [SerializeField, Tooltip("Skeleton Grp")]
    protected SkeletonGraphic Skeleton;


    public void PlayAnimation(HERO_PLAY_ANIMATION_TYPE ani_type)
    {
        switch (ani_type)
        {
            case HERO_PLAY_ANIMATION_TYPE.NONE:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.READY_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.READY_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.JUMP_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.JUMP_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.WALK_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.ATTACK_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.ATTACK_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.ATTACK_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.SKILL_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.SKILL_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.SKILL_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                {
                    Skeleton.AnimationState.SetAnimation(1, "1_win", false);
                    Skeleton.AnimationState.AddAnimation(1, "1_idle", true, 0);
                }
                break;
        }
    }

    protected void PlayAnimation(int track, string anim_name, bool loop)
    {
        Skeleton.AnimationState.SetAnimation(track, anim_name, loop);
    }
}

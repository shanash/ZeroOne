using Cinemachine;
using Spine;
using UnityEngine;
using UnityEngine.Timeline;

public class Hero_100001 : HeroBase_V2
{


    #region States


    //public override void UnitStateMoveIn()
    //{
    //    if (Team_Type == TEAM_TYPE.LEFT)
    //    {
    //        MoveLeftTeam();
    //    }
    //    else
    //    {
    //        MoveRightTeam();
    //    }

    //}

    //public override void UnitStateMove()
    //{
    //    MoveLeftTeam();
    //    base.UnitStateMove();
    //}

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
        if (Ultimate_Skill_Playable_Director == null)
        {
            return;
        }
        var stage_cam = Battle_Mng.GetVirtualCineManager().GetStageCamera();
        var unit_back_bg = Battle_Mng.GetBattleField().GetUnitBackFaceBG();

        var ta = (TimelineAsset)Ultimate_Skill_Playable_Director.playableAsset;
        var tracks = ta.GetOutputTracks();
        foreach (var track in tracks)
        {
            if (track is AnimationTrack)
            {
                if (track.name.Equals("Unit_Back_BG_Animation_Track"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, unit_back_bg.GetComponent<Animator>());
                }
                else if (track.name.Equals("Stage_Cam_Animation_Track"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, stage_cam.GetComponent<Animator>());
                }
            }
            else if (track is CinemachineTrack)
            {
                Ultimate_Skill_Playable_Director.SetGenericBinding(track, Camera.main.GetComponent<CinemachineBrain>());
            }
        }

        Ultimate_Skill_Playable_Director.Play();
    }

    protected override void UnsetPlayableDirector()
    {
        if (Ultimate_Skill_Playable_Director == null)
        {
            return;
        }


        var ta = (TimelineAsset)Ultimate_Skill_Playable_Director.playableAsset;
        var tracks = ta.GetOutputTracks();

        foreach (var track in tracks)
        {
            if (track is AnimationTrack)
            {
                if (track.name.Equals("Unit_Back_BG_Animation_Track"))
                {
                    Ultimate_Skill_Playable_Director.ClearGenericBinding(track);
                }
                else if (track.name.Equals("Stage_Cam_Animation_Track"))
                {
                    Ultimate_Skill_Playable_Director.ClearGenericBinding(track);
                }
            }
            else if (track is CinemachineTrack)
            {
                Ultimate_Skill_Playable_Director.ClearGenericBinding(track);
            }

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
        }
    }
    #endregion
    

}

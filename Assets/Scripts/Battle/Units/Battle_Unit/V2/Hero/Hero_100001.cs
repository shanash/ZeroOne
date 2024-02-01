using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


/// <summary>
/// 루시아
/// </summary>
public class Hero_100001 : HeroBase_V2
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
        if (Ultimate_Skill_Playable_Director == null)
        {
            return;
        }
        
        var unit_back_bg = Battle_Mng.GetBattleField().GetUnitBackFaceBG();
        var virtual_cam = Battle_Mng.GetVirtualCineManager();
        var brain_cam = virtual_cam.GetBrainCam();
        var stage_cam = virtual_cam.GetStageCamera();

        var character_cam = virtual_cam.GetCharacterCamera();
        var active_group_cam = virtual_cam.GetActiveTargetGroupCamera();
        var active_group = virtual_cam.GetActiveTargetGroup();

        var ta = (TimelineAsset)Ultimate_Skill_Playable_Director.playableAsset;
        var tracks = ta.GetOutputTracks();
        foreach (var track in tracks)
        {
            if (track is AnimationTrack)
            {
                if (track.name.Equals("imation_TrackAnimation_Track"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, unit_back_bg.GetComponent<Animator>());
                }
                else if (track.name.Equals("CharacterCameraAnimationTrack"))
                {
                    character_cam.Follow = this.transform;
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, character_cam.GetComponent<Animator>());
                }
                else if (track.name.Equals("ActiveGroupCameraAnimationTrack"))
                {
                    active_group_cam.Follow = active_group.transform;
                    int t_cnt = Normal_Attack_Target.Count;
                    float weight = 1f / t_cnt;
                    for (int i = 0; i < t_cnt; i++)
                    {
                        active_group.AddMember(Normal_Attack_Target[i].transform, weight, 2);
                    }
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, active_group_cam.GetComponent<Animator>());
                }
            }
            
            else if (track is CinemachineTrack)
            {
                Ultimate_Skill_Playable_Director.SetGenericBinding(track, brain_cam);
                var clips = track.GetClips();
                foreach (var clip in clips)
                {
                    CinemachineShot shot = clip.asset as CinemachineShot;
                    if (shot != null)
                    {
                        if (shot.DisplayName.Equals("CharacterCamera"))
                        {
                            Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, character_cam);
                        }
                        else if (shot.DisplayName.Equals("ActiveGroupCamera"))
                        {
                            Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, active_group_cam);
                        }
                        else if (shot.DisplayName.Equals("StageCamera"))
                        {
                            Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, stage_cam);
                        }

                    }
                }
            }
        }

        StartPlayableDirector();
    }

    protected override void UnsetPlayableDirector()
    {
        var virtual_mng = Battle_Mng.GetVirtualCineManager();

        var character_cam = virtual_mng.GetCharacterCamera();
        if (character_cam != null)
        {
            character_cam.Follow = null;
        }

        var active_group_cam = virtual_mng.GetActiveTargetGroupCamera();
        if (active_group_cam != null)
        {
            active_group_cam.Follow = null;
        }


        var active_target_group = virtual_mng.GetActiveTargetGroup();
        if (active_target_group != null)
        {
            int cnt = Normal_Attack_Target.Count;
            for (int i = 0; i < cnt; i++)
            {
                active_target_group.RemoveMember(Normal_Attack_Target[i].transform);
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

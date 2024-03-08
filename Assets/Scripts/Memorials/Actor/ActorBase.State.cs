using Spine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZeroOne.Input;

public abstract partial class ActorBase : MonoBehaviour
{
    protected StateSystemBase<ACTOR_STATES> FSM = null;

    // 플레이되는 리액션 애니메이션 트랙들이 모두 종료되고서야 IDLE로 돌아가도록 하기 위해 체크할 트랙을 담아놓는다
    protected TrackEntry react_track_entry = null;

    // 근원전달 및 다른곳에서 탈착해야 하는 장비가 있으면 여기에 설정
    protected TrackEntry equip_track_entry = null;

    TrackEntry te_vert;
    TrackEntry te_horz;

    protected virtual void InitStates()
    {
        FSM = new StateSystemBase<ACTOR_STATES>();
        FSM.AddTransition(new ActorStateIntro());
        FSM.AddTransition(new ActorStateIdle());
        FSM.AddTransition(new ActorStateReact());
        FSM.AddTransition(new ActorStateDrag());
        FSM.AddTransition(new ActorStateNade());
        FSM.AddTransition(new ActorStateEyeTracker());
    }

    public virtual void Lazy_Init(ACTOR_STATES trans)
    {
        if (FSM == null)
        {
            InitStates();
        }

        FSM.Lazy_Init_Setting(trans, this);
    }

    public void ChangeState(ACTOR_STATES trans)
    {
        FSM?.ChangeState(trans);
    }

    public void RevertState()
    {
        FSM?.RevertState();
    }

    public ACTOR_STATES GetCurrentState()
    {
        if (FSM != null)
        {
            return FSM.CurrentTransitionID;
        }
        return ACTOR_STATES.NONE;
    }

    public ACTOR_STATES GetPreviousState()
    {
        if (FSM != null)
        {
            return FSM.PreviousTransitionID;
        }
        return ACTOR_STATES.NONE;
    }

    public virtual void OnUpdate(float dt)
    {
        FSM?.UpdateState();
    }

    public virtual void ActorStateIdleBegin()
    {
        var te = FindTrack(IDLE_BASE_TRACK);

        bool need_set_anim = false;
        float mix_duration = 0.0f;

        if (te == null || te.IsComplete || te.IsEmptyAnimation)
        {
            need_set_anim = true;
        }
        else if (!te.Animation.Name.Equals(Idle_Animation_Data.Animation_Idle_Name))
        {
            need_set_anim = true;
            mix_duration = 0.2f;
        }

        if (need_set_anim)
        {
            Skeleton.AnimationState.SetAnimation(IDLE_BASE_TRACK, Idle_Animation_Data.Animation_Idle_Name, true).MixDuration = mix_duration;
            if (equip_track_entry == null || equip_track_entry.IsComplete)
            {
                equip_track_entry = Skeleton.AnimationState.SetAnimation(90, (Location_Type == SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE) ? "90_equipment_off" : "90_equipment_on", true);
            }
        }
    }

    public virtual void ActorStateReactBegin()
    {
        // UI버튼들 입력을 애니메이션 끝날때까지 막는다.
        ScreenEffectManager.I.SetBlockInputUI(true);
        GestureManager.Instance.Enable = false;

        if (Current_Timeline_Id != 0)
        {
            Director.Play();
        }
        else
        {
            // 챗모션 데이터로부터 해당 애니메이션 전부 플레이
            var chat_motion_data = Reaction_Chat_Motions[Current_Chat_Motion_Id];
            if (!TryGetTrackNum(chat_motion_data.animation_name, out int track_num))
            {
                throw new InvalidTrackException(track_num);
            }

            var te = Skeleton.AnimationState.SetAnimation(track_num, chat_motion_data.animation_name, false);

            // 아이들 트랙은 기존 아이들 애니메이션을 끊고 들어가야 하기 때문에 mix duration을 줍니다
            if (IDLE_BASE_TRACK == track_num)
            {
                te.MixDuration = 0.2f;
            }

            // 이 엔트리들이 전부 재생완료 되면 React 상태가 종료되도록 합니다
            react_track_entry = te;
        }
        Current_Serifu_Index = -1;

        // 연속 제스쳐 카운트 갯수 하나 증가
        if (Current_Interaction != null)
        {
            var key = (Current_Interaction.gescure_type_01, Current_Interaction.touch_type_01);
            if (!Gesture_Touch_Counts.Key.Equals(key))
            {
                Gesture_Touch_Counts = new KeyValuePair<(TOUCH_GESTURE_TYPE geture_type, TOUCH_BODY_TYPE body_type), int>(key, 1);
            }
            else
            {
                Gesture_Touch_Counts = new KeyValuePair<(TOUCH_GESTURE_TYPE geture_type, TOUCH_BODY_TYPE body_type), int>(key, Gesture_Touch_Counts.Value + 1);
            }
        }
    }

    public virtual void ActorStateReactUpdate()
    {
        var te = FindTrack(MOUTH_TRACK);
        if (te != null)
        {
            Elapsed_Time_For_Mouth_Open += Time.deltaTime;
            te.Alpha = Mathf.Lerp(Origin_Mouth_Alpha, Dest_Mouth_Alpha, System.Math.Min(1.0f, Elapsed_Time_For_Mouth_Open / AudioManager.VOICE_TERM_SECONDS));
        }

        if (Current_Timeline_Id != 0 && Director.state == UnityEngine.Playables.PlayState.Paused)
        {
            GameObject.Find("Square").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            Camera.main.gameObject.transform.position = new Vector3(0, 0, -10);
            Camera.main.gameObject.transform.eulerAngles = Vector3.zero;
            Camera.main.orthographicSize = 10;
            FSM.ChangeState(ACTOR_STATES.IDLE);
        }
    }

    public virtual void ActorStateReactExit()
    {
        // 애니메이션 끝나면 풉니다.
        GestureManager.Instance.Enable = true;
        ScreenEffectManager.I.SetBlockInputUI(false);

        if (Is_Playing_Success_Transfer_Essence)
        {
            Is_Playing_Success_Transfer_Essence = false;
            OnCompleteTransferEssence();
        }
        if (OnSendMessage == null)
        {
            DisappearBalloon();
        }

        if (Current_State_Id != Current_Interaction.after_state_id)
        {
            SetActor(Current_Interaction.after_state_id);
        }

        react_track_entry = null;
        Current_Chat_Motion_Id = -1;
    }

    public virtual void ActorStateDragBegin()
    {
        /*
        if (!TryGetTrackNum(Current_Interaction.drag_animation_name, out int track_num))
        {
            throw new InvalidTrackException(track_num);
        }

        Drag_Track_Entry = Skeleton.AnimationState.SetAnimation(track_num, Current_Interaction.drag_animation_name, false);
        Drag_Track_Entry.MixDuration = 0.0f;
        Drag_Track_Entry.TimeScale = 0.0f;
        */
    }

    public virtual void ActorStateDragEnd()
    {
        Skeleton.AnimationState.SetEmptyAnimation(Drag_Track_Entry.TrackIndex, 0.0f);
    }

    public virtual void ActorStateNadeBegin()
    {
        Skeleton.AnimationState.SetAnimation(30, "30_nade_in", false).MixDuration = 0.2f;
        Skeleton.AnimationState.AddAnimation(30, "30_nade_idle", true, 0);

        te_horz = FindTrack(31);
        te_vert = FindTrack(32);
    }

    public virtual void ActorStateNadeUpdate()
    {
        UpdateFaceAnimationDirection(0.1f, ref te_horz, ref te_vert, "31_nade_Right", "32_nade_Up", "31_nade_Left", "32_nade_Down");

        if (!ShouldContinueMovingFace())
        {
            FSM.ChangeState(ACTOR_STATES.IDLE);
        }
    }

    public virtual void ActorStateNadeEnd()
    {
        float mix_duration = 0.0f;
        var te = FindTrack(30);
        if (te.Animation.Name.Equals("30_nade_in"))
        {
            mix_duration = 0.2f;
        }

        Skeleton.AnimationState.SetAnimation(30, "30_nade_out", false).MixDuration = mix_duration;
        Skeleton.AnimationState.AddEmptyAnimation(30, 0.2f, 0.0f);
        Skeleton.AnimationState.SetEmptyAnimation(31, 0);
        Skeleton.AnimationState.SetEmptyAnimation(32, 0);

        Dragged_Canvas_Position = Vector2.zero;
    }

    public virtual void ActorStateEyeTrakerBegin()
    {
        te_horz = FindTrack(10);
        te_vert = FindTrack(11);
    }

    public virtual void ActorStateEyeTrackerUpdate()
    {
        UpdateFaceAnimationDirection(1, ref te_horz, ref te_vert, "10_Right", "11_Up", "10_Left", "11_Down");

        if (!ShouldContinueMovingFace())
        {
            FSM.ChangeState(ACTOR_STATES.IDLE);
        }
    }

    public virtual void ActorStateEyeTrakerEnd()
    {
        Skeleton.AnimationState.SetEmptyAnimation(10, 0);
        Skeleton.AnimationState.SetEmptyAnimation(11, 0);
    }
}

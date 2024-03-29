using Cinemachine;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using ZeroOne.Input;

public abstract partial class ActorBase : MonoBehaviour, IActorPositionProvider, FluffyDuck.Util.MonoFactory.IProduct
{
    // 보정 X, Y값
    const float ADDED_POS_X = 0f;
    const float ADDED_POS_Y = -0.75496f;
    const float ESSENCE_POS_Y = -8f;

    protected static readonly float FACE_MOVE_MAX_DISTANCE = (float)GameDefine.RESOLUTION_SCREEN_HEIGHT / 4;
    protected const int IDLE_BASE_TRACK = 0;
    protected const int MOUTH_TRACK = 17;
    //const string FACE_BONE_NAME = "touch_center";
    //const string BALLOON_BONE_NAME = "balloon";

    [SerializeField]
    protected SkeletonAnimation Skeleton;

    /*
    [SerializeField]
    Collider2D Head;
    */

    [SerializeField]
    protected PlayableDirector Director = null;

    // 입모양 애니메이션
    [SerializeField, Tooltip("말할때 입 벌어지는 크기 배율 조정"), Range(1, 10)]
    float Talk_Mouth_Wide_Multiple = 10.0f;

    // 얼굴 방향 움직임    
    [SerializeField, Tooltip("얼굴이 포인터를 따라가는 속도"), Range(1, 10)]
    float Look_At_Speed = 3.0f;
    Vector2 Dragged_Canvas_Position;
    Vector2 Current_Face_Direction;// 얼굴 방향

    ////////////////////////// 상태
    protected IdleAnimationData Idle_Animation_Data;
    KeyValuePair<(TOUCH_GESTURE_TYPE geture_type, TOUCH_BODY_TYPE body_type), int> Gesture_Touch_Counts;
    protected int Idle_Played_Count;

    protected Bone Face; // 얼굴 위치를 가져오기 위한 본
    protected Bone Balloon; // 말풍선 위치 본

    Producer Producer;
    L2d_Interaction_Base_Data Current_Interaction;

    L2d_Love_State_Data LoveState;
    LOVE_LEVEL_TYPE Current_Love_Level_Type => LoveState.love_level_type;

    protected L2d_Skin_Ani_State_Data SkinAniState;
    protected L2d_Char_Skin_Data SkinData;

    // 키의 bool 값은 근원전달 성공 실패 값이다
    // 근원 전달이 아닌 상황에서는 false로 찾아주세요
    protected Dictionary<Tuple<TOUCH_BODY_TYPE, TOUCH_GESTURE_TYPE, bool>, L2d_Interaction_Base_Data> Interaction_Bases;
    TOUCH_BODY_TYPE Essence_Success_Body = TOUCH_BODY_TYPE.NONE;

    protected Me_Chat_Motion_Data Idle_Chat_Motion;
    protected Dictionary<int, Me_Chat_Motion_Data> Reaction_Chat_Motions;

    protected int Current_Chat_Motion_Id;
    protected int Current_Timeline_Id;
    protected int Current_Serifu_Index;
    protected int Current_Balloon_ID = -1; // 표시되어 있는 말풍선 아이디

    Dictionary<int, Me_Serifu_Data> Serifues;
    string Current_Mouth_Anim_Name;

    TrackEntry Drag_Track_Entry;

    float Origin_Mouth_Alpha = 0; // 원래 입 크기
    float Dest_Mouth_Alpha = 0; // 움직이기 위한 목표로 하는 입 크기
    float Elapsed_Time_For_Mouth_Open = 0;

    bool Is_Quit = false;

    protected abstract Spine.AnimationState AnimationState { get; }
    protected int Current_State_Id => SkinAniState.state_id;

    // 대사 전달
    public event Action<string, float, bool> OnSendMessage;
    // 근원전달 성공 여부 전달
    public event Action<bool, TOUCH_BODY_TYPE> OnResultTransferEssence;
    // 근원전달 성공 애니메이션 종료
    public event Action OnCompleteTransferEssence;

    bool Is_Playing_Success_Transfer_Essence = false;
    SPINE_CHARA_LOCATION_TYPE Location_Type = SPINE_CHARA_LOCATION_TYPE.NONE;

    #region MonoBehaviour Methods

    void OnApplicationQuit()
    {
        Is_Quit = true;
    }

    void OnDestroy()
    {
        if (!Is_Quit)
        {
            RemoveGestureEventListener();
            RemoveSkeletonEventListener();
        }
    }

    void Update()
    {
        FSM.UpdateState();
    }
    #endregion

    #region Spine Animation Callbacks
    /// <summary>
    /// 스파인 애니메이션 시작시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void SpineAnimationStart(TrackEntry entry)
    {
        //Debug.Log($"SpineAnimationStart : {entry.Animation.Name}".WithColorTag(Color.red));
    }

    /// <summary>
    /// 스파인 애니메이션이 끊겼을때 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void SpineAnimationInterrupt(TrackEntry entry)
    {

    }

    /// <summary>
    /// 스파인 애니메이션 동작 완료시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void SpineAnimationComplete(TrackEntry entry)
    {
        if (entry == null || entry.IsEmptyAnimation)
        {
            return;
        }

        if (entry.TrackIndex != 30)
        {
            //Debug.Log($"SpineAnimationComplete : {entry.Animation.Name}".WithColorTag(Color.red));
        }

        if (FSM.CurrentTransitionID == ACTOR_STATES.REACT)
        {
            if (Current_Timeline_Id == 0)
            {
                if (react_track_entry == entry)
                {
                    if (entry.TrackIndex != IDLE_BASE_TRACK)
                    {
                        Skeleton.AnimationState.SetEmptyAnimation(entry.TrackIndex, 0);
                    }
                    else
                    {
                        Skeleton.AnimationState.SetAnimation(IDLE_BASE_TRACK, Idle_Animation_Data.Animation_Idle_Name, true);
                    }

                    react_track_entry = null;
                    FSM.ChangeState(ACTOR_STATES.IDLE);
                }
            }
        }
    }

    /// <summary>
    /// 스파인 애니메이션 동작 종료시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void SpineAnimationEnd(TrackEntry entry)
    {
        //Debug.Log($"SpineAnimationEnd : {entry.Animation.Name}".WithColorTag(Color.red));
    }

    /// <summary>
    /// 스파인 애니메이션 동작 플레이 중 호출되는 이벤트를 받아주는 리스너
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="evt"></param>
    protected virtual void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        Debug.Log($"SpineAnimationEvent : {entry.Animation.Name} : {evt.Data.Name} : {evt.String}");
        SpineCharaDefine.TryParseEvent(evt.Data.Name.ToUpper(), out SpineCharaDefine.SPINE_EVENT eEvt);
        switch (eEvt)
        {
            case SpineCharaDefine.SPINE_EVENT.MOUTH_SHAPE: // 입모양 애니메이션 지정. 이후 입모양이 움직이는 애니메이션으로 이행
                Current_Mouth_Anim_Name = evt.String.Equals("close") ? string.Empty : evt.String;
                break;
            case SpineCharaDefine.SPINE_EVENT.VOICE: // 대사 출력 및 재생
                if (Current_Chat_Motion_Id == -1)
                {
                    Debug.Assert(false, $"출력할 대사가 없습니다. {Current_Chat_Motion_Id} :: {Current_Serifu_Index}");
                    return;
                }

                Current_Serifu_Index++;
                var serifu = GetSerifuData(Reaction_Chat_Motions[Current_Chat_Motion_Id], Current_Serifu_Index);
                if (serifu == null)
                {
                    Debug.LogWarning($"출력할 대사가 없습니다. {Current_Chat_Motion_Id} :: {Current_Serifu_Index}");
                    return;
                }

                string sound_key = GameDefine.GetLocalizeString(serifu.audio_clip_id);
                float voice_length = 0.0f;
                if (!string.IsNullOrEmpty(sound_key))
                {
                    voice_length = AudioManager.Instance.GetClipLength(GameDefine.GetLocalizeString(serifu.audio_clip_id));
                    AudioManager.Instance.PlayVoice(GameDefine.GetLocalizeString(serifu.audio_clip_id), false, OnAudioStateAndVolumeChanged);
                }

                if (!string.IsNullOrEmpty(GameDefine.GetLocalizeString(serifu.dialogue_text_id)))
                {
                    OnSendMessage?.Invoke(GameDefine.GetLocalizeString(serifu.dialogue_text_id), voice_length, true);
                    if (OnSendMessage == null)
                    {
                        DisappearBalloon();
                        SpeechBalloonManager.Instance.CreateBalloon(
                            (balloon_id) =>
                            {
                                Current_Balloon_ID = balloon_id;
                            },
                            GameDefine.GetLocalizeString(serifu.dialogue_text_id),
                            this,
                            new Vector2(200, 170),
                            SpeechBalloon.BalloonSizeType.FixedWidth,
                            SpeechBalloon.Pivot.Right);
                    }
                }
                break;
            case SpineCharaDefine.SPINE_EVENT.SOUND: // 사운드만 재생
                AudioManager.Instance.PlayFX($"Assets/AssetResources/Audio/Voice/Eileen/{evt.String}");
                break;
        }
    }
    #endregion

    /// <summary>
    /// 스켈레톤(스파인) 이벤트 리스너 등록
    /// </summary>
    protected void AddSkeletonEventListener()
    {
        if (Skeleton != null)
        {
            Skeleton.AnimationState.Start += SpineAnimationStart;
            Skeleton.AnimationState.Complete += SpineAnimationComplete;
            Skeleton.AnimationState.End += SpineAnimationEnd;
            Skeleton.AnimationState.Event += SpineAnimationEvent;
        }
    }

    /// <summary>
    /// 스켈레톤(스파인) 이벤트 리스너 등록
    /// </summary>
    protected void RemoveSkeletonEventListener()
    {
        if (Skeleton != null)
        {
            Skeleton.AnimationState.Start -= SpineAnimationStart;
            Skeleton.AnimationState.Complete -= SpineAnimationComplete;
            Skeleton.AnimationState.End -= SpineAnimationEnd;
            Skeleton.AnimationState.Event -= SpineAnimationEvent;
        }
    }

    public void OnAudioStateAndVolumeChanged(string sound_key, AudioManager.AUDIO_STATES audio_state, float volume_rms)
    {
        TrackEntry te;
        switch (audio_state)
        {
            case AudioManager.AUDIO_STATES.PLAYING:
                te = FindTrack(MOUTH_TRACK);
                if (te == null || !te.Animation.Name.Equals(Current_Mouth_Anim_Name))
                {
                    if (Current_Mouth_Anim_Name.Equals(string.Empty))
                    {
                        return;
                    }
                    te = Skeleton.AnimationState.SetAnimation(MOUTH_TRACK, Current_Mouth_Anim_Name, false);
                    te.MixDuration = 0.2f;
                    te.Alpha = 0;
                    Dest_Mouth_Alpha = 0;
                }
                Elapsed_Time_For_Mouth_Open = 0;
                Origin_Mouth_Alpha = te.Alpha;
                Dest_Mouth_Alpha = Mathf.Clamp(volume_rms * Talk_Mouth_Wide_Multiple, 0, 1);
                break;
            case AudioManager.AUDIO_STATES.END:
                if (OnSendMessage == null)
                {
                    DisappearBalloon();
                }
                te = FindTrack(MOUTH_TRACK);
                if (te != null)
                {
                    te.Alpha = 0;
                }
                break;
            default:
                break;
        }
    }

    bool IsColliderVisible(Collider2D collider)
    {
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        Bounds camBounds = new Bounds(cam.transform.position, new Vector3(camWidth, camHeight, 100));

        return camBounds.Intersects(collider.bounds);
    }

    #region Gesture Callbacks
    protected virtual void OnTap(ICursorInteractable comp)
    {
        Debug.Log($"OnTap : {comp.GameObjectName}");
        if (this == null)
        {
            RemoveGestureEventListener();
            return;
        }

        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }

        if (FSM.CurrentTransitionID != ACTOR_STATES.IDLE)
        {
            return;
        }

        var bounding_box = comp as SpineBoundingBox;
        if (bounding_box == null)
        {
            Debug.Assert(false, "ActorBase::OnTap : 터치된 bounding_box가 존재하지 않습니다.");
            return;
        }

        Current_Interaction = GetInteractionData(TOUCH_GESTURE_TYPE.CLICK, bounding_box);
        if (Current_Interaction == null)
        {
            return;
        }

        // TODO:M2 스펙때문에 실패도 성공 파티클이 보여야 하기 때문에 일단 막습니다.. 밖에 EssenceController에서 처리할겁니다
        /*
        if (!TouchCanvas.Instance.Touch_Effect_Enable)
        {
            TouchCanvas.Instance.SetTouchEffectPrefabPath(Current_Interaction.check_using_essense ? TouchCanvas.Effect_Pink_Path : TouchCanvas.Effect_Blue_Path);
        }
        */

        SetReactionData(Current_Interaction, TOUCH_GESTURE_TYPE.CLICK, bounding_box);

        FSM.ChangeState(ACTOR_STATES.REACT);
    }

    protected virtual void OnDoubleTap(ICursorInteractable comp)
    {
        if (this == null)
        {
            RemoveGestureEventListener();
            return;
        }

        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }

        if (FSM.CurrentTransitionID != ACTOR_STATES.IDLE)
        {
            return;
        }

        var bounding_box = comp as SpineBoundingBox;
        if (bounding_box == null)
        {
            Debug.Assert(false, "ActorBase::OnTap : 터치된 bounding_box가 존재하지 않습니다.");
            return;
        }

        Current_Interaction = GetInteractionData(TOUCH_GESTURE_TYPE.DOUBLE_CLICK, bounding_box);
        if (Current_Interaction == null)
        {
            return;
        }

        SetReactionData(Current_Interaction, TOUCH_GESTURE_TYPE.DOUBLE_CLICK, bounding_box);

        FSM.ChangeState(ACTOR_STATES.REACT);
    }

    protected virtual void OnDrag(ICursorInteractable comp, Vector2 position, Vector2 drag_vector, int state)
    {
        if (this == null)
        {
            RemoveGestureEventListener();
            return;
        }

        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }

        bool isDragValid = (FSM.CurrentTransitionID == ACTOR_STATES.DRAG || FSM.CurrentTransitionID == ACTOR_STATES.EYE_TRACKER) && state >= 1;
        bool isIdleValid = FSM.CurrentTransitionID == ACTOR_STATES.IDLE && state == 0;
        if (!isDragValid && !isIdleValid)
        {
            return;
        }

        switch (comp)
        {
            case Background:
                Dragged_Canvas_Position = position;

                switch (state)
                {
                    case 0:
                        FSM.ChangeState(ACTOR_STATES.EYE_TRACKER);
                        break;
                    case 1:
                        break;
                    case 2:
                        Dragged_Canvas_Position = Vector2.zero;
                        break;
                }
                break;
        }
    }

    protected virtual void OnNade(ICursorInteractable obj, Vector2 position, Vector2 delta, int state)
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }
    }
    #endregion

    int GetConsecutiveGestureCount(TOUCH_GESTURE_TYPE gesture_type, TOUCH_BODY_TYPE body_type)
    {
        var key = (gesture_type, body_type);
        if (!Gesture_Touch_Counts.Key.Equals(key))
        {
            return 0;
        }

        return Gesture_Touch_Counts.Value;
    }

    L2d_Interaction_Base_Data GetInteractionData(TOUCH_GESTURE_TYPE type, SpineBoundingBox bounding_box)
    {
        bool is_essence_success =
            bounding_box.GetTouchBodyType() == Essence_Success_Body
            && Essence_Success_Body != TOUCH_BODY_TYPE.NONE;

        var key = Tuple.Create(bounding_box.GetTouchBodyType(), type, is_essence_success);

        if (!Interaction_Bases.ContainsKey(key))
        {
            key = Tuple.Create(TOUCH_BODY_TYPE.NONE, type, false);
            if (!Interaction_Bases.ContainsKey(key))
            {
                Debug.Assert(false, "반응 애니메이션을 찾을 수 없습니다.");

                return null;
            }
        }
        else if (is_essence_success)
        {
            Essence_Success_Body = TOUCH_BODY_TYPE.NONE;
            Is_Playing_Success_Transfer_Essence = true;
        }
        OnResultTransferEssence?.Invoke(is_essence_success, bounding_box.GetTouchBodyType());

        return Interaction_Bases[key];
    }

    void SetReactionData(L2d_Interaction_Base_Data data, TOUCH_GESTURE_TYPE type, SpineBoundingBox bounding_box)
    {
        if (data == null)
        {
            return;
        }
        Current_Interaction = data;
        Current_Chat_Motion_Id = data.reaction_ani_id;
        Current_Timeline_Id = data.reaction_timeline_id;
    }

    protected void AddGestureEventListener()
    {
        //GestureManager.Instance.OnNade += OnNade;
        GestureManager.Instance.OnTap += OnTap;
        GestureManager.Instance.OnDoubleTap += OnDoubleTap;
        GestureManager.Instance.OnDrag += OnDrag;
    }

    protected void RemoveGestureEventListener()
    {
        //GestureManager.Instance.OnNade -= OnNade;
        if (GestureManager.Instance != null)
        {
            GestureManager.Instance.OnTap -= OnTap;
            GestureManager.Instance.OnDoubleTap -= OnDoubleTap;
            GestureManager.Instance.OnDrag -= OnDrag;
        }
    }

    float CalculateTop(Vector3 cam_pos, float fov, Vector3 target)
    {
        var distanceToTarget = Vector3.Distance(cam_pos, target);

        // 수직 FOV의 절반을 라디안으로 변환
        float verticalFOVInRadians = fov * Mathf.Deg2Rad;
        // 카메라로부터 대상까지의 거리를 이용하여 시야 상단의 높이 계산
        float topHeightAtDistance = distanceToTarget * Mathf.Tan(verticalFOVInRadians / 2);

        // 카메라의 현재 y 위치에 계산된 높이를 추가하여 상단의 y값 계산
        return cam_pos.y + topHeightAtDistance;
    }

    bool Initialize(Producer pd, L2d_Char_Skin_Data skin_data, LOVE_LEVEL_TYPE love_level, SPINE_CHARA_LOCATION_TYPE type)
    {
        if (Skeleton == null)
        {
            Debug.Assert(false, $"프리팹 {this.GetType()} 컴포넌트의 인스펙터에 SkeletonAnimation이 없습니다");
            return false;
        }

        if (type == SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE)
        {
            InitTimeline();
        }

        var goCM = GameObject.Find("VirtualCineManager");
        var goMemCamera = goCM.transform.Find("MemorialCamera");
        var Vcam = goMemCamera.GetComponent<CinemachineVirtualCamera>();

        float scale = this.transform.lossyScale.y;
        float[] vertex = null;
        this.Skeleton.Skeleton.GetBounds(out float x, out float y, out float width, out float height, ref vertex);
        float added_pos_x = ADDED_POS_X;
        float added_pos_y = ADDED_POS_Y;
        
        Location_Type = type;

        if (type == SPINE_CHARA_LOCATION_TYPE.LOBBY || type == SPINE_CHARA_LOCATION_TYPE.HERO_INFO)
        {
            // TODO: 로비에 배치되는 Sprite 스탠드를 임시로 입력했던 값(-2.2, 1.3)을 넣고 작업을 하고 계셨다
            // M2 이후에 처리하는게 좋을 것 같습니다.
            var tr = this.transform.Find("Sprite");
            if (tr != null)
            {
                added_pos_x = -2.2f;
                added_pos_y = -1.3f;
            }
            this.transform.position = new Vector3(added_pos_x, (y - height) * scale + CalculateTop(Vcam.transform.position, Vcam.m_Lens.FieldOfView, Vector3.zero) + added_pos_y, this.transform.position.z);
        }
        else if (type == SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE)
        {
            this.transform.position = new Vector3(0, ESSENCE_POS_Y, this.transform.position.z);
        }
        else if (type == SPINE_CHARA_LOCATION_TYPE.LOBBY_EXPECT)
        {
            // TODO: 로비에 배치되는 Sprite 스탠드를 임시로 입력했던 값(-2.2, 1.3)을 넣고 작업을 하고 계셨다
            // M2 이후에 처리하는게 좋을 것 같습니다.
            var tr = this.transform.Find("Sprite");
            if (tr != null)
            {
                added_pos_x = -1.5f;
                added_pos_y = 1.0f;
            }
            this.transform.position = new Vector3(added_pos_x, (y - height) * scale + CalculateTop(Vcam.transform.position, Vcam.m_Lens.FieldOfView, Vector3.zero) + added_pos_y, this.transform.position.z);
        }

        InitField();

        if (!GetSkeletonComponents())
        {
            return false;
        }

        Producer = pd;
        SkinData = skin_data;

        SetActor(love_level);

        Lazy_Init(ACTOR_STATES.IDLE);

        AddGestureEventListener();
        AddSkeletonEventListener();
        return true;
    }

    void InitField()
    {
        Idle_Played_Count = 0;
        Gesture_Touch_Counts = default;
        Face = null;
        Balloon = null;
        Producer = null;
        //Touch_Type_Interactions = null;
        Current_Interaction = null;
        Reaction_Chat_Motions = null;
        Serifues = null;
        Drag_Track_Entry = null;
        Current_Chat_Motion_Id = -1;
        Current_Serifu_Index = -1;
        //Nade_Point = 0;
        Current_Mouth_Anim_Name = string.Empty;
        Dragged_Canvas_Position = Vector2.zero;
        Current_Face_Direction = Vector2.zero;
        Is_Quit = false;
    }

    void InitTimeline()
    {
        //Timeline_Asset = await CommonUtils.GetResourceFromAddressableAsset<TimelineAsset>("Assets/AssetResources/Prefabs/Standing/ST_Eileen/TL_Part3_Lv0");
        //var tracks = Timeline_Asset.GetOutputTracks();
        var Timeline_Asset = Director.playableAsset as TimelineAsset;

        foreach (var track in Timeline_Asset.GetOutputTracks())
        {
            switch(track.name)
            {
                case "Animation Track":
                    Director.SetGenericBinding(track, Camera.main.GetComponent<Animator>());
                    break;
                case "Animation Track (1)":
                    Director.SetGenericBinding(track, GameObject.Find("Square").GetComponent<Animator>());
                    break;
            }
        }
    }

    void SetActor(LOVE_LEVEL_TYPE type)
    {
        LoveState = MasterDataManager.Instance.Get_L2D_LoveState(SkinData.l2d_id, type);
        SetActor(LoveState.state_id);
    }

    void SetActor(int state_id)
    {
        SkinAniState = MasterDataManager.Instance.Get_L2D_SkinAniState(state_id);
        Idle_Chat_Motion = MasterDataManager.Instance.Get_MemorialChatMotion_(SkinAniState.base_ani_id);
        Interaction_Bases = MasterDataManager.Instance.Get_L2D_InteractionBases(SkinAniState.interaction_group_id, false);
        var reaction_ani_ids = Interaction_Bases.Values.Select(i => i.reaction_ani_id).ToList();
        Reaction_Chat_Motions = MasterDataManager.Instance.Get_AniData(reaction_ani_ids);
        var serifu_ids = Reaction_Chat_Motions.Values.SelectMany(chat_motion_data => chat_motion_data.serifu_ids).ToList();
        Serifues = MasterDataManager.Instance.Get_SerifuData(serifu_ids);

        List<string> audio_clip_pathes = Serifues.Values
            .Where(serifu => !string.IsNullOrEmpty(serifu.audio_clip_id))
            .Select(serifu => GameDefine.GetLocalizeString(serifu.audio_clip_id))
            .ToList();

        for (int i = 1; i < 5; i++)
        {
            audio_clip_pathes.Add($"Assets/AssetResources/Audio/Voice/Eileen/Moan_Eileen_{i}");
        }

        AudioManager.Instance.PreloadAudioClipsAsync(audio_clip_pathes, null);

        Idle_Animation_Data = new IdleAnimationData
        {
            Animation_Idle_Name = Idle_Chat_Motion.animation_name,
            Bored_Chatmotion_Ids = null,
            Bored_Count = 0
        };

        Gesture_Touch_Counts = default;
    }

    bool GetSkeletonComponents()
    {
        // 손가락 방향을 따라 얼굴이 따라가는 애니메이션을 위한 얼굴 추적 기준 본과
        // 말풍선을 띄우기 위한 말풍선 본이 현재는 없어서 주석처리했습니다
        //TODO: 나중에 살릴 것
        /*
        try
        {
            Face = FindBone(FACE_BONE_NAME);
            Debug.Assert(Face != null, $"얼굴 추적 기준 본이 없습니다 : {FACE_BONE_NAME}");
            Balloon = FindBone(BALLOON_BONE_NAME);
            Debug.Assert(Face != null, $"말풍선용 본이 없습니다 : {BALLOON_BONE_NAME}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        */
        //return Face != null && Balloon != null;
        return true;
    }

    /// <summary>
    /// 해당 캐릭터의 대사 데이터를 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Me_Serifu_Data GetSerifuData(Me_Chat_Motion_Data data, int serifu_ids_index)
    {
        if (Serifues == null)
        {
            Debug.Assert(false, "정상적으로 초기화가 되지 않았습니다.");
            return null;
        }

        if (Serifues.Count == 0 || data == null)
        {
            return null;
        }

        if (data.serifu_ids.Count() <= serifu_ids_index)
        {
            return null;
        }

        return Serifues[data.serifu_ids[serifu_ids_index]];
    }

    /// <summary>
    /// 현재 표시되는 말풍선을 삭제
    /// </summary>
    void DisappearBalloon()
    {
        if (Current_Balloon_ID == -1)
        {
            return;
        }
        SpeechBalloonManager.Instance.DisappearBalloon(Current_Balloon_ID);
        Current_Balloon_ID = -1;
    }

    /// <summary>
    /// Attachment 찾기
    /// </summary>
    /// <param name="slot_name"></param>
    /// <param name="attachment_name"></param>
    /// <returns></returns>
    public Attachment FindAttachment(string slot_name, string attachment_name)
    {
        var slot = Skeleton.Skeleton.FindSlot(slot_name);
        if (slot != null)
        {
            return Skeleton.Skeleton.GetAttachment(slot_name, attachment_name);
        }
        return null;
    }

    public void SetEssenceBodyPart(TOUCH_BODY_TYPE type)
    {
        if (type == TOUCH_BODY_TYPE.NONE)
        {
            type = (TOUCH_BODY_TYPE)UnityEngine.Random.Range(1, 5);
        }
        Essence_Success_Body = type;

        Debug.Log($"SetEssenceBodyPart : {Essence_Success_Body}");
    }

    /// <summary>
    /// 입력한 애니메이션의 지정트랙을 가져온다
    /// 규격외의 이름이 입력된 경우엔 -1을 돌려준다
    /// </summary>
    /// <param name="animation_name">애니메이션 이름</param>
    /// <returns>트랙 넘버</returns>
    protected static bool TryGetTrackNum(string animation_name, out int track_index)
    {
        string[] word = animation_name.Split('_');
        bool result = int.TryParse(word[0], out int num);
        track_index = result ? num : -1;
        return result;
    }

    /// <summary>
    /// 현재 프레임의 얼굴 방향 업데이트
    /// </summary>
    void UpdateFaceAnimationDirection(float multiple_value, ref TrackEntry track1, ref TrackEntry track2, params string[] anim_names)
    {
        Vector3 face_world_pos = Face.GetWorldPosition(Skeleton.transform);
        Vector2 face_screen_pos = Camera.main.WorldToScreenPoint(face_world_pos);
        Vector2 dest_direction = CalculateDesiredDirection(face_screen_pos, multiple_value);

        Current_Face_Direction = Vector2.Lerp(Current_Face_Direction, dest_direction, Time.deltaTime * Look_At_Speed);

        SetFaceAnimation(ref track1, Current_Face_Direction.x, anim_names[0], anim_names[2]);
        SetFaceAnimation(ref track2, Current_Face_Direction.y, anim_names[1], anim_names[3]);
    }

    /// <summary>
    /// 목표로 하는 마우스 클릭방향 수치 정제
    /// Dragged_Canvas_Position에 제로값이 들어가 있을경우는 손을 뗀 상태니까
    /// 캐릭터는 서서히 정면을 바라보도록 합니다
    /// </summary>
    /// <param name="face_screen_pos">얼굴 본 위치</param>
    /// <returns></returns>
    Vector2 CalculateDesiredDirection(Vector2 face_screen_pos, float multiple_value)
    {
        Vector2 dest_direction = Vector2.zero;

        float face_move_max_distance = FACE_MOVE_MAX_DISTANCE * multiple_value;

        if (!Dragged_Canvas_Position.Equals(Vector2.zero))
        {
            // Dragged_Canvas_Position - face_screen_pos = 얼굴에서 드래그중인 포인트까지의 벡터
            // 벡터 방향은 살린채 ClampMagnitude로 최대값을 FACE_MOVE_MAX_DISTANCE로 제한합니다
            dest_direction = Vector2.ClampMagnitude(Dragged_Canvas_Position - face_screen_pos, FACE_MOVE_MAX_DISTANCE);
            // dest_direction에 Mathf.Sqrt(2.0f) / FACE_MOVE_MAX_DISTANCE를 곱해서
            // 자연스럽게 축소시키고, 45도 각도의 벡터 최대값이 (1,1)이 되도록 맞춥니다
            dest_direction *= Mathf.Sqrt(2.0f) / face_move_max_distance;

            // 1 ~ -1 값 외엔 다 잘라냅니다
            dest_direction.x = Mathf.Clamp(dest_direction.x, -1f, 1f);
            dest_direction.y = Mathf.Clamp(dest_direction.y, -1f, 1f);
        }

        return dest_direction;
    }

    /// <summary>
    /// 얼굴 애니메이션 설정
    /// </summary>
    /// <param name="track">트랙</param>
    /// <param name="track_index">트랙 인덱스</param>
    /// <param name="direction_value">방향값, 상단오른쪽이 플러스, 하단 왼쪽이 마이너스</param>
    /// <param name="positive_anim_name">플러스값 애니메이션 이름</param>
    /// <param name="negative_anim_name">마이너스값 애니메이션 이름</param>
    void SetFaceAnimation(ref TrackEntry track, float direction_value, string positive_anim_name, string negative_anim_name)
    {
        string animation_name = $"{(direction_value > 0 ? positive_anim_name : negative_anim_name)}";
        if (track == null || track.Animation.Name != animation_name)
        {
            if (track != null)
            {
                track.Alpha = 0;
            }
            TryGetTrackNum(animation_name, out var num);
            track = Skeleton.AnimationState.SetAnimation(num, animation_name, false);
            track.MixDuration = 0;
        }

        track.TimeScale = 0;
        track.Alpha = Mathf.Abs(direction_value);
    }

    /// <summary>
    /// 둘 다 제로에 근접해야지만 빠져나감
    /// 처음 드래그했을때는 Dragged_Canvas_Position이 0이 아닐거고
    /// 손을 땠을때는 Current_Face_Direction가 0이 아닐거기 때문에
    /// 손을 때고서 Current_Face_Direction가 0이 될때까지 자연스럽게 기다리기 위한 의도입니다
    /// = 손을 땠을때 부드럽게 정면보는 상태로 돌아가기 위한 의도
    /// </summary>
    /// <returns></returns>
    bool ShouldContinueMovingFace()
    {
        var cursor = Vector2.Distance(Dragged_Canvas_Position, Vector2.zero);
        var face_dir_distance = Vector2.Distance(Current_Face_Direction, Vector2.zero);

        bool result = cursor > 0.01f || face_dir_distance > 0.01f;

        return result;
    }

    /// <summary>
    /// 말풍선 뼈대 위치
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBalloonWorldPosition()
    {
        if (Balloon == null)
        {
            return new Vector3(-1.5f, 2.6f, 0);
        }

        return Balloon.GetWorldPosition(Skeleton.transform);
    }

    /// <summary>
    /// 슬롯 찾기
    /// </summary>
    /// <param name="slot_name"></param>
    /// <returns></returns>
    public Slot FindSlot(string slot_name)
    {
        return Skeleton.Skeleton.FindSlot(slot_name);
    }

    /// <summary>
    /// 트랙 찾기
    /// </summary>
    /// <param name="track">트랙 넘버</param>
    /// <returns>찾은 트랙엔트리</returns>
    public TrackEntry FindTrack(int track)
    {
        return Skeleton.AnimationState.GetCurrent(track);
    }

    /// <summary>
    /// 본(뼈대) 찾기
    /// </summary>
    /// <param name="bone_name">본 이름</param>
    /// <returns>찾은 본</returns>
    public Bone FindBone(string bone_name)
    {
        return Skeleton.Skeleton.FindBone(bone_name);
    }

    public void Pause(bool val)
    {
        Skeleton.AnimationState.TimeScale = val ? 0 : 1;
        if (val)
        {
            RemoveSkeletonEventListener();
            RemoveGestureEventListener();
        }
        else
        {
            AddSkeletonEventListener();
            AddGestureEventListener();
        }
    }

    /// <summary>
    /// 아무 반응이 없을때 자동으로 재생되는 애니메이션(Bored_Chatmotion)을 재생하기 위한 record..였는데
    /// 현재는 그런 애니메이션 데이터가 없어서 의미가 없는 상태입니다
    /// 나중에 아예 안쓰는게 확정되면 정리해주세요
    /// </summary>
    public record IdleAnimationData
    {
        public string Animation_Idle_Name;
        public int[] Bored_Chatmotion_Ids;
        public int Bored_Count;
    }

    public class ChatMotionNotFoundException : Exception
    {
        public int Chat_Motion_ID { get; private set; }

        public ChatMotionNotFoundException(int chat_motion_id)
            : base($"선택된 챗모션이 존재하지 않습니다, Chat Motion ID: {chat_motion_id}")
        {
            Chat_Motion_ID = chat_motion_id;
        }
    }

    public class InvalidTrackException : Exception
    {
        public int Animation_Track { get; private set; }

        public InvalidTrackException(int animation_track = -1)
            : base($"선택된 트랙이 올바르지 않습니다, Animation Track: {animation_track}")
        {
            Animation_Track = animation_track;
        }
    }
}

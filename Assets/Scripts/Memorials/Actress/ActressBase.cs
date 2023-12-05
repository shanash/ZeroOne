using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SkeletonUtility))]
public abstract class ActressBase : MonoBehaviour, IActressPositionProvider
{
    const int INTERACTION_TRACK = 0;
    const int FACE_HORIZON_TRACK = 10;
    const int FACE_VERTICAL_TRACK = 11;
    const string IDLE_NAME = "idle";
    const string FACE_BONE_NAME = "touch_center";
    const string BALLOON_BONE_NAME = "balloon";
    static readonly float FACE_MOVE_MAX_DISTANCE = (float)GameDefine.SCREEN_BASE_HEIGHT / 4;

    protected SkeletonAnimation Skeleton;
    protected Camera Main_Cam;
    protected MemorialManager Memorial_Mng;
    protected MEMORIAL_TYPE Memorial_Type = MEMORIAL_TYPE.NONE;
    private List<SpineBoundingBox> InputDowned_Components = new List<SpineBoundingBox>();

    // 참조할 본들
    Bone Face; // 얼굴 위치를 가져오기 위한 본
    Bone Balloon; // 말풍선 위치 본

    protected Dictionary<int, (string Base, string[] Subs)> State_Idle_Animation = null;
    protected int Current_State_Id
    {
        get
        {
            return _Current_State_Id;
        }

        set
        {
            if (_Current_State_Id != value)
            {
                // 기존 서브 애니메이션 종료
                if (Idle_Sub_Animations != null)
                {
                    foreach (var anim_name in Idle_Sub_Animations)
                    {
                        if (TryGetTrackNum(anim_name, out int track_index))
                        {
                            Skeleton.AnimationState.SetEmptyAnimation(track_index, 0);
                        }
                    }
                }
                var te = Skeleton.AnimationState.SetAnimation(INTERACTION_TRACK, State_Idle_Animation[value].Base, true);
                te.MixDuration = 0.2f;

                if (State_Idle_Animation[value].Subs != null)
                {
                    foreach (var anim_name in State_Idle_Animation[value].Subs)
                    {
                        if (TryGetTrackNum(anim_name, out int track_index))
                        {
                            te = Skeleton.AnimationState.SetAnimation(track_index, anim_name, true);
                            te.MixDuration = 0.0f;
                        }
                    }
                }
            }
            _Current_State_Id = value;
        }
    }
    int _Current_State_Id = 0;

    // 리액션 데이터들
    List<Me_Interaction_Data>[,] Touch_Type_Interactions = null; // 신체 타입별 반응 리스트
    int[] LastPlayedInteractionIndices = null;
    Dictionary<TOUCH_GESTURE_TYPE, (TOUCH_BODY_TYPE touch_body_type, int count)> Gesture_Touch_Counts = null;

    Dictionary<int, Me_Chat_Motion_Data> Chat_Motions = null; // 챗모션 아이디, 챗모션(애니메이션 + 대사들)
    Dictionary<int, Me_Serifu_Data> Serifues = null; // 대사 아이디, 대사

    // 얼굴 방향 움직임    
    [SerializeField, Tooltip("얼굴이 포인터를 따라가는 속도"), Range(1, 10)]
    float Look_At_Speed = 3.0f;
    Coroutine CoMoveFace;// 얼굴을 실제로 움직이는 실행중인 코루틴
    Vector2 Dragged_Canvas_Position = Vector2.zero;// 얼굴을 움직이기 위해 유저가 탭 중인 위치
    Vector2 Current_Face_Direction = Vector2.zero;// 얼굴 방향

    // 애니메이션 및 대사
    int Current_Chat_Motion_ID = -1; // 사용중인 챗모션 (애니메이션 + 대사들)
    int Current_Serifu_Index = -1; // 출력중인 대사 인덱스
    int Current_Balloon_ID = -1; // 표시되어 있는 말풍선 아이디

    // 입모양 애니메이션
    [SerializeField, Tooltip("말할때 입 벌어지는 크기 배율 조정"), Range(1, 10)]
    float Talk_Mouth_Wide_Multiple = 3.0f;
    TrackEntry _Playing_Mouth_Track_Entry = null; // 플레이중인 입모양 트랙엔트리
    string Current_Mouth_Anim_Name = string.Empty; // 이벤트로 받은 입모양 애니메이션 이름
    float Origin_Mouth_Alpha = 0; // 원래 입 크기
    float Dest_Mouth_Alpha = 0; // 움직이기 위한 목표로 하는 입 크기
    float Elapsed_Time_For_Mouth_Open = 0; //

    protected string Idle_Animation
    {
        get
        {
            if (State_Idle_Animation == null
                || State_Idle_Animation.Count == 0
                || !State_Idle_Animation.ContainsKey(Current_State_Id))
            {
                Debug.Assert(false, "기본 아이들 애니메이션이 설정되지 않았습니다");
                return null;
            }

            return State_Idle_Animation[Current_State_Id].Base;
        }
    }

    protected string[] Idle_Sub_Animations
    {
        get
        {
            if (State_Idle_Animation == null
    || State_Idle_Animation.Count == 0
    || !State_Idle_Animation.ContainsKey(Current_State_Id))
            {
                return null;
            }

            return State_Idle_Animation[Current_State_Id].Subs;
        }
    }

    Bone indicator = null;
    PointAttachment pt = null;

    #region Monobehaviour Methods 
    void Awake()
    {
        Skeleton = GetComponent<SkeletonAnimation>();
        Main_Cam = Camera.main;
        Face = FindBone(FACE_BONE_NAME);
        Debug.Assert(Face != null, $"얼굴 추적 기준 본이 없습니다 : {FACE_BONE_NAME}");
        Balloon = FindBone(BALLOON_BONE_NAME);
        Debug.Assert(Face != null, $"말풍선용 본이 없습니다 : {BALLOON_BONE_NAME}");
    }

    public void Init(int player_character_id, int state_id)
    {
        Touch_Type_Interactions = MasterDataManager.Instance.Get_MemorialInteraction(player_character_id);
        LastPlayedInteractionIndices = Enumerable.Repeat(-1, Touch_Type_Interactions.Length).ToArray(); // 배열 생성하면서 -1로 전부 초기화
        Gesture_Touch_Counts = new Dictionary<TOUCH_GESTURE_TYPE, (TOUCH_BODY_TYPE touch_body_type, int count)>();
        Chat_Motions = MasterDataManager.Instance.Get_MemorialChatMotion(player_character_id);
        Serifues = MasterDataManager.Instance.Get_MemorialSerifu(player_character_id);
        var audio_keys = GetMemorialAudioKeysFromSerifu(Serifues.Values.ToList());
        AudioManager.Instance.PreloadAudioClipsAsync(audio_keys, null);
        State_Idle_Animation = MasterDataManager.Instance.Get_MemorialStateAnimation(player_character_id);
        Current_State_Id = state_id;
    }

    void OnEnable()
    {
        GestureManager.Instance.OnGestureDetected += OnGestureDetect;
        InputCanvas.OnDrag += OnDrag;

        if (Skeleton != null)
        {
            Skeleton.AnimationState.Start += SpineAnimationStart;
            Skeleton.AnimationState.Complete += SpineAnimationComplete;
            Skeleton.AnimationState.Interrupt += SpineAnimationInterrupt;
            Skeleton.AnimationState.End += SpineAnimationEnd;
            Skeleton.AnimationState.Event += SpineAnimationEvent;
        }
    }

    void OnDisable()
    {
        if (Skeleton != null)
        {
            Skeleton.AnimationState.Start -= SpineAnimationStart;
            Skeleton.AnimationState.Complete -= SpineAnimationComplete;
            Skeleton.AnimationState.Interrupt += SpineAnimationInterrupt;
            Skeleton.AnimationState.End -= SpineAnimationEnd;
            Skeleton.AnimationState.Event -= SpineAnimationEvent;
        }

        GestureManager.Instance.OnGestureDetected -= OnGestureDetect;
        InputCanvas.OnDrag -= OnDrag;
    }

    void Update()
    {
        OnUpdate();

        Elapsed_Time_For_Mouth_Open += Time.deltaTime;
        if (_Playing_Mouth_Track_Entry != null)
        {
            _Playing_Mouth_Track_Entry.Alpha = Mathf.Lerp(Origin_Mouth_Alpha, Dest_Mouth_Alpha, Math.Min(1.0f, Elapsed_Time_For_Mouth_Open / AudioManager.VOICE_TERM_SECONDS));
        }
    }
    #endregion

    #region Spine Animation Callbacks
    protected virtual void SpineAnimationStart(TrackEntry entry)
    {
        Debug.Log($"SpineAnimationStart : {entry.Animation.Name}");
    }
    protected virtual void SpineAnimationInterrupt(TrackEntry entry)
    {
        Debug.Log($"SpineAnimationInterrupt : {entry.Animation.Name}");
    }
    protected virtual void SpineAnimationComplete(TrackEntry entry)
    {
        Debug.Log($"SpineAnimationComplete : {entry.Animation.Name}");
    }
    protected virtual void SpineAnimationEnd(TrackEntry entry)
    {
        if (entry.Animation.Name.Contains(IDLE_NAME))
        {
            return;
        }

        if (Current_Chat_Motion_ID != -1)
        {
            Current_Chat_Motion_ID = -1;
        }

        DisappearBalloon();
        Debug.Log($"SpineAnimationEnd : {entry.Animation.Name}");
    }
    protected virtual void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        Debug.Log($"SpineAnimationEvent : {evt.Data.Name} : {evt.String}");
        MemorialDefine.TryParseEvent(evt.Data.Name.ToUpper(), out MemorialDefine.SPINE_EVENT eEvt);
        switch (eEvt)
        {
            case MemorialDefine.SPINE_EVENT.MOUTH_OPEN:
                Current_Mouth_Anim_Name = evt.String;
                break;
            case MemorialDefine.SPINE_EVENT.MOUTH_CLOSE:
                Current_Mouth_Anim_Name = string.Empty;
                break;
            case MemorialDefine.SPINE_EVENT.VOICE:
                if (Current_Chat_Motion_ID == -1)
                {
                    Debug.Assert(false, $"출력할 대사가 없습니다. {Current_Chat_Motion_ID} :: {Current_Serifu_Index}");
                    return;
                }
                Current_Serifu_Index++;
                var serifu = GetSerifuData(Current_Chat_Motion_ID, Current_Serifu_Index);
                if (serifu == null)
                {
                    Debug.LogWarning($"출력할 대사가 없습니다. {Current_Chat_Motion_ID} :: {Current_Serifu_Index}");
                    //Debug.Assert(false, $"출력할 대사가 없습니다. {Current_Chat_Motion_ID} :: {Current_Serifu_Index}");
                    return;
                }

                DisappearBalloon();
                if (!string.IsNullOrEmpty(serifu.text_kr))
                {
                    SpeechBalloonManager.Instance.CreateBalloon(
                        (balloon_id) =>
                        {
                            Current_Balloon_ID = balloon_id;
                        },
                        serifu.text_kr,
                        this,
                        new Vector2(460, 170),
                        SpeechBalloon.BalloonSizeType.FixedWidth,
                        SpeechBalloon.Pivot.Right);
                }

                string sound_key = serifu.audio_clip_key;
                if (!string.IsNullOrEmpty(sound_key))
                {
                    AudioManager.Instance.PlayVoice(serifu.audio_clip_key, false, OnAudioStateAndVolumeChanged);
                }
                break;
            default:
                // 이벤트 이름으로 작업물에 사운드 파일 이름이 들어가므로 지정된 이벤트외의 다른 이름이 들어갈 수 있습니다.
                // 혹시 모르니 로그로 표시만 해줍시다
                Debug.Log($"지정된 이벤트 외의 호출 : {evt.Data.Name.ToUpper()} :: {evt.String}");
                // Debug.Assert(false, $"잘못된 이벤트가 들어가 있습니다. {evt.Data.Name.ToUpper()} :: {evt.String}");
                break;
        }
    }

    /// <summary>
    /// 말풍선 위치를 가져오기 위한 메소드
    /// 혹시 다른 본 위치도 외부에서 사용해야 된다면
    /// 조금 더 제네릭하게 수정해줍시다.
    /// </summary>
    /// <returns>해당 월드 위치</returns>
    public Vector3 GetBalloonWorldPosition()
    {
        return Balloon.GetWorldPosition(Skeleton.transform);
    }

    public void OnAudioStateAndVolumeChanged(string sound_key, AudioManager.AUDIO_STATES audio_state, float volume_rms)
    {
        switch (audio_state)
        {
            case AudioManager.AUDIO_STATES.START:
                if (TryGetTrackNum(Current_Mouth_Anim_Name, out int track_index))
                {
                    if (track_index == -1)
                    {
                        return;
                    }

                    var pre_played_te = Skeleton.AnimationState.GetCurrent(track_index);
                    if (pre_played_te != null)
                    {
                        pre_played_te.Alpha = 0;
                    }

                    _Playing_Mouth_Track_Entry = Skeleton.AnimationState.SetAnimation(track_index, Current_Mouth_Anim_Name, false);
                    _Playing_Mouth_Track_Entry.Alpha = 0;
                    _Playing_Mouth_Track_Entry.MixDuration = 0.2f;
                }
                Origin_Mouth_Alpha = 0;
                Dest_Mouth_Alpha = 0;
                Elapsed_Time_For_Mouth_Open = 0;
                break;
            case AudioManager.AUDIO_STATES.PLAYING:
                if (_Playing_Mouth_Track_Entry == null || !_Playing_Mouth_Track_Entry.Animation.Name.Equals(Current_Mouth_Anim_Name))
                {
                    if (Current_Mouth_Anim_Name.Equals(string.Empty))
                    {
                        return;
                    }

                    float before_alpha = 0;

                    if (_Playing_Mouth_Track_Entry != null)
                    {
                        before_alpha = _Playing_Mouth_Track_Entry.Alpha;
                    }

                    if (TryGetTrackNum(Current_Mouth_Anim_Name, out int index))
                    {
                        if (index == -1)
                        {
                            _Playing_Mouth_Track_Entry = null;
                            return;
                        }

                        var pre_played_te = Skeleton.AnimationState.GetCurrent(index);
                        if (pre_played_te != null)
                        {
                            pre_played_te.Alpha = 0;
                        }

                        _Playing_Mouth_Track_Entry = Skeleton.AnimationState.SetAnimation(index, Current_Mouth_Anim_Name, false);
                    }
                    _Playing_Mouth_Track_Entry.Alpha = before_alpha;
                    _Playing_Mouth_Track_Entry.MixDuration = 0.2f;
                }

                Elapsed_Time_For_Mouth_Open = 0;
                Origin_Mouth_Alpha = _Playing_Mouth_Track_Entry.Alpha;
                Dest_Mouth_Alpha = Mathf.Clamp(volume_rms * Talk_Mouth_Wide_Multiple, 0, 1);
                break;
            case AudioManager.AUDIO_STATES.END:
                DisappearBalloon();
                if (_Playing_Mouth_Track_Entry == null)
                {
                    return;
                }
                _Playing_Mouth_Track_Entry.Alpha = 0;
                _Playing_Mouth_Track_Entry = null;
                break;
            default:
                break;
        }
    }
    #endregion

    public void SetMemorialType(MEMORIAL_TYPE type)
    {
        Memorial_Type = type;
    }

    /// <summary>
    /// Attachment 찾기
    /// </summary>
    /// <param name="slot_name"></param>
    /// <param name="attachment_name"></param>
    /// <returns></returns>
    public Attachment FindAttachment(string slot_name, string attachment_name)
    {
        if (Skeleton != null)
        {
            var slot = Skeleton.Skeleton.FindSlot(slot_name);
            if (slot != null)
            {
                return Skeleton.Skeleton.GetAttachment(slot_name, attachment_name);
            }
        }
        return null;
    }

    /// <summary>
    /// 슬롯 찾기
    /// </summary>
    /// <param name="slot_name"></param>
    /// <returns></returns>
    public Slot FindSlot(string slot_name)
    {
        if (Skeleton != null)
        {
            return Skeleton.skeleton.FindSlot(slot_name);
        }
        return null;
    }

    /// <summary>
    /// 지정 트랙의 (지정)애니메이션이 플레이 중인지 여부 반환
    /// 하지만 아이들 애니메이션은 상시 돌아갈 것이기 때문에 예외로 처리한다
    /// </summary>
    /// <param name="track">트랙번호</param>
    /// <param name="anim_name">지정할 애니메이션 이름</param>
    /// <returns></returns>
    protected bool IsAnimationRunning(int track, string anim_name = default)
    {
        var entry = FindTrack(track);
        if (entry == null) return false;

        if (entry.Animation.Name.Equals(anim_name) || (anim_name == default && !entry.Animation.Name.Equals(Idle_Animation)))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 입력한 애니메이션의 지정트랙을 가져온다
    /// 규격외의 이름이 입력된 경우엔 -1을 돌려준다
    /// </summary>
    /// <param name="animation_name">애니메이션 이름</param>
    /// <returns>트랙 넘버</returns>
    protected bool TryGetTrackNum(string animation_name, out int track_index)
    {
        string[] word = animation_name.Split('_');
        bool result = int.TryParse(word[0], out int num);
        track_index = result ? num : -1;
        return result;
    }

    /// <summary>
    /// 지정 트랙에 플래이 중인 애니메이션 이름 반환
    /// </summary>
    /// <param name="track"></param>
    /// <returns></returns>
    protected string GetRunningAnimationName(int track)
    {
        var entry = FindTrack(track);
        if (entry == null) return string.Empty;
        return entry.Animation.Name;
    }

    /// <summary>
    /// 트랙 찾기
    /// </summary>
    /// <param name="track">트랙 넘버</param>
    /// <returns>찾은 트랙엔트리</returns>
    protected TrackEntry FindTrack(int track)
    {
        if (Skeleton != null)
        {
            return Skeleton.AnimationState.GetCurrent(track);
        }
        return null;
    }

    /// <summary>
    /// 본(뼈대) 찾기
    /// </summary>
    /// <param name="bone_name">본 이름</param>
    /// <returns>찾은 본</returns>
    protected Bone FindBone(string bone_name)
    {
        if (Skeleton != null)
        {
            return Skeleton.Skeleton.FindBone(bone_name);
        }
        return null;
    }

    protected virtual void OnUpdate() { }

    private void OnGestureDetect(TOUCH_GESTURE_TYPE type, ICursorInteractable component, Vector2 vector, int drag_id)
    {
        var bounding_box = component as SpineBoundingBox;
        if (bounding_box == null)
        {
            return;
        }

        switch (type)
        {
            case TOUCH_GESTURE_TYPE.DOWN:
                InputDowned_Components.Add(bounding_box);
                break;
            case TOUCH_GESTURE_TYPE.UP:
                InputDowned_Components.Clear();
                break;
        }

        //일단 DOWN과 UP은 사용하지 않습니다
        if (type == TOUCH_GESTURE_TYPE.DOWN || type == TOUCH_GESTURE_TYPE.UP)
        {
            return;
        }

        // 현재 애니메이션이 진행중이면 이 이후로는 클릭해도 반응하지 않는다
        if (IsAnimationRunning(INTERACTION_TRACK))
        {
            return;
        }

        HandleGesture(bounding_box, type, vector, drag_id);
    }

    /// <summary>
    /// 제스쳐에 따라 어떤 반응을 플레이할지를 결정합니다
    /// </summary>
    /// <param name="bounding_box"></param>
    /// <param name="gesture_type"></param>
    protected virtual void HandleGesture(SpineBoundingBox bounding_box, TOUCH_GESTURE_TYPE gesture_type, Vector2 pos, int drag_id)
    {
        int body_type_id = (int)(bounding_box.GetTouchBodyType());
        int gesture_type_id = (int)gesture_type;
        int chat_motion_id = 0;

        try
        {
            if (Touch_Type_Interactions[body_type_id, gesture_type_id] == null || Touch_Type_Interactions[body_type_id, gesture_type_id].Count == 0)
            {
                return;
            }

            int index = GetInteractionIndex(
                bounding_box,
                Touch_Type_Interactions[body_type_id, gesture_type_id],
                gesture_type,
                pos,
                drag_id);

            if (index == -1)
            {
                return;
            }

            if (Touch_Type_Interactions[body_type_id, gesture_type_id][index].change_state_id > 0)
            {
                Current_State_Id = Touch_Type_Interactions[body_type_id, gesture_type_id][index].change_state_id;
                Debug.Log($"Current_State_Id : {Current_State_Id}");
            }

            if (Touch_Type_Interactions[body_type_id, gesture_type_id][index].chat_motion_ids == null || Touch_Type_Interactions[body_type_id, gesture_type_id][index].chat_motion_ids.Length == 0)
            {
                return;
            }

            int[] available_indexes = Touch_Type_Interactions[body_type_id, gesture_type_id][index].chat_motion_ids;

            int last_available_index = Array.IndexOf(available_indexes, LastPlayedInteractionIndices[body_type_id]);

            int select_index = last_available_index + 1;

            if (available_indexes.Length <= select_index)
            {
                select_index = 0;
            }

            // 조건따라 골라줍니다
            chat_motion_id = available_indexes[select_index];


            if (chat_motion_id == -1)
            {
                // 재생할 조건이 없으니 중단
                return;
            }
            LastPlayedInteractionIndices[body_type_id] = chat_motion_id;

            PlayAnimationForChatMotion(chat_motion_id, (gesture_type == TOUCH_GESTURE_TYPE.DRAG) ? 0.0f : 0.2f);

            TOUCH_BODY_TYPE body_type = bounding_box.GetTouchBodyType();
            if (!Gesture_Touch_Counts.ContainsKey(gesture_type))
            {
                Gesture_Touch_Counts.Add(gesture_type, (body_type, 0));
            }

            int before_count = 0;
            if (Gesture_Touch_Counts[gesture_type].touch_body_type == body_type)
            {
                before_count = Gesture_Touch_Counts[gesture_type].count;
            }

            Gesture_Touch_Counts[gesture_type] = (body_type, before_count + 1);
        }
        catch (Exception e)
        {
            Debug.Assert(false, $"body_type_id : {body_type_id}, chat_motion_id : {chat_motion_id}");
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 인터렉션 조건에 맞는 인덱스들을 뽑아줍니다
    /// 따로 만들고 싶은 조건이 있다면 상속받아 구현합니다.
    /// 구현이 없으면 그냥 랜덤으로..
    /// </summary>
    /// <param name="interaction_datas"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    protected virtual int GetInteractionIndex(SpineBoundingBox bounding_box, List<Me_Interaction_Data> interaction_datas, TOUCH_GESTURE_TYPE gesture_type, Vector2 screen_pos, int drag_id)
    {
        TOUCH_BODY_TYPE type = bounding_box.GetTouchBodyType();
        TOUCH_BODY_DIRECTION dir = bounding_box.GetTouchBodyDirection();
        //List<int> available_indexes = new List<int>();

        int result_index = -1;

        int cnt = interaction_datas.Count;
        for (int i = 0; i < cnt; ++i)
        {
            if (interaction_datas[i].condition_state_ids != null && interaction_datas[i].condition_state_ids.Length > 0)
            {
                if (!Array.Exists(interaction_datas[i].condition_state_ids, element => element == Current_State_Id))
                {
                    continue;
                }
            }

            if (interaction_datas[i].touch_gesture_type != gesture_type)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(interaction_datas[i].touch_body_direction))
            {
                if (!dir.ToString().Equals(interaction_datas[i].touch_body_direction))
                {
                    continue;
                }
            }

            if (interaction_datas[i].touch_condition_inequality == INEQUALITY_TYPE.NONE)
            {
                result_index = i;
            }
            else
            {
                int count = 1;
                if (Gesture_Touch_Counts.ContainsKey(gesture_type))
                {
                    if (Gesture_Touch_Counts[gesture_type].touch_body_type == type)
                    {
                        count += Gesture_Touch_Counts[gesture_type].count;
                    }
                }

                switch (interaction_datas[i].touch_condition_inequality)
                {
                    case INEQUALITY_TYPE.EQUAL:
                        if (count == interaction_datas[i].touch_condition_count)
                        {
                            result_index = i;
                        }
                        break;
                    case INEQUALITY_TYPE.NOT_EQUAL:
                        if (count != interaction_datas[i].touch_condition_count)
                        {
                            result_index = i;
                        }
                        break;
                    case INEQUALITY_TYPE.GREATER:
                        if (count > interaction_datas[i].touch_condition_count)
                        {
                            result_index = i;
                        }
                        break;
                    case INEQUALITY_TYPE.GREATER_EQUAL:
                        if (count >= interaction_datas[i].touch_condition_count)
                        {
                            result_index = i;
                        }
                        break;
                    case INEQUALITY_TYPE.LESS:
                        if (count < interaction_datas[i].touch_condition_count)
                        {
                            result_index = i;
                        }
                        break;
                    case INEQUALITY_TYPE.LESS_EQUAL:
                        if (count <= interaction_datas[i].touch_condition_count)
                        {
                            result_index = i;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (result_index != -1)
            {
                break;
            }
        }

        return result_index;
    }

    protected virtual void OnDrag(InputCanvas.InputActionPhase phase, Vector2 delta, Vector2 position)
    {
        if (InputDowned_Components.Count == 0)
        {
            Debug.Assert(false, "OnClickDown이 호출되지 않았습니다");
            return;
        }

        // 얼굴 방향이 변경되는 건 OnInputDown때 배경터치일때입니다
        if (InputDowned_Components.Count == 1
            && InputDowned_Components[0] is SpineBoundingBox)
        {
            if (InputDowned_Components[0].GetTouchBodyType() == TOUCH_BODY_TYPE.NONE)
            {
                switch (phase)
                {
                    case InputCanvas.InputActionPhase.Started:
                        Dragged_Canvas_Position = position;
                        if (CoMoveFace == null)
                        {
                            CoMoveFace = StartCoroutine(CoMoveFaceToDirection("10_Right", "11_Up", "10_Left", "11_Down"));
                        }
                        break;
                    case InputCanvas.InputActionPhase.Performed:
                        Dragged_Canvas_Position = position;
                        break;
                    case InputCanvas.InputActionPhase.Canceled:
                        Dragged_Canvas_Position = Vector2.zero;
                        break;
                }
            }
            else if (InputDowned_Components[0].GetTouchBodyType() == TOUCH_BODY_TYPE.HEAD)
            {
                switch (phase)
                {
                    case InputCanvas.InputActionPhase.Started:
                        Dragged_Canvas_Position = position;
                        if (CoMoveFace == null)
                        {
                            var te_in = Skeleton.AnimationState.SetAnimation(30, "30_nade_in", false);
                            te_in.MixDuration = 0.2f;
                            Skeleton.AnimationState.AddAnimation(30, "30_nade_idle", true, 0);
                            CoMoveFace = StartCoroutine(CoMoveFaceToDirection("31_nade_Right", "32_nade_Up", "31_nade_Left", "32_nade_Down"));
                        }
                        break;
                    case InputCanvas.InputActionPhase.Performed:
                        Dragged_Canvas_Position = position;
                        break;
                    case InputCanvas.InputActionPhase.Canceled:
                        var te_out = Skeleton.AnimationState.SetAnimation(30, "30_nade_out", false);
                        te_out.MixDuration = 0.2f;
                        Dragged_Canvas_Position = Vector2.zero;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 말풍선 및 음성 플레이를 위한 애니메이션을 플레이
    /// </summary>
    /// <param name="chat_motion_id">챗모션 ID</param>
    protected void PlayAnimationForChatMotion(int chat_motion_id, float mixDuration = 0.2f)
    {
        Debug.Log($"PlayAnimationForChatMotion : {chat_motion_id}");
        if (!Chat_Motions.ContainsKey(chat_motion_id))
        {
            throw new ChatMotionNotFoundException(chat_motion_id);
        }

        var chat_motion_data = Chat_Motions[chat_motion_id];

        foreach (string anim_name in chat_motion_data.animation_name)
        {
            // 인터렉션 트랙은 0번으로 고정되어 있다
            if (!TryGetTrackNum(anim_name, out int track_num))
            {
                throw new InvalidTrackException(track_num);
            }

            var te = Skeleton.AnimationState.SetAnimation(track_num, anim_name, false);
            te.MixDuration = mixDuration;

            if (track_num == INTERACTION_TRACK)
            {
                // 반응애니메이션 끝나고 아이들로 돌아가는 부분은 연결동작으로 맞춰져 있기 때문에
                // 크로스페이드 블랜딩을 쓰면 더 부자연스러워 보인다
                te = Skeleton.AnimationState.AddAnimation(track_num, Idle_Animation, true, 0);
                te.MixDuration = 0.0f;
            }
            else
            {
                Skeleton.AnimationState.AddEmptyAnimation(track_num, 0.2f, 0);
            }
        }

        Current_Chat_Motion_ID = chat_motion_id;
        Current_Serifu_Index = -1;
    }

    public void PlayIdleAnimation()
    {
        var te = Skeleton.AnimationState.SetAnimation(0, Idle_Animation, true);
        te.MixDuration = 0.2f;

        if (Idle_Sub_Animations != null)
        {
            foreach (var anim_name in Idle_Sub_Animations)
            {
                if (TryGetTrackNum(anim_name, out int track_num))
                {
                    Skeleton.AnimationState.SetAnimation(track_num, anim_name, true);
                }
            }
        }
    }

    bool TryGetTrackCheckEqual(out int track_index, params string[] anim_names)
    {
        track_index = -1;
        foreach (var name in anim_names)
        {
            if (!TryGetTrackNum(name, out int index))
            {
                return false;
            }

            if (track_index != -1 && track_index != index)
            {
                return false;
            }

            track_index = index;
        }

        return true;
    }

    /// <summary>
    /// 유저가 화면에 드래그하면 캐릭터 얼굴이 부드럽게 드래그포인트를 향하기 위해서 매 프레임 실행되는 코루틴
    /// 손을 떼도 캐릭터가 다시 정면을 볼때까지 실행이 끝나지 않는다
    /// </summary>
    /// <returns></returns>
    IEnumerator CoMoveFaceToDirection(string right, string up, string left, string down)
    {
        if (!TryGetTrackCheckEqual(out int horizon_track_index, right, left)
            || !TryGetTrackCheckEqual(out int vertical_track_index, up, down))
        { 
            yield break;
        }

        TrackEntry track1 = FindTrack(horizon_track_index);
        TrackEntry track2 = FindTrack(vertical_track_index);

        while (ShouldContinueMovingFace())
        {
            UpdateFaceAnimationDirection(ref track1, ref track2, right, up, left, down);
            yield return null;
        }

        Skeleton.AnimationState.ClearTrack(FACE_HORIZON_TRACK);
        Skeleton.AnimationState.ClearTrack(FACE_VERTICAL_TRACK);
        CoMoveFace = null;
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
        return Vector2.Distance(Dragged_Canvas_Position, Vector2.zero) > 0.01f || Vector2.Distance(Current_Face_Direction, Vector2.zero) > 0.01f;
    }

    /// <summary>
    /// 현재 프레임의 얼굴 방향 업데이트
    /// </summary>
    void UpdateFaceAnimationDirection(ref TrackEntry track1, ref TrackEntry track2, params string[] anim_names)
    {
        Vector3 face_world_pos = Face.GetWorldPosition(Skeleton.transform);
        Vector2 face_screen_pos = Main_Cam.WorldToScreenPoint(face_world_pos);
        Vector2 dest_direction = CalculateDesiredDirection(face_screen_pos);

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
    Vector2 CalculateDesiredDirection(Vector2 face_screen_pos)
    {
        Vector2 dest_direction = Vector2.zero;

        if (!Dragged_Canvas_Position.Equals(Vector2.zero))
        {
            // Dragged_Canvas_Position - face_screen_pos = 얼굴에서 드래그중인 포인트까지의 벡터
            // 벡터 방향은 살린채 ClampMagnitude로 최대값을 FACE_MOVE_MAX_DISTANCE로 제한합니다
            dest_direction = Vector2.ClampMagnitude(Dragged_Canvas_Position - face_screen_pos, FACE_MOVE_MAX_DISTANCE);
            // dest_direction에 Mathf.Sqrt(2.0f) / FACE_MOVE_MAX_DISTANCE를 곱해서
            // 자연스럽게 축소시키고, 45도 각도의 벡터 최대값이 (1,1)이 되도록 맞춥니다
            dest_direction *= Mathf.Sqrt(2.0f) / FACE_MOVE_MAX_DISTANCE;

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
    /// 해당 캐릭터의 대사 데이터를 가져오기
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Me_Serifu_Data GetSerifuData(int chat_motion_id, int serifu_ids_index)
    {
        if (Serifues == null)
        {
            Debug.Assert(false, "정상적으로 초기화가 되지 않았습니다.");
            return null;
        }
        Debug.Assert(Serifues.Count > 0 && chat_motion_id > -1, $"없는 대사를 불러오려합니다. 챗모션ID : {chat_motion_id}, 대사 인덱스 : {serifu_ids_index}");
        if (Serifues.Count == 0 || chat_motion_id == -1 || Chat_Motions[chat_motion_id].serifu_ids == null)
            return null;

        if (Chat_Motions[chat_motion_id].serifu_ids.Count() <= serifu_ids_index)
            return null;

        return Serifues[Chat_Motions[chat_motion_id].serifu_ids[serifu_ids_index]];
    }

    /// <summary>
    /// 현재 표시되는 말풍선을 삭제
    /// </summary>
    void DisappearBalloon()
    {
        SpeechBalloonManager.Instance.DisappearBalloon(Current_Balloon_ID);
        Current_Balloon_ID = -1;
    }

    /// <summary>
    /// 캐릭터 대사 데이터에서 사용되는 오디오클립 키들을 가져오기
    /// </summary>
    /// <param name="serifu_list"></param>
    /// <returns></returns>
    List<string> GetMemorialAudioKeysFromSerifu(List<Me_Serifu_Data> serifu_list)
    {
        List<string> audio_keys = new List<string>();
        foreach (var serifu in serifu_list)
        {
            if (!string.IsNullOrEmpty(serifu.audio_clip_key))
            {
                audio_keys.Add(serifu.audio_clip_key);
            }
        }

        return audio_keys;
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

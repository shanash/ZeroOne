using FluffyDuck.Util;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FluffyDuck.Memorial
{
    public abstract partial class ActorBase : MonoBehaviour, IActorPositionProvider
    {
        protected static readonly float FACE_MOVE_MAX_DISTANCE = (float)GameDefine.SCREEN_BASE_HEIGHT / 4;
        protected const int IDLE_BASE_TRACK = 0;
        protected const int MOUTH_TRACK = 17;
        const string FACE_BONE_NAME = "touch_center";
        const string BALLOON_BONE_NAME = "balloon";

        [SerializeField]
        protected SkeletonAnimation Skeleton;

        // 입모양 애니메이션
        [SerializeField, Tooltip("말할때 입 벌어지는 크기 배율 조정"), Range(1, 10)]
        float Talk_Mouth_Wide_Multiple = 10.0f;

        // 얼굴 방향 움직임    
        [SerializeField, Tooltip("얼굴이 포인터를 따라가는 속도"), Range(1, 10)]
        float Look_At_Speed = 3.0f;
        Vector2 Dragged_Canvas_Position;
        Vector2 Current_Face_Direction;// 얼굴 방향

        ////////////////////////// 상태
        protected Dictionary<int, IdleAnimationData> Idle_Animation_Datas;
        protected int Current_State_Id;
        private KeyValuePair<(TOUCH_GESTURE_TYPE geture_type, TOUCH_BODY_TYPE body_type), int> Gesture_Touch_Counts;
        protected int Idle_Played_Count;

        protected Bone Face; // 얼굴 위치를 가져오기 위한 본
        protected Bone Balloon; // 말풍선 위치 본

        Producer Producer;
        List<Me_Interaction_Data>[,] Touch_Type_Interactions;
        Me_Interaction_Data Current_Interaction;

        protected Dictionary<int, Me_Chat_Motion_Data> Chat_Motions;
        protected int Current_Chat_Motion_Id;
        protected int Current_Serifu_Index;
        protected int Current_Balloon_ID = -1; // 표시되어 있는 말풍선 아이디

        Dictionary<int, Me_Serifu_Data> Serifues;
        string Current_Mouth_Anim_Name;

        TrackEntry Drag_Track_Entry;

        float Nade_Point = 0;

        float Origin_Mouth_Alpha = 0; // 원래 입 크기
        float Dest_Mouth_Alpha = 0; // 움직이기 위한 목표로 하는 입 크기
        float Elapsed_Time_For_Mouth_Open = 0;

        // 상태 ID에 기반한 상태 데이터를 가져옵니다
        protected IdleAnimationData Current_State_Data
        {
            get
            {
                if (Idle_Animation_Datas.Count == 0
                    || !Idle_Animation_Datas.ContainsKey(Current_State_Id))
                {
                    Debug.Assert(false, $"기본 아이들 애니메이션이 설정되지 않았습니다. State_Animation_Datas.Count: {Idle_Animation_Datas.Count}, State_Animation_Datas.ContainsKey(Current_State_Id): {Idle_Animation_Datas.ContainsKey(Current_State_Id)}");
                    return null;
                }

                return Idle_Animation_Datas[Current_State_Id];
            }
        }

        void OnEnable()
        {
            AddGestureEventListener();
            AddSkeletonEventListener();
        }

        void OnDisable()
        {
            RemoveGestureEventListener();
            RemoveSkeletonEventListener();
        }

        void Update()
        {
            FSM.UpdateState();
        }

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

            if (FSM.CurrentTransitionID == ACTOR_STATES.IDLE)
            {
                if (Current_State_Data.Bored_Count > 0)
                {
                    Idle_Played_Count++;
                    if (Idle_Played_Count % Current_State_Data.Bored_Count == 0)
                    {
                        int index = Idle_Played_Count / Current_State_Data.Bored_Count - 1;
                        Current_Chat_Motion_Id = Current_State_Data.Bored_Chatmotion_Ids[index];
                        if (Current_State_Data.Bored_Chatmotion_Ids.Length - 1 == index)
                        {
                            Idle_Played_Count = 0;
                        }

                        FSM.ChangeState(ACTOR_STATES.REACT);
                    }
                }
            }
            else if (FSM.CurrentTransitionID == ACTOR_STATES.REACT)
            {
                if (react_track_entries.Contains(entry))
                {
                    if (entry.TrackIndex != IDLE_BASE_TRACK)
                    {
                        Skeleton.AnimationState.SetEmptyAnimation(entry.TrackIndex, 0);
                    }
                    else
                    {
                        Skeleton.AnimationState.SetAnimation(IDLE_BASE_TRACK, Current_State_Data.Animation_Idle_Name, true);
                    }

                    react_track_entries.Remove(entry);
                }

                if (react_track_entries.Count == 0)
                {
                    FSM.ChangeState(ACTOR_STATES.IDLE);
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
            MemorialDefine.TryParseEvent(evt.Data.Name.ToUpper(), out MemorialDefine.SPINE_EVENT eEvt);
            switch (eEvt)
            {
                case MemorialDefine.SPINE_EVENT.MOUTH_SHAPE:
                    Current_Mouth_Anim_Name = evt.String.Equals("close") ? string.Empty : evt.String;
                    break;
                case MemorialDefine.SPINE_EVENT.VOICE:
                    if (Current_Chat_Motion_Id == -1)
                    {
                        Debug.Assert(false, $"출력할 대사가 없습니다. {Current_Chat_Motion_Id} :: {Current_Serifu_Index}");
                        return;
                    }

                    Current_Serifu_Index++;
                    var serifu = GetSerifuData(Chat_Motions[Current_Chat_Motion_Id], Current_Serifu_Index);
                    if (serifu == null)
                    {
                        Debug.LogWarning($"출력할 대사가 없습니다. {Current_Chat_Motion_Id} :: {Current_Serifu_Index}");
                        //Debug.Assert(false, $"출력할 대사가 없습니다. {Current_Chat_Motion_ID} :: {Current_Serifu_Index}");
                        return;
                    }

                    if (!string.IsNullOrEmpty(serifu.text_kr))
                    {
                        DisappearBalloon();

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
            }
        }

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
                    DisappearBalloon();
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

        protected virtual void OnTap(ICursorInteractable comp)
        {
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

            Current_Interaction = GetInteractionData(TOUCH_GESTURE_TYPE.TOUCH, bounding_box);
            if (Current_Interaction == null)
            {
                return;
            }

            SetReactionData(Current_Interaction, TOUCH_GESTURE_TYPE.TOUCH, bounding_box);

            FSM.ChangeState(ACTOR_STATES.REACT);
        }

        protected virtual void OnDoubleTap(ICursorInteractable comp)
        {
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

            Current_Interaction = GetInteractionData(TOUCH_GESTURE_TYPE.DOUBLE_TOUCH, bounding_box);
            if (Current_Interaction == null)
            {
                return;
            }

            SetReactionData(Current_Interaction, TOUCH_GESTURE_TYPE.DOUBLE_TOUCH, bounding_box);

            FSM.ChangeState(ACTOR_STATES.REACT);
        }

        protected virtual void OnDrag(ICursorInteractable comp, Vector2 position, Vector2 drag_vector, int state)
        {
            bool isDragValid = (FSM.CurrentTransitionID == ACTOR_STATES.DRAG || FSM.CurrentTransitionID == ACTOR_STATES.EYE_TRACKER) && state >= 1;
            bool isIdleValid = FSM.CurrentTransitionID == ACTOR_STATES.IDLE && state == 0;
            if (!isDragValid && !isIdleValid)
            {
                return;
            }

            Vector2 drag_dest;

            switch (comp)
            {
                case SpineBoundingBox bounding_box:
                    var data = GetInteractionData(TOUCH_GESTURE_TYPE.DRAG, bounding_box);

                    if (data == null)
                    {
                        return;
                    }

                    drag_dest = bounding_box.GetPtDirection();

                    if (drag_dest.Equals(Vector2.zero))
                    {
                        Debug.Assert(false, "ActorBase::OnDrag : 해당 터치에는 포인트가 없어서 드래그할 수 없습니다");
                        return;
                    }

                    if (drag_vector.sqrMagnitude.Equals(0.0f))
                    {
                        drag_vector = drag_dest * 0.001f;
                    }

                    if (!string.IsNullOrEmpty(data.drag_animation_name))
                    {
                        switch (state)
                        {
                            case 0:
                                Current_Interaction = data;
                                SetReactionData(Current_Interaction, TOUCH_GESTURE_TYPE.DRAG, bounding_box);

                                FSM.ChangeState(ACTOR_STATES.DRAG);
                                break;
                            case 1:
                                float close_value = // 드래그한 정도가 얼만큼 drag_dest와 일치했는가 (0~1 이외의 값은 크게 의미 없다)
                                    (drag_vector.sqrMagnitude / drag_dest.sqrMagnitude) // 드래그한 길이가 얼마나 대상과 비슷한가
                                    * CommonUtils.Math.Cos(drag_dest, drag_vector); // 드래그한 방향이 얼마나 대상과 비슷한가

                                if (Drag_Track_Entry != null)
                                {
                                    if (close_value < 1)
                                    {
                                        Drag_Track_Entry.TrackTime = close_value;
                                    }
                                    else
                                    {
                                        FSM.ChangeState(ACTOR_STATES.REACT);
                                    }
                                }
                                break;
                            case 2:
                                FSM.ChangeState(ACTOR_STATES.IDLE);
                                break;
                        }
                    }
                    break;
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
            bool isNadeValid = FSM.CurrentTransitionID == ACTOR_STATES.NADE && state >= 1;
            bool isIdleValid = FSM.CurrentTransitionID == ACTOR_STATES.IDLE && state == 0;

            if (!isIdleValid && !isNadeValid)
            {
                return;
            }

            var bounding_box = obj as SpineBoundingBox;
            if (bounding_box == null)
            {
                return;
            }

            var data = GetInteractionData(TOUCH_GESTURE_TYPE.NADE, bounding_box);

            if (data == null)
            {
                return;
            }

            Dragged_Canvas_Position = position;

            switch (state)
            {
                case 0:
                    Current_Interaction = data;
                    SetReactionData(Current_Interaction, TOUCH_GESTURE_TYPE.NADE, bounding_box);
                    Nade_Point = 0;

                    FSM.ChangeState(ACTOR_STATES.NADE);
                    break;
                case 1:
                    Nade_Point += delta.sqrMagnitude * 0.00001f;
                    if (Nade_Point > 10)
                    {
                        FSM.ChangeState(ACTOR_STATES.REACT);
                    }
                    break;
                case 2:
                    Dragged_Canvas_Position = Vector2.zero;
                    break;
            }
        }

        int GetConsecutiveGestureCount(TOUCH_GESTURE_TYPE gesture_type, TOUCH_BODY_TYPE body_type)
        {
            var key = (gesture_type, body_type);
            if (!Gesture_Touch_Counts.Key.Equals(key))
            {
                return 0;
            }

            return Gesture_Touch_Counts.Value;
        }

        Me_Interaction_Data GetInteractionData(TOUCH_GESTURE_TYPE type, SpineBoundingBox bounding_box)
        {
            var interaction_datas = Touch_Type_Interactions[(int)type, (int)bounding_box.GetTouchBodyType()];

            if (interaction_datas == null || interaction_datas.Count == 0)
            {
                return null;
            }

            int count = 1 + GetConsecutiveGestureCount(type, bounding_box.GetTouchBodyType());

            try
            {
                return interaction_datas
                    .Where(data => (data.condition_state_ids == null || data.condition_state_ids.Length == 0 || data.condition_state_ids.Contains(Current_State_Id)))
                    .Where(data => string.IsNullOrEmpty(data.touch_body_direction) || bounding_box.GetTouchBodyDirection().ToString().Equals(data.touch_body_direction))
                    .Where(data => (data.condition_max_gesture_count == 0 && data.condition_min_gesture_count == 0) // min max 둘다 0이면 통과
                    || ((data.condition_min_gesture_count == 0 || data.condition_min_gesture_count <= count) // condition_min_gesture_count 0이면 조건이 없는걸로 취급
                    && (data.condition_max_gesture_count == 0 || count <= data.condition_max_gesture_count))) // condition_max_gesture_count 0이면 조건이 없는걸로 취급
                    .First();
            }
            catch (InvalidOperationException)
            {
                // 조건들을 통과 못하는 경우는 그냥 패스
            }
            catch (Exception e)
            {
                // 이외의 예외는 확인해봅니다
                Debug.LogError($"Current_State_Id : {Current_State_Id}");
                Debug.LogException(e);
            }
            return null;
        }

        private void SetReactionData(Me_Interaction_Data data, TOUCH_GESTURE_TYPE type, SpineBoundingBox bounding_box)
        {
            if (data == null)
            {
                return;
            }

            int index = GetConsecutiveGestureCount(type, bounding_box.GetTouchBodyType()) % data.chat_motion_ids.Length;

            Current_Interaction = data;
            Current_Chat_Motion_Id = data.chat_motion_ids[index];
        }

        protected void AddGestureEventListener()
        {
            GestureManager.Instance.OnNade += OnNade;
            GestureManager.Instance.OnTap += OnTap;
            GestureManager.Instance.OnDoubleTap += OnDoubleTap;
            GestureManager.Instance.OnDrag += OnDrag;
        }

        protected void RemoveGestureEventListener()
        {
            GestureManager.Instance.OnNade -= OnNade;
            GestureManager.Instance.OnTap -= OnTap;
            GestureManager.Instance.OnDoubleTap -= OnDoubleTap;
            GestureManager.Instance.OnDrag -= OnDrag;
        }

        bool Initialize(Producer pd, Me_Resource_Data me_data)
        {
            if (Skeleton == null)
            {
                Debug.Assert(false, $"프리팹 {this.GetType()} 컴포넌트의 인스펙터에 SkeletonAnimation이 없습니다");
                return false;
            }

            InitField();

            if (!GetSkeletonComponents())
            {
                return false;
            }

            Producer = pd;
            Touch_Type_Interactions = MasterDataManager.Instance.Get_MemorialInteraction(me_data.player_character_id);
            Chat_Motions = MasterDataManager.Instance.Get_MemorialChatMotion(me_data.player_character_id);
            Serifues = MasterDataManager.Instance.Get_MemorialSerifu(me_data.player_character_id);

            List<string> audio_keys = Serifues.Values
                .Where(serifu => !string.IsNullOrEmpty(serifu.audio_clip_key))
                .Select(serifu => serifu.audio_clip_key)
                .ToList();

            AudioManager.Instance.PreloadAudioClipsAsync(audio_keys, null);

            var state_animation_data = MasterDataManager.Instance.Get_MemorialStateAnimation(me_data.player_character_id);
            Idle_Animation_Datas = state_animation_data.ToDictionary(
                    data => data.Key,
                    data => new IdleAnimationData
                    {
                        Animation_Idle_Name = data.Value.Idle_Animation_Name,
                        Bored_Chatmotion_Ids = data.Value.Bored_Chatmotion_Ids,
                        Bored_Count = data.Value.Bored_Condition_Count
                    });
            Current_State_Id = me_data.state_id;

            Gesture_Touch_Counts = default;

            Lazy_Init(ACTOR_STATES.IDLE);

            return true;
        }

        void InitField()
        {
            Idle_Animation_Datas = null;
            Current_State_Id = -1;
            Idle_Played_Count = 0;
            Gesture_Touch_Counts = default;
            Face = null;
            Balloon = null;
            Producer = null;
            Touch_Type_Interactions = null;
            Current_Interaction = null;
            Chat_Motions = null;
            Serifues = null;
            Drag_Track_Entry = null;
            Current_Chat_Motion_Id = -1;
            Current_Serifu_Index = -1;
            Nade_Point = 0;
            Current_Mouth_Anim_Name = string.Empty;
            Dragged_Canvas_Position = Vector2.zero;
            Current_Face_Direction = Vector2.zero;
        }

        bool GetSkeletonComponents()
        {
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

            return Face != null && Balloon != null;
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

        public Vector3 GetBalloonWorldPosition()
        {
            return Balloon.GetWorldPosition(Skeleton.transform);
        }

        /// <summary>
        /// 현재 프레임의 얼굴 방향 업데이트
        /// </summary>
        private void UpdateFaceAnimationDirection(float multiple_value, ref TrackEntry track1, ref TrackEntry track2, params string[] anim_names)
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
}

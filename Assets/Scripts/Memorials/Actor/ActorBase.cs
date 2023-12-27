using FluffyDuck.Util;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FluffyDuck.Memorial
{
    public abstract partial class ActorBase : MonoBehaviour
    {
        protected const int IDLE_BASE_TRACK = 0;
        const string FACE_BONE_NAME = "touch_center";
        const string BALLOON_BONE_NAME = "balloon";

        [SerializeField]
        protected SkeletonAnimation Skeleton;

        ////////////////////////// 상태
        protected Dictionary<int, IdleAnimationData> Idle_Animation_Datas;
        protected int Current_State_Id;
        private KeyValuePair<(TOUCH_GESTURE_TYPE geture_type, TOUCH_BODY_TYPE body_type), int> Gesture_Touch_Counts;
        protected int Idle_Played_Count;

        protected Bone Face; // 얼굴 위치를 가져오기 위한 본
        protected Bone Balloon; // 말풍선 위치 본

        Producer Producer;
        List<Me_Interaction_Data>[,] Touch_Type_Interactions;
        Me_Interaction_Data Selected_Interaction;

        protected Dictionary<int, Me_Chat_Motion_Data> Chat_Motions;
        protected int Selected_Chat_Motion_Id;

        Dictionary<int, Me_Serifu_Data> Serifues;

        TrackEntry Drag_Track_Entry;

        float Nade_Point = 0;

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
                        Selected_Chat_Motion_Id = Current_State_Data.Bored_Chatmotion_Ids[index];
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
        protected virtual void SpineAnimationEvent(TrackEntry entry, Spine.Event evt) { }

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

            Selected_Interaction = GetInteractionData(TOUCH_GESTURE_TYPE.TOUCH, bounding_box);
            if (Selected_Interaction == null)
            {
                return;
            }

            SetReactionData(Selected_Interaction, TOUCH_GESTURE_TYPE.TOUCH, bounding_box);

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

            Selected_Interaction = GetInteractionData(TOUCH_GESTURE_TYPE.DOUBLE_TOUCH, bounding_box);
            if (Selected_Interaction == null)
            {
                return;
            }

            SetReactionData(Selected_Interaction, TOUCH_GESTURE_TYPE.DOUBLE_TOUCH, bounding_box);

            FSM.ChangeState(ACTOR_STATES.REACT);
        }

        protected virtual void OnDrag(ICursorInteractable comp, Vector2 position, Vector2 drag_vector, int state)
        {
            bool isDragValid = FSM.CurrentTransitionID == ACTOR_STATES.DRAG && state >= 1;
            bool isIdleValid = FSM.CurrentTransitionID == ACTOR_STATES.IDLE && state == 0;
            if (!isDragValid && !isIdleValid)
            {
                return;
            }

            var bounding_box = comp as SpineBoundingBox;
            if (bounding_box == null)
            {
                Debug.Assert(false, "ActorBase::OnTap : 터치된 bounding_box가 존재하지 않습니다.");
                return;
            }

            var data = GetInteractionData(TOUCH_GESTURE_TYPE.DRAG, bounding_box);

            if (data == null)
            {
                return;
            }

            Vector2 drag_dest = bounding_box.GetPtDirection();

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
                switch(state)
                {
                    case 0:
                        Debug.Log("SET!!".WithColorTag(Color.red));
                        Selected_Interaction = data;
                        SetReactionData(Selected_Interaction, TOUCH_GESTURE_TYPE.DRAG, bounding_box);

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
                                Debug.Log($"close_value : {close_value}".WithColorTag(Color.red));

                                Drag_Track_Entry.TrackTime = close_value;
                            }
                            else
                            {
                                Debug.Log($"REACT".WithColorTag(Color.red));
                                FSM.ChangeState(ACTOR_STATES.REACT);
                            }
                        }
                        break;
                    case 2:
                        FSM.ChangeState(ACTOR_STATES.IDLE);
                        break;
                }
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
                Debug.Assert(false, "ActorBase::OnTap : 터치된 bounding_box가 존재하지 않습니다.");
                return;
            }

            var data = GetInteractionData(TOUCH_GESTURE_TYPE.NADE, bounding_box);
            
            if (data == null)
            {
                return;
            }

            switch (state)
            {
                case 0:
                    Selected_Interaction = data;
                    SetReactionData(Selected_Interaction, TOUCH_GESTURE_TYPE.NADE, bounding_box);

                    FSM.ChangeState(ACTOR_STATES.NADE);
                    break;
                case 1:
                    Nade_Point += delta.sqrMagnitude * 0.01f;
                    if (Nade_Point < 1)
                    {
                        Debug.Log($"Nade_Point : {Nade_Point}".WithColorTag(Color.red));
                        // TODO:
                    }
                    else
                    {
                        Debug.Log($"Nade_Point : {Nade_Point}".WithColorTag(Color.red));
                        FSM.ChangeState(ACTOR_STATES.REACT);
                    }
                    break;
                case 2:
                    FSM.ChangeState(ACTOR_STATES.IDLE);
                    break;
            }
        }

        private Me_Interaction_Data GetInteractionData(TOUCH_GESTURE_TYPE type, SpineBoundingBox bounding_box)
        {
            var interaction_datas = Touch_Type_Interactions[(int)type, (int)bounding_box.GetTouchBodyType()];

            if (interaction_datas == null || interaction_datas.Count == 0)
            {
                return null;
            }

            int count = 1;
            var key = (type, bounding_box.GetTouchBodyType());
            if (Gesture_Touch_Counts.Key.Equals(key))
            {
                count += Gesture_Touch_Counts.Value;
            }

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
            catch(InvalidOperationException)
            {
                // 조건들을 통과 못하는 경우는 그냥 패스
            }
            catch(Exception e)
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
            
            int index = Array.IndexOf(data.chat_motion_ids, Selected_Chat_Motion_Id) + 1;

            if (index >= data.chat_motion_ids.Length)
            {
                index = 0;
            }

            Selected_Interaction = data;
            Selected_Chat_Motion_Id = data.chat_motion_ids[index];
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

        public record IdleAnimationData
        {
            public string Animation_Idle_Name;
            public int[] Bored_Chatmotion_Ids;
            public int Bored_Count;
        }
    }
}

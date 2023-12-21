using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FluffyDuck.Memorial
{
    public abstract partial class ActorBase : MonoBehaviour
    {
        const string FACE_BONE_NAME = "touch_center";
        const string BALLOON_BONE_NAME = "balloon";

        [SerializeField]
        protected SkeletonAnimation Skeleton;

        protected Bone Face; // 얼굴 위치를 가져오기 위한 본
        protected Bone Balloon; // 말풍선 위치 본

        Producer Producer;
        List<Me_Interaction_Data>[,] Touch_Type_Interactions;
        int[] LastPlayedInteractionIndices;

        bool Initialize(Producer pd, Me_Resource_Data data)
        {
            if (Skeleton == null)
            {
                Debug.Assert(false, $"프리팹 {this.GetType()} 컴포넌트의 인스펙터에 SkeletonAnimation이 없습니다");
                return false;
            }
            Face = FindBone(FACE_BONE_NAME);
            Debug.Assert(Face != null, $"얼굴 추적 기준 본이 없습니다 : {FACE_BONE_NAME}");
            Balloon = FindBone(BALLOON_BONE_NAME);
            Debug.Assert(Face != null, $"말풍선용 본이 없습니다 : {BALLOON_BONE_NAME}");

            Producer = pd;
            Touch_Type_Interactions = MasterDataManager.Instance.Get_MemorialInteraction(data.player_character_id);
            LastPlayedInteractionIndices = Enumerable.Repeat(-1, Touch_Type_Interactions.Length).ToArray(); // 배열 생성하면서 -1로 전부 초기화

            Debug.Log("ActorBase::Initialize");

            return true;
        }

        /// <summary>
        /// 스켈레톤(스파인) 이벤트 리스너 등록
        /// </summary>
        protected void SetSkeletonEventListener()
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
        /// 스파인 애니메이션 시작시 호출되는 리스너
        /// </summary>
        /// <param name="entry"></param>
        protected virtual void SpineAnimationStart(TrackEntry entry) { }
        /// <summary>
        /// 스파인 애니메이션 동작 완료시 호출되는 리스너
        /// </summary>
        /// <param name="entry"></param>
        protected virtual void SpineAnimationComplete(TrackEntry entry)
        {
            string animation_name = entry.Animation.Name;
        }
        /// <summary>
        /// 스파인 애니메이션 동작 종료시 호출되는 리스너
        /// </summary>
        /// <param name="entry"></param>
        protected virtual void SpineAnimationEnd(TrackEntry entry) { }
        /// <summary>
        /// 스파인 애니메이션 동작 플레이 중 호출되는 이벤트를 받아주는 리스너
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="evt"></param>
        protected virtual void SpineAnimationEvent(TrackEntry entry, Spine.Event evt) { }


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
    }
}

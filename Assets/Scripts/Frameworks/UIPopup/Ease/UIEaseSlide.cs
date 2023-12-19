using FluffyDuck.Util;
using System;
using UnityEngine;

namespace FluffyDuck.UI
{
    public class UIEaseSlide : UIEaseBase
    {
        [SerializeField, Tooltip("등장시 도착 위치")]
        Vector2 In_End_Position;
        [SerializeField, Tooltip("숨김시 도착 위치")]
        Vector2 Out_End_Position;

        /// <summary>
        /// 이동전 시작 좌표
        /// </summary>
        Vector2 Start_Position;
        /// <summary>
        /// 이동전 시작 좌표와 이동후 도착 좌표간 거리
        /// </summary>
        Vector2 Distance;

        public override void StartMoveIn(System.Action cb = null)
        {
            Start_Position = This_Rect.localPosition;
            Distance = In_End_Position - Start_Position;
            base.StartMoveIn(cb);
        }

        public override void StartMoveOut(Action cb = null)
        {
            Start_Position = This_Rect.localPosition;
            Distance = Out_End_Position - Start_Position;
            base.StartMoveOut(cb);
        }

        protected override void UpdateEase(EasingFunction.Function func, float weight)
        {
            float ev = func(0.0f, 1.0f, weight);
            Vector2 easing_delta = Distance * ev;
            This_Rect.localPosition = Start_Position + easing_delta;
        }
        /// <summary>
        /// 이동 완료 후 호출되는 함수.
        /// 최종 목적지의 좌표에 약간의 오차가 있을 수 있기 때문에
        /// 오차를 줄이기 위해 목표지점으로 마지막 업데이트 해준다.
        /// </summary>
        protected override void UpdatePostDelayEnd()
        {
            if (Move_Type == MOVE_TYPE.MOVE_IN)
            {
                This_Rect.localPosition = In_End_Position;
            }
            else
            {
                This_Rect.localPosition = Out_End_Position;
            }
        }
    }

}

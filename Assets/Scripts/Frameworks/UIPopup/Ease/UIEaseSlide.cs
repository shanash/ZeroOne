using FluffyDuck.Util;
using System;
using UnityEngine;

namespace FluffyDuck.UI
{
    public class UIEaseSlide : UIEaseBase
    {
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
            var found = FindEaseData(MOVE_TYPE.MOVE_IN);
            if (found == null)
            {
                return;
            }
            Start_Position = This_Rect.localPosition;
            Distance = found.Ease_Vector - Start_Position;
            base.StartMoveIn(cb);
        }

        public override void StartMoveOut(Action cb = null)
        {
            var found = FindEaseData(MOVE_TYPE.MOVE_IN);
            if (found == null)
            {
                return;
            }
            Start_Position = This_Rect.localPosition;
            Distance = found.Ease_Vector - Start_Position;
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
            var found = FindEaseData(Move_Type);
            if (found != null)
            {
                This_Rect.localPosition = found.Ease_Vector;
            }
        }

        public override void ResetEase(params object[] data)
        {
            if (data.Length != 1)
            {
                return;
            }
            if (This_Rect != null)
            {
                Vector2 pos = (Vector2)data[0];
                This_Rect.localPosition = pos;
            }
        }

    }

}

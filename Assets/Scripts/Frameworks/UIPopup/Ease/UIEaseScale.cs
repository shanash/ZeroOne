using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.UI
{
    public class UIEaseScale : UIEaseBase
    {
        [SerializeField, Tooltip("등장시 최종 스케일")]
        Vector2 In_End_Scale;
        [SerializeField, Tooltip("숨김시 최종 스케일")]
        Vector2 Out_End_Scale;

        /// <summary>
        /// 시작전 스케일
        /// </summary>
        Vector2 Start_Scale;
        /// <summary>
        /// 시작전 스케일과 종료후 스케일의 차이
        /// </summary>
        Vector2 Diff_Scale;

        public override void StartMoveIn(Action cb = null)
        {
            Start_Scale = This_Rect.localScale;
            Diff_Scale = In_End_Scale - Start_Scale;
            base.StartMoveIn(cb);
        }
        public override void StartMoveOut(Action cb = null)
        {
            Start_Scale = This_Rect.localScale;
            Diff_Scale = Out_End_Scale - Start_Scale;
            base.StartMoveOut(cb);
        }

        protected override void UpdateEase(EasingFunction.Function func, float weight)
        {
            float ev = func(0.0f, 1.0f, weight);
            Vector2 easing_delta = Diff_Scale * ev;
            This_Rect.localScale = Start_Scale + easing_delta;
        }

        /// <summary>
        /// 이동 완료 후 호출되는 함수
        /// 최종 목표 스케일값에 약간의 오차가 있을 수 있기 때문에
        /// 오차를 줄이기 위해 목표 스케일로 마지막 업데이트를 해준다.
        /// </summary>
        protected override void UpdatePostDelayEnd()
        {
            if (Move_Type == MOVE_TYPE.MOVE_IN)
            {
                This_Rect.localScale = In_End_Scale;
            }
            else
            {
                This_Rect.localScale = Out_End_Scale;
            }
        }
    }
}


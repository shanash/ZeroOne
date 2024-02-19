using FluffyDuck.Util;
using System;
using UnityEngine;

namespace FluffyDuck.UI
{
    public class UIEaseScale : UIEaseBase
    {
        /// <summary>
        /// 시작전 스케일
        /// </summary>
        Vector2 Start_Scale;
        /// <summary>
        /// 시작전 스케일과 종료후 스케일의 차이
        /// </summary>
        Vector2 Diff_Scale;

        public override void StartMove(MOVE_TYPE mtype, Action cb = null)
        {
            var found = FindEaseData(mtype);
            if (found == null)
            {
                return;
            }
            Start_Scale = This_Rect.localScale;
            Diff_Scale = found.Ease_Vector - Start_Scale;
            base.StartMove(mtype, cb);
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
            var found = FindEaseData(Move_Type);
            if (found != null)
            {
                This_Rect.localScale = found.Ease_Vector;
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
                Vector2 scale = (Vector2)data[0];
                This_Rect.localScale = scale;
            }
        }
    }
}


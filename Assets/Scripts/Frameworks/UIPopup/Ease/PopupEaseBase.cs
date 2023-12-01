using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FluffyDuck.UI
{
    public class PopupEaseBase : EasingFade
    {
        protected enum MOVE_TYPE
        {
            MOVE_IN = 0,
            MOVE_OUT
        }
        /// <summary>
        /// 컨테이너의 RectTransform
        /// </summary>
        protected RectTransform This_Rect;

        [SerializeField, Tooltip("팝업 등장시 Ease Type")]
        protected EasingFunction.Ease Move_In_Ease_Type = EasingFunction.Ease.NotUse;

        [SerializeField, Tooltip("팝업 사라질 때 Ease Type")]
        protected EasingFunction.Ease Move_Out_Ease_Type = EasingFunction.Ease.NotUse;

        [SerializeField, Tooltip("지속시간")]
        protected float Ease_Duration;

        /// <summary>
        /// 기본 지속시간
        /// </summary>
        protected const float DEFAULT_DURATION = 0.2f;

        protected MOVE_TYPE Move_Type = MOVE_TYPE.MOVE_IN;

        private void Awake()
        {
            CheckThisRect();
        }

        protected void CheckThisRect()
        {
            if (This_Rect == null)
            {
                This_Rect = GetComponent<RectTransform>();
            }
        }
        /// <summary>
        /// 등장 애니메이션 요청
        /// </summary>
        /// <param name="cb"></param>
        public virtual void StartMoveIn(System.Action cb = null) 
        {
            Move_Type = MOVE_TYPE.MOVE_IN;
            if (Ease_Duration == 0f)
            {
                Ease_Duration = DEFAULT_DURATION;
            }
            SetEasing(Move_In_Ease_Type, 0, Ease_Duration);
            StartEasing(cb);
        }

        /// <summary>
        /// 숨김 애니메이션 요청
        /// </summary>
        /// <param name="cb"></param>
        public virtual void StartMoveOut(System.Action cb = null) 
        {
            Move_Type = MOVE_TYPE.MOVE_OUT;
            if (Ease_Duration == 0f)
            {
                Ease_Duration = DEFAULT_DURATION;
            }
            SetEasing(Move_Out_Ease_Type, 0, Ease_Duration);
            StartEasing(cb);
        }

        protected override void OnFadeUpdate(float weight)
        {
            if (EaseFade == EasingFunction.Ease.NotUse)
            {
                return;
            }
            UpdateEase(EasingFunction.GetEasingFunction(EaseFade), weight);
        }

        /// <summary>
        /// Ease type에 맞는 함수를 이용하여 Easing 계산
        /// </summary>
        /// <param name="func"></param>
        /// <param name="weight"></param>
        protected virtual void UpdateEase(EasingFunction.Function func, float weight) { }
        /// <summary>
        /// Easing Update 종료 후 후방 Delay 시간 종료되면 호출되는 함수.
        /// </summary>
        protected override void UpdatePostDelayEnd() { }
    }

}

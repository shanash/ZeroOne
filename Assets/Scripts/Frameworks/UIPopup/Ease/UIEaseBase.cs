using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace FluffyDuck.UI
{
    public class UIEaseBase : EasingFade
    {
        public enum MOVE_TYPE
        {
            NONE = 0,
            MOVE_IN,
            MOVE_OUT
        }

        [Serializable]
        protected class UIEaseData
        {
            /// <summary>
            /// Move Type
            /// </summary>
            public MOVE_TYPE Move_Type;
            public EasingFunction.Ease Ease_Type = EasingFunction.Ease.NotUse;
            /// <summary>
            /// 좌표/스케일 등으로 사용 가능
            /// </summary>
            public Vector2 Ease_Vector;
            /// <summary>
            /// 알파값 등으로 사용 가능
            /// </summary>
            public float Ease_Float;
            /// <summary>
            /// 지속시간
            /// </summary>
            public float Ease_Duration;
        }

        /// <summary>
        /// 컨테이너의 RectTransform
        /// </summary>
        protected RectTransform This_Rect;

        [SerializeField, Tooltip("Ease Data List")]
        protected List<UIEaseData> Ease_Data_List;


        /// <summary>
        /// 기본 지속시간
        /// </summary>
        protected const float DEFAULT_DURATION = 0.2f;

        protected MOVE_TYPE Move_Type = MOVE_TYPE.NONE;

        private void Awake()
        {
            InitCheckComponent();
        }
        protected virtual void InitCheckComponent()
        {
            if (This_Rect == null)
            {
                This_Rect = GetComponent<RectTransform>();
            }
        }

        protected UIEaseData FindEaseData(MOVE_TYPE mtype)
        {
            return Ease_Data_List.Find(x => x.Move_Type == mtype);
        }

        /// <summary>
        /// 애니메이션 시작
        /// </summary>
        /// <param name="mtype"></param>
        /// <param name="cb"></param>
        public virtual void StartMove(MOVE_TYPE mtype, System.Action cb = null)
        {
            var found = FindEaseData(mtype);
            if (found != null)
            {
                Move_Type = found.Move_Type;
                float duration = found.Ease_Duration;
                if (duration == 0f)
                {
                    duration = DEFAULT_DURATION;
                }
                SetEasing(found.Ease_Type, 0, duration);
                StartEasing(cb);
            }
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

        /// <summary>
        /// 상태를 초기화 시키기 위한 함수<br/>
        /// 값을 받아서 각각의 상황에 맞는 데이터를 사용한다.
        /// </summary>
        /// <param name="data"></param>
        public virtual void ResetEase(params object[] data) { }
    }

}

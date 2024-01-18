using FluffyDuck.UI;
using FluffyDuck.Util;
using System;

namespace FluffyDuck.UI
{
    public abstract class UIEaseAlphaBase : UIEaseBase
    {
        /// <summary>
        /// 시작시 Alpha
        /// </summary>
        protected float Start_Alpha;
        /// <summary>
        /// 시작과 종료시 Alpha 값의 차이
        /// </summary>
        protected float Diff_Alpha;

        protected abstract float InitialAlphaAtMovementStart { get; }

        protected abstract void InitializeAlphaProperties();

        protected abstract void UpdateAlphaTransparency(float alpha);

        protected override void InitCheckComponent()
        {
            base.InitCheckComponent();
            InitializeAlphaProperties();
        }

        public override void StartMove(MOVE_TYPE mtype, Action cb = null)
        {
            var found = FindEaseData(mtype);
            if (found == null)
            {
                return;
            }
            Start_Alpha = InitialAlphaAtMovementStart;
            Diff_Alpha = found.Ease_Float - Start_Alpha;
            base.StartMove(mtype, cb);
        }

        protected override void OnFadeUpdate(float weight)
        {
            if (EaseFade == EasingFunction.Ease.NotUse)
            {
                return;
            }
            UpdateAlphaFader(EasingFunction.GetEasingFunction(EaseFade), weight);
        }

        void UpdateAlphaFader(EasingFunction.Function func, float weight)
        {
            float ev = func(0.0f, 1.0f, weight);
            float easing_alpah = Diff_Alpha * ev;

            UpdateAlphaTransparency(Start_Alpha + easing_alpah);
        }

        protected override void UpdatePostDelayEnd()
        {
            var found = FindEaseData(Move_Type);
            if (found != null)
            {
                UpdateAlphaTransparency(found.Ease_Float);
            }
        }

        public override void ResetEase(params object[] data)
        {
            if (data.Length != 1)
            {
                return;
            }
            float alpha = (float)data[0];

            UpdateAlphaTransparency(alpha);
        }
    }
}

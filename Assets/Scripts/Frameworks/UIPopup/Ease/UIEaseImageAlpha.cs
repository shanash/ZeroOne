using System;
using UnityEngine;
using UnityEngine.UI;

namespace FluffyDuck.UI
{
    [RequireComponent(typeof(Image))]
    public class UIEaseImageAlpha : UIEaseAlphaBase
    {
        [SerializeField]
        protected Image Image;

        protected override float InitialAlphaAtMovementStart => Image.color.a;

        protected override void InitializeAlphaProperties() { }

        protected override void UpdateAlphaTransparency(float alpha)
        {
            var current_color = Image.color;
            current_color.a = alpha;
            Image.color = current_color;
        }

        /// <summary>
        /// Easing을 이용하여 알파 변경
        /// </summary>
        /// <param name="data">목표로 할 알파값</param>
        public override void StartEasing(object data, Action cb = null)
        {
            Start_Alpha = Image.color.a;
            Diff_Alpha = (float)(data) - Start_Alpha;
            base.StartEasing(data, cb);
        }
    }
}

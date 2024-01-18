using UnityEngine;

namespace FluffyDuck.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIEaseCanvasGroupAlpha : UIEaseAlphaBase
    {
        protected CanvasGroup Canvas_Grp;

        protected override float InitialAlphaAtMovementStart => Canvas_Grp.alpha;

        protected override void InitializeAlphaProperties()
        {
            if (Canvas_Grp == null)
                Canvas_Grp = GetComponent<CanvasGroup>();
        }

        protected override void UpdateAlphaTransparency(float alpha)
        {
            if (Canvas_Grp != null)
            {
                Canvas_Grp.alpha = alpha;
            }
        }
    }
}

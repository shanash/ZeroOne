using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScalerTest : MonoBehaviour, IPointerClickHandler
{
    public EasingFade Fade;
    public Text Easing_Name;

    FluffyDuck.Util.EasingFunction.Ease Ease_Type = FluffyDuck.Util.EasingFunction.Ease.EaseInQuad;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Fade.IsPlaying())
        {
            return;
        }
        Ease_Type += 1;
        if (Ease_Type >= FluffyDuck.Util.EasingFunction.Ease.NotUse)
        {
            Ease_Type = FluffyDuck.Util.EasingFunction.Ease.EaseInQuad;
        }
        
        Fade?.SetEasing(Ease_Type, 0, 0.5f);
        int t = (int)Ease_Type;
        if (t % 2 == 0)
        {
            Fade?.StartEasing(Vector2.one);
        }
        else
        {
            Fade?.StartEasing(-Vector2.one);
        }
        
        Easing_Name.text = Ease_Type.ToString();
    }

    private void OnEnable()
    {
        Fade?.SetEasing(Ease_Type, 0, 0.5f);
        Fade?.StartEasing(Vector2.one);
        Easing_Name.text = Ease_Type.ToString();
    }

}

using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RotateTest : MonoBehaviour, IPointerClickHandler
{
    public EasingFade Fade;
    public Text Easing_Name;

    FluffyDuck.Util.EasingFunction.Ease Ease_Type = FluffyDuck.Util.EasingFunction.Ease.EaseInQuad;

    Vector3 rotate = new Vector3(0, 0, 360);

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
            Fade?.StartEasing(rotate);
        }
        else
        {
            Fade?.StartEasing(-rotate);
        }
        Easing_Name.text = Ease_Type.ToString();
    }

    private void OnEnable()
    {
        Fade?.SetEasing(Ease_Type, 0, 0.5f);
        Fade?.StartEasing(rotate);
        Easing_Name.text = Ease_Type.ToString();
    }
}

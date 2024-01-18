using FluffyDuck.UI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffectUIContainer : MonoBehaviour
{
    [SerializeField, Tooltip("페이드 효과를 위한 이미지")]
    Image _Cover_Image;
    
    [SerializeField, Tooltip("페이드 효과를 위한 Ease")]
    UIEaseImageAlpha _Cover_Image_EaseAlpha;

    public Image Cover_Image => _Cover_Image;
    public UIEaseImageAlpha Cover_Image_EaseAlpha => _Cover_Image_EaseAlpha;
}

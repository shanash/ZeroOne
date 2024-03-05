using Cysharp.Threading.Tasks.Triggers;
using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadingPopup : PopupBase
{
    [SerializeField, Tooltip("스탠딩 캐릭터 이미지")]
    Image Standing_Character_Image;

    [SerializeField, Tooltip("캐릭터 이름")]
    TMP_Text Name_Text;

    [SerializeField, Tooltip("나이 타이틀")]
    TMP_Text Age_Title;
    [SerializeField, Tooltip("나이")]
    TMP_Text Age_Text;

    [SerializeField, Tooltip("생일 타이틀")]
    TMP_Text Birthday_Title;
    [SerializeField, Tooltip("생일")]
    TMP_Text Birthday_Text;

    [SerializeField, Tooltip("키 타이틀")]
    TMP_Text Height_Title;
    [SerializeField, Tooltip("키")]
    TMP_Text Height_Text;

    [SerializeField, Tooltip("취미 타이틀")]
    TMP_Text Hobby_Title;
    [SerializeField, Tooltip("취미")]
    TMP_Text Hobby_Text;

    [SerializeField, Tooltip("로딩 게이지 바")]
    Slider Loading_Gauge;

    [SerializeField, Tooltip("로딩 바")]
    RectTransform Loading_Bar;

    /// <summary>
    /// 달리기 영웅 (scale = 0.2)
    /// </summary>
    [SerializeField, Tooltip("달리기 영웅")]
    UIHeroBase Running_Hero;

    protected override bool Initialize(object[] data)
    {
        Running_Hero.PlayAnimation(HERO_PLAY_ANIMATION_TYPE.RUN_01);
        return true;
    }

    public void SetProgressCallback(float progress)
    {
        Debug.Log($"SetProgressCallback => {progress * 100f}");
        Loading_Gauge.value = Mathf.Clamp01(progress);
    }
}

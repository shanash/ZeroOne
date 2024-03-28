using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultRewardPopup : PopupBase
{
    [Space()]
    [Header("Container")]
    [SerializeField, Tooltip("BG Image")]
    Image BG_Image;

    [SerializeField, Tooltip("컨테이너")]
    RectTransform Container;

    [SerializeField, Tooltip("등장 애니")]
    Animator Container_Anim;

    [Space()]
    [Header("리워드")]
    [SerializeField, Tooltip("리워드 컨테이너")]
    RectTransform Reward_Container;

    [Space()]
    [Header("Buttons")]
    [SerializeField, Tooltip("홈 버튼 텍스트")]
    TMP_Text Home_Btn_Text;

    [SerializeField, Tooltip("다음 버튼 텍스트")]
    TMP_Text Next_Btn_Text;

    #region Vars
    
    Texture BG_Texture;         //  blur 배경 이미지 텍스쳐

    #endregion

    public void OnClickHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }
    public void OnClickNext()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }
}

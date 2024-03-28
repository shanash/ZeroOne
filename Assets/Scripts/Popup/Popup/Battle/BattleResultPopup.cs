using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultPopup : PopupBase
{
    [Space()]
    [Header("배경")]
    Image BG_Image;

    [Space()]
    [Header("시퀀스 0 => 영웅 대기")]
    [SerializeField, Tooltip("영웅 대기 장소")]
    RectTransform Waiting_Room;
    

    [Space()]
    [Header("시퀀스 1 => 승리/패배 애니")]

    [SerializeField, Tooltip("승리 - 자동 플레이")]
    GameObject Title_Victory;
    [SerializeField, Tooltip("패배 - 자동 플레이")]
    GameObject Title_Defeat;

    [Space()]
    [Header("시퀀스 2 => 별 애니")]
    [SerializeField, Tooltip("별 갯수 애니")]
    Animator Result_Star_Anim;

    [Space()]
    [Header("시퀀스 3 => 경험치 결과 애니")]
    [SerializeField, Tooltip("경험치 결과 컨테이너 - 자동 플레이")]
    GameObject Result_Exp;

    [Space()]
    [Header("시퀀스 4 => 플레이어 경험치 증가 보여주기 - 레벨업 시 레벨업 팝업 보여준 후 시퀀스 5로 넘어가기")]
    [SerializeField, Tooltip("플레이어 닉네임")]
    TMP_Text Player_Nickname;
    [SerializeField, Tooltip("플레이어 경험치 게이지")]
    Slider Player_Exp_Gauge;
    [SerializeField, Tooltip("플레이어 획득 경험치")]
    TMP_Text Player_Gain_Exp;
    [SerializeField, Tooltip("플레이어 레벨")]
    TMP_Text Player_Level_Text;
    [SerializeField, Tooltip("기본 보상 아이템")]
    Image Default_Reward_Item_Icon;
    [SerializeField, Tooltip("기본 보상 아이템 획득 갯수")]
    TMP_Text Default_Reward_Item_Count;

    [Space()]
    [Header("시퀀스 5 => 각 캐릭터별 순서대로 경험치 게이지 증가(레벨업 시 레벨업 표시하고 다음으로)")]
    [SerializeField, Tooltip("캐릭터 정보 리스트 컨테이너")]
    RectTransform Hero_Info_List_Container;

    [Space()]
    [Header("시퀀스 6 => 버튼 보이기")]
    [SerializeField, Tooltip("다음 버튼")]
    Button Next_Btn;
    [SerializeField, Tooltip("다음 버튼 텍스트")]
    TMP_Text Next_Btn_Text;
    

    Texture BG_Texture;         //  blur 배경 이미지 텍스쳐
}

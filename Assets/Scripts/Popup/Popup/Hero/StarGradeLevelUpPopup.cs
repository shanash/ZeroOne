using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarGradeLevelUpPopup : PopupBase
{
    [SerializeField, Tooltip("팝업 타이틀")]
    TMP_Text Popup_Title;


    [SerializeField, Tooltip("스탯 정보 리스트 뷰")]
    ScrollRect Status_List_View;

    [SerializeField, Tooltip("스탯 정보 리스트")]
    List<HeroStatusInfoNode> Stat_Info_List;

    [SerializeField, Tooltip("캐릭터 이름")]
    TMP_Text Character_Name;

    [SerializeField, Tooltip("현재 성급 리스트")]
    List<Image> Current_Star_Grades;

    [SerializeField, Tooltip("결과 성급 리스트")]
    List<Image> Result_Star_Grades;

    [SerializeField, Tooltip("캐릭터 아이콘")]
    HeroCardBase Card;

    [SerializeField, Tooltip("소모 재료 타이틀")]
    TMP_Text Cost_Material_Title;

    [SerializeField, Tooltip("소모 조각 개수")]
    TMP_Text Cost_Piece_Count;

    [SerializeField, Tooltip("필요 골드")]
    TMP_Text Need_Gold_Count;

    [SerializeField, Tooltip("취소 버튼 텍스트")]
    TMP_Text Cancel_Btn_Text;

    [SerializeField, Tooltip("성장 버튼")]
    UIButtonBase Star_Grade_Up_Btn;

    [SerializeField, Tooltip("성장 버튼 텍스트")]
    TMP_Text Star_Grade_Up_Btn_Text;

    BattlePcData Before_Data;
    BattlePcData After_Data;

    bool Is_Load_Complete;
    bool Is_Anim_Complete;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1 || data[0] is not BattlePcData)
        {
            return false;
        }
        Before_Data = (BattlePcData)data[0];
        After_Data = Before_Data.GetNextStarGradeData();
        FixedUpdatePopup();
        return true;
    }

    protected override void ShowPopupAniEndCallback()
    {
        Status_List_View.verticalNormalizedPosition = 1f;
    }



    protected override void FixedUpdatePopup()
    {
        //  popup title
        Popup_Title.text = GameDefine.GetLocalizeString("system_stargrowup");
        
        //  stat info
        UpdateStatInfoList();

        //  name
        Character_Name.text = Before_Data.GetUnitName();

        //  before star -> after star
        int before_star_count = Before_Data.GetStarGrade();
        for (int i = 0; i < Current_Star_Grades.Count; i++)
        {
            Current_Star_Grades[i].gameObject.SetActive(i < before_star_count);
        }
        int after_star_count = After_Data.GetStarGrade();
        for(int i = 0; i < Result_Star_Grades.Count; i++)
        {
            Result_Star_Grades[i].gameObject.SetActive(i < after_star_count);
        }

        //  hero card
        Card.SetHeroDataID(Before_Data.GetUnitID());

        //  cost material
        Cost_Piece_Count.text = Before_Data.User_Data.GetNeedPiece().ToString("N0");

        //  need gold
        Need_Gold_Count.text = Before_Data.User_Data.GetNeedGold().ToString("N0");

        //  cancel

        //  level up
        Star_Grade_Up_Btn_Text.text = GameDefine.GetLocalizeString("system_growup");

    }


    void UpdateStatInfoList()
    {
        int cnt = Stat_Info_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Stat_Info_List[i].SetBattleUnitData(Before_Data, After_Data);
        }
        
    }

    void OnResponseStarGrade(RESPONSE_TYPE code)
    {
        if (code != RESPONSE_TYPE.SUCCESS)
        {
            if (code == RESPONSE_TYPE.ALREADY_MAX_LEVEL)
            {
                ShowNoticePopup("이미 최대 성급입니다.", 1.5f);
            }
            else if (code == RESPONSE_TYPE.NOT_ENOUGH_GOLD)
            {
                ShowNoticePopup("골드가 부족합니다.", 1.5f);
            }
            else if (code == RESPONSE_TYPE.NOT_ENOUGH_ITEM)
            {
                ShowNoticePopup("조각이 부족합니다.", 1.5f);
            }
            else if (code == RESPONSE_TYPE.NOT_ENOUGH_ALL)
            {
                ShowNoticePopup("골드 및 조각이 부족합니다.", 1.5f);
            }
            return;
        }

        GameData.Instance.GetUserGoodsDataManager().Save();
        GameData.Instance.GetUserItemDataManager().Save();
        GameData.Instance.GetUserHeroDataManager().Save();
        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_GOLD);

        Closed_Delegate?.Invoke();
        HidePopup(() =>
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
            
        });
    }

    public void OnClickStarGradeUp()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Before_Data.AdvanceStarGrade(OnResponseStarGrade);
    }

    public void OnClickClose()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    void ShowNoticePopup(string msg, float duration)
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(duration, msg);
        });

    }
}

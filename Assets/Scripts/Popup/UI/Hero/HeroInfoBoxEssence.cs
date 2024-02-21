using FluffyDuck.UI;
using FluffyDuck.Util;
using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoBoxEssence : MonoBehaviour
{
    const int PERCENT = 60;
    const int FOUNDED_SPOT_NUM = 3;
    const int DATE_COUNT = 2;

    /// <summary>
    /// "근원 전달 달성률"
    /// </summary>
    [SerializeField]
    TMP_Text TransferEssencePercent_Subject;

    /// <summary>
    /// 근원전달 퍼센트 텍스트
    /// </summary>
    [SerializeField]
    TMP_Text TransferEssencePercent_Value;
    
    /// <summary>
    /// 근원전달 게이지 이미지
    /// </summary>
    [SerializeField]
    Image TransferEssencePercent_Image;

    /// <summary>
    /// "전투력"
    /// </summary>
    [SerializeField]
    TMP_Text CombatPower_Subject;

    /// <summary>
    /// 전투력 플러스 수치
    /// </summary>
    [SerializeField]
    TMP_Text AddedCombatPower_Value;

    /// <summary>
    /// "전달 반응"
    /// </summary>
    [SerializeField]
    TMP_Text TransferReaction_Subject;

    /// <summary>
    /// 전달 반응 토글들의 그룹
    /// </summary>
    [SerializeField]
    ToggleGroup TransferReaction_ToggleGroup;

    /// <summary>
    /// 전달 반응 토글 스크롤
    /// </summary>
    [SerializeField]
    InfiniteScroll TransferReaction_Buttons_View;

    /// <summary>
    /// "스팟 발견"
    /// </summary>
    [SerializeField]
    TMP_Text FoundSpot_Subject;

    /// <summary>
    /// 발견한 스팟 갯수 이미지 오브젝트
    /// </summary>
    [SerializeField]
    GameObject[] FoundSpot_Value;

    /// <summary>
    /// "스킵하기"
    /// </summary>
    [SerializeField]
    TMP_Text FoundSpot_Skip_Button_Value;

    /// <summary>
    /// 오늘 사용가능한 근원전달 재화 "2/2"
    /// </summary>
    [SerializeField]
    TMP_Text Transfer_Resources_Value;

    /// <summary>
    /// "근원 전달"
    /// </summary>
    [SerializeField]
    TMP_Text TransferEssence_Button_Value;


    List<RelationshipToggleItemData> Toggle_Datas;
    int Selected_Relationship_Index = 0;

    public void SetHeroData(BattleUnitData _/*data*/)
    {
        //Unit_Data = data;
        FixedUpdatePopup();
    }

    public void FixedUpdatePopup()
    {
        TransferEssencePercent_Subject.text = ConstString.HeroInfoUI.ESSENCE_TRANSFER_PERCENT;
        CombatPower_Subject.text = ConstString.Hero.COMBAT_POWER;
        TransferReaction_Subject.text = ConstString.HeroInfoUI.ESSENCE_TRANSFER_REACTION;
        FoundSpot_Subject.text = ConstString.HeroInfoUI.ESSENCE_FOUND_SPOT;
        FoundSpot_Skip_Button_Value.text = ConstString.HeroInfoUI.ESSENCE_SKIP;
        TransferEssence_Button_Value.text = ConstString.HeroInfoUI.ESSENCE_TRANSFER;

        int percent = PERCENT;

        TransferReaction_Buttons_View.Clear();

        Toggle_Datas = new List<RelationshipToggleItemData>();

        for (int i = 0; i < (int)LOVE_LEVEL_TYPE.LOVE; i++)
        {
            /*
            var toggle_data = new RelationshipToggleItemData(i,
                ConstString.Hero.LOVE_LEVEL[i],
                TransferReaction_ToggleGroup,
                (i * 20 <= percent),
                OnToggleChangedRelationship);
            */
            //TODO: 0번째 1번째만 활성화 시켜줍니다
            var toggle_data = new RelationshipToggleItemData(i,
                ConstString.Hero.LOVE_LEVEL[i],
                TransferReaction_ToggleGroup,
                i <= 1,
                OnToggleChangedRelationship);
            Toggle_Datas.Add(toggle_data);
            TransferReaction_Buttons_View.InsertData(toggle_data);
        }

        Refresh();
    }

    void Refresh()
    {
        int percent = PERCENT;
        int found_spot_number = FOUNDED_SPOT_NUM;

        TransferEssencePercent_Value.text = percent.ToPercentage();
        TransferEssencePercent_Image.fillAmount = percent.ToPercentageFloat();

        var list = MasterDataManager.Instance.Get_EssenceStatusDataRange(1, percent);

        int add_atk = 0;
        int add_matk = 0;
        int add_def = 0;
        int add_mdef = 0;
        int add_hp = 0;

        foreach (var item in list)
        {
            add_atk += item.add_atk;
            add_matk += item.add_matk;
            add_def += item.add_def;
            add_mdef += item.add_mdef;
            add_hp += item.add_hp;
        }

        var combat = (int)GameCalc.GetCombatPoint(add_hp, add_atk, add_def, 0, 0, 0, 0, 0);
        
        AddedCombatPower_Value.text = ConstString.FormatPlus(combat);

        for (int i = 0; i < FoundSpot_Value.Length; i++)
        {
            FoundSpot_Value[i].SetActive(found_spot_number > i);
        }

        Transfer_Resources_Value.text = $"{DATE_COUNT}/{DATE_COUNT}";
    }

    public void OnClickBuffDetailButton()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/StatusPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            var data_list = MasterDataManager.Instance.Get_EssenceStatusDataRange(1, 100);

            List<StatusItemData> status_list = new List<StatusItemData>();

            foreach (var data in data_list)
            {
                status_list.Add(new StatusItemData(data.essence_charge_per.ToPercentage(), data.ToStringForUI()));
            }

            popup.ShowPopup(ConstString.StatusPopup.ESSENCE_TITLE, status_list);
        });
    }

    public void OnClickReactionInfoButton()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnToggleChangedRelationship(int index, bool is_on)
    {
        if (!is_on)
        {
            return;
        }

        for (int i = 0; i < Toggle_Datas.Count; i++)
        {
            Toggle_Datas[i].Selected = (i == index);
        }

        TransferReaction_Buttons_View.UpdateAllData();

        Selected_Relationship_Index = index;
    }

    public void OnClickSpotInfoSkipButton()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnClickTransferReactionInfoButton()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
    }

    public void OnClickTransferEssenceButton()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/EssenceTransferPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup();
        });
    }

    public void OnSelectedTab(Gpm.Ui.Tab tab)
    {
        Refresh();
    }
}

using FluffyDuck.UI;
using System.Collections.Generic;
using TMPro;
using UI.SkillLevelPopup;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoBoxLevelUp : MonoBehaviour
{
    const int MAX_EXP_ITEM_ID = 10;

    [SerializeField]
    TMP_Text Level_Text = null;

    [SerializeField]
    TMP_Text Result_Level_Text = null;

    [SerializeField]
    Slider Exp_Bar = null;

    [SerializeField]
    TMP_Text[] Stat_Text = null;

    [SerializeField, Tooltip("경험치 아이템")]
    SkillExpItem[] Exp_Items = null;

    [SerializeField, Tooltip("사용 골드 수치 텍스트")]
    TMP_Text Need_Gold_Text = null;

    [SerializeField, Tooltip("자동선택 버튼")]
    UIButtonBase AutoSelect_Button = null;

    [SerializeField, Tooltip("레벨업 버튼")]
    UIButtonBase Up_Button = null;

    BattlePcData Unit_Data = null;
    List<USE_EXP_ITEM_DATA> Use_Exp_Items = null;
    List<UserItem_NormalItemData> Exist_Exp_Items = null;

    public void SetHeroData(BattlePcData data)
    {
        Unit_Data = data;

        Use_Exp_Items = new List<USE_EXP_ITEM_DATA>();
        Exist_Exp_Items = new List<UserItem_NormalItemData>();

        FixedUpdatePopup();
    }

    public void FixedUpdatePopup()
    {
        Exist_Exp_Items.Clear();
        // 유저가 소유하고 있는 경험치 아이템 가져오기
        for (int i = 0; i < 5; i++)
        {
            var data = (UserItem_NormalItemData)GameData.Instance.GetUserItemDataManager().FindUserItem(ITEM_TYPE_V2.EXP_POTION_C, MAX_EXP_ITEM_ID - i);
            if (data != null)
            {
                Debug.Log($"data.Item_ID : {data.Item_ID}");
                
                Exist_Exp_Items.Add(data);
            }
        }

        UpdateExpItemButtons();
        Refresh();
    }

    void Refresh()
    {
        if (Unit_Data == null)
        {
            return;
        }

        Use_Exp_Items.Clear();
        UpdateExpItemButtons();

        UpdateUI();
    }

    void UpdateUI()
    {
        Level_Text.text = Unit_Data.GetLevel().ToString();

        Exp_Bar.value = Unit_Data.User_Data.GetExpPercetage();
    }

    /// <summary>
    /// 경험치 아이템 버튼 업데이트<br />
    /// 경험치 아이템 버튼을 눌렀을때 업데이트 시켜주면 순환 호출을 하기 때문에<br />
    /// UpdateUI에서는 별도로 분리
    /// </summary>
    void UpdateExpItemButtons()
    {
        for (int i = 0; i < Exp_Items.Length; i++)
        {
            if (Exist_Exp_Items.Count <= i)
            {
                Exp_Items[i].gameObject.SetActive(false);
                continue;
            }

            if (!Use_Exp_Items.Exists(x => x.Item_ID == Exist_Exp_Items[i].Data.item_id))
            {
                Use_Exp_Items.Add(CreateExpItem(Exist_Exp_Items[i].Data.item_id, 0));
            }

            Exp_Items[i].Initialize(Exist_Exp_Items[i], OnChangedUseItemCount);
            Exp_Items[i].gameObject.SetActive(Exist_Exp_Items[i].GetCount() > 0);
            var item = Use_Exp_Items.Find(x => x.Item_ID == Exist_Exp_Items[i].Item_ID);
            Exp_Items[i].SetUseCount(item.Use_Count);
        }
    }


    public void OnSelectedTab(Gpm.Ui.Tab tab)
    {
        Refresh();
    }

    public void OnClickLevelUp()
    {
        Unit_Data.User_Data.AddExpUseItem(OnResponseLevelup, Use_Exp_Items);
    }

    void OnResponseLevelup(USE_EXP_ITEM_RESULT_DATA result)
    {
        if (result.Code != ERROR_CODE.SUCCESS && result.Code != ERROR_CODE.LEVEL_UP_SUCCESS)
        {
            Debug.Assert(false, $"스킬 레벨업 에러 ERROR_CODE :{result.Code}");
            return;
        }

        GameData.Instance.GetUserHeroDataManager().Save();
        GameData.Instance.GetUserGoodsDataManager().Save();
        GameData.Instance.GetUserItemDataManager().Save();

        Use_Exp_Items.Clear();
        UpdateExpItemButtons();
        UpdateUI();

        if (result.Code == ERROR_CODE.LEVEL_UP_SUCCESS)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
        }
    }

    public ERROR_CODE OnChangedUseItemCount(int item_id, int count)
    {
        if (Unit_Data.User_Data.GetMaxLevel() == Unit_Data.User_Data.GetLevel())
        {
            return ERROR_CODE.ALREADY_MAX_LEVEL;
        }

        var item = Use_Exp_Items.Find(x => x.Item_ID == item_id);
        item.Use_Count = count;

        return ERROR_CODE.SUCCESS;
    }

    USE_EXP_ITEM_DATA CreateExpItem(int item_id, int count)
    {
        return new USE_EXP_ITEM_DATA()
        {
            Item_ID = item_id,
            Item_Type = ITEM_TYPE_V2.EXP_POTION_C,
            Use_Count = count,
        };
    }
}

using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections.Generic;
using UI.SkillLevelPopup;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using FluffyDuck.Util;

public class SkillLevelPopup : PopupBase
{
    const int max_exp_item_id = 20;
    readonly SKILL_TYPE[] skill_type_by_index = { SKILL_TYPE.SPECIAL_SKILL, SKILL_TYPE.SKILL_01, SKILL_TYPE.SKILL_02, /*차후에 패시브*/ };

    [SerializeField, Tooltip("탭 컨트롤러")]
    TabController TabController = null;

    [SerializeField, Tooltip("스킬 탭")]
    SkillInfoTab[] Skill_Tab_Ui = null;

    [SerializeField, Tooltip("스킬 이름")]
    TMP_Text Skill_Name = null;

    [SerializeField, Tooltip("레벨업 이전과 이후의 스킬 정보")]
    SkillInfo[] skillInfos = null;

    [SerializeField, Tooltip("경험치 텍스트\'0/100\'")]
    TMP_Text Add_Exp_Text = null;

    [SerializeField, Tooltip("경험치 바")]
    Slider Add_Exp_Bar = null;

    [SerializeField, Tooltip("경험치 아이템")]
    SkillExpItem[] Exp_Items = null;

    [SerializeField, Tooltip("사용 골드 수치 텍스트")]
    TMP_Text Need_Gold_Text = null;

    [SerializeField, Tooltip("자동선택 버튼")]
    UIButtonBase AutoSelect_Button = null;

    [SerializeField, Tooltip("레벨업 버튼")]
    UIButtonBase Up_Button = null;

    IReadOnlyList<UserHeroSkillData> Skill_Groups = null;

    List<UserItem_NormalItemData> Exist_Exp_Items = null;

    List<USE_EXP_ITEM_DATA> Use_Exp_Items = null;

    int Selected_Index = -1;

    UserHeroSkillData[] Current_SkillGroups = null;

    UserHeroSkillData Current_SkillGroup => Current_SkillGroups[Selected_Index];
    UserHeroSkillData After_SkillGroup { get; set; } = null;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1 || data[0] is not IReadOnlyList<UserHeroSkillData>)
        {
            return false;
        }

        Skill_Groups = data[0] as IReadOnlyList<UserHeroSkillData>;
        Use_Exp_Items = new List<USE_EXP_ITEM_DATA>();

        Exist_Exp_Items = new List<UserItem_NormalItemData>();

        FixedUpdatePopup();
        return true;
    }

    ERROR_CODE UpdateUI(bool use_levelup_animation = false)
    {
        Skill_Name.text = Current_SkillGroup.Group_Data.name_kr;
        UpdateExpItemButtons();

        return SetLevelupData(use_levelup_animation);
    }

    void UpdateExpItemButtons()
    {
        for (int i = 0; i < Exist_Exp_Items.Count; i++)
        {
            Exp_Items[i].Initialize(Exist_Exp_Items[i], OnChangedUseItemCount);
            Exp_Items[i].gameObject.SetActive(Exist_Exp_Items[i].GetCount() > 0);
            if (Use_Exp_Items.Exists(x => x.Item_ID == Exist_Exp_Items[i].Item_ID))
            {
                var item = Use_Exp_Items.Find(x => x.Item_ID == Exist_Exp_Items[i].Item_ID);
                Exp_Items[i].SetUseCount(item.Use_Count);
            }
        }
    }

    protected override void FixedUpdatePopup()
    {
        TabController.GetTab(3).SetBlockTab(true);

        Current_SkillGroups = new UserHeroSkillData[Skill_Tab_Ui.Length];

        for (int i = 0; i < Skill_Tab_Ui.Length; i++)
        {
            var skill_group = Skill_Groups.FirstOrDefault(x => x.Group_Data.skill_type == skill_type_by_index[i]);
            TabController.GetTab(i).SetBlockTab(skill_group == null);

            Current_SkillGroups[i] = skill_group;
            Skill_Tab_Ui[i].Set(skill_group);
        }

        Exist_Exp_Items.Clear();
        // 유저가 소유하고 있는 경험치 아이템 가져오기
        for (int i = 0; i < 5; i++)
        {
            var data = (UserItem_NormalItemData)GameData.Instance.GetUserItemDataManager().FindUserItem(ITEM_TYPE_V2.EXP_SKILL, max_exp_item_id - i);
            Exist_Exp_Items.Add(data);
        }
        Use_Exp_Items.Clear();
        
        UpdateUI();
    }

    public override void UpdatePopup()
    {
        base.UpdatePopup();
        
        if (Selected_Index > -1)
        {
            if (Current_SkillGroup == null)
            {
                // TODO: M1 개발도중 아직 궁극기가 전부 구현되지 않았기 때문에 예외처리만 해준다.. 이후에는 Assert로 변경하든지 합시다.
                return;
            }

            UpdateUI();
        }
    }

    ERROR_CODE SetLevelupData(bool use_levelup_animation)
    {
        ERROR_CODE result_code = ERROR_CODE.SUCCESS;
        After_SkillGroup = Current_SkillGroup;

        double need_golds = 0;
        bool exist_enough_golds = true;
        double not_used_exp = 0;

        if (Use_Exp_Items.Count > 0)
        {
            result_code = Current_SkillGroup.SumExpItemInfo(out double items_total_exp, out need_golds, Use_Exp_Items);

            if (result_code != ERROR_CODE.SUCCESS)
            {
                Debug.Assert(false, $"잘못된 아이템 에러 : {result_code}");
                return result_code;
            }

            exist_enough_golds = GameData.Instance.GetUserGoodsDataManager().IsUsableGoodsCount(GOODS_TYPE.GOLD, need_golds);
            After_SkillGroup = Current_SkillGroup.GetAddedExpSkillGroup(items_total_exp, out result_code);

            if (After_SkillGroup == null)
            {
                Debug.Assert(false, $"스킬레벨업 에러 : {result_code}");
                return result_code;
            }

            var add_exp = (After_SkillGroup.GetExp() - Current_SkillGroup.GetExp());
            not_used_exp = items_total_exp - add_exp;

            Debug.Log($"not_used_exp : {not_used_exp}");
        }

        // 
        skillInfos[0].SetData(Current_SkillGroup);
        skillInfos[1].SetData(After_SkillGroup);

        // 경험치 텍스트 및 바
        double cur_exp = After_SkillGroup.IsMaxLevel() ? not_used_exp : After_SkillGroup.GetLevelExp();
        double next_exp = After_SkillGroup.GetNextExp();
        string exp_text = $"{cur_exp}/{next_exp}";
        exp_text = After_SkillGroup.IsMaxLevel() ? exp_text.WithColorTag(Color.red) : exp_text;
        Add_Exp_Text.text = exp_text;

        float exp_bar_val = After_SkillGroup.IsMaxLevel() ? (float)(not_used_exp / After_SkillGroup.GetNextExp()) : After_SkillGroup.GetExpPercentage();
        Add_Exp_Bar.value = exp_bar_val;

        Add_Exp_Bar.image.color = After_SkillGroup.IsMaxLevel() ? Color.red : Color.white;

        //골드 소모 텍스트
        string gold_string = ((int)need_golds).ToString("N0");
        Need_Gold_Text.text = (exist_enough_golds) ? gold_string : gold_string.WithColorTag(Color.red);

        // 버튼 활성/비활성화
        AutoSelect_Button.enabled = !Current_SkillGroup.IsMaxLevel();
        Up_Button.enabled = !Current_SkillGroup.IsMaxLevel() && exist_enough_golds;

        return result_code;
    }

    public ERROR_CODE OnChangedUseItemCount(int item_id, int count)
    {
        if (Use_Exp_Items.Exists(x => x.Item_ID == item_id && x.Use_Count < count)
            && After_SkillGroup.IsMaxLevel())
        {
            return ERROR_CODE.ALREADY_MAX_LEVEL;
        }

        Use_Exp_Items.RemoveAll(x => x.Item_ID == item_id);
        if (count > 0)
        {
            Use_Exp_Items.Add(CreateExpItem(item_id, count));
        }

        return UpdateUI();
    }

    public void OnSelected(Tab tab)
    {
        Selected_Index = TabController.GetTabIndex(tab);

        if (Skill_Groups == null)
        {
            return;
        }
        Use_Exp_Items.Clear();

        UpdateUI();
    }

    public void OnClickLevelup()
    {
        var result = Current_SkillGroup.AddExpUseItem(Use_Exp_Items);

        if (result.Code != ERROR_CODE.SUCCESS && result.Code != ERROR_CODE.LEVEL_UP_SUCCESS)
        {
            Debug.Assert(false, $"스킬 레벨업 에러 ERROR_CODE :{result.Code}");
            return;
        }

        GameData.Instance.GetUserHeroSkillDataManager().Save();
        GameData.Instance.GetUserGoodsDataManager().Save();

        Use_Exp_Items.Clear();
        UpdateUI(true);

        if (result.Code == ERROR_CODE.LEVEL_UP_SUCCESS)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
        }
    }

    public void OnClickAutoSelect()
    {
        List<USE_EXP_ITEM_DATA> use_item = new List<USE_EXP_ITEM_DATA>();

        double total_exp = 0;
        bool up_to_max_level = false;

        for (int i = 0; i < Exist_Exp_Items.Count; i++)
        {
            int item_id = Exist_Exp_Items[i].Item_ID;
            int count = 1;
            var use_item_copy = use_item.ToList();
            bool exist_enough_golds = true;

            while (exist_enough_golds && !up_to_max_level)
            {
                use_item_copy.RemoveAll(x => x.Item_ID == item_id);
                use_item_copy.Add(CreateExpItem(item_id, count));

                Current_SkillGroup.SumExpItemInfo(out double _total_exp, out double need_golds, use_item_copy);
                exist_enough_golds = GameData.Instance.GetUserGoodsDataManager().IsUsableGoodsCount(GOODS_TYPE.GOLD, need_golds);

                if (exist_enough_golds)
                {
                    var after_data = Current_SkillGroup.GetAddedExpSkillGroup(_total_exp, out ERROR_CODE result_code);
                    if (result_code != ERROR_CODE.SUCCESS && result_code != ERROR_CODE.LEVEL_UP_SUCCESS)
                    {
                        Debug.Assert(false, $"오류!! : {result_code}");

                        Debug.Log($"total_exp : {total_exp} : {up_to_max_level}");

                        foreach (var item in use_item)
                        {
                            Debug.Log($"item : {item.Item_ID} : {item.Use_Count}");
                        }

                        return;
                    }
                    up_to_max_level = after_data.IsMaxLevel();
                    total_exp = _total_exp;
                    use_item = use_item_copy.ToList();
                    count++;
                }
            }
        }

        /*
        Current_SkillGroup.SumExpItemInfo(out double exp, out double golds, use_item);
        Debug.Log($"exp : {exp}, gold : {golds}");
        */

        if (use_item.Count == 0)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
            {
                popup.ShowPopup(3f, "경험치 아이템이나 골드가 모자라서 선택할 수 없습니다");
            });
            return;
        }

        Use_Exp_Items = use_item;
        UpdateExpItemButtons();
    }

    USE_EXP_ITEM_DATA CreateExpItem(int item_id, int count)
    {
        return new USE_EXP_ITEM_DATA()
        {
            Item_ID = item_id,
            Item_Type = ITEM_TYPE_V2.EXP_SKILL,
            Use_Count = count,
        };
    }
}

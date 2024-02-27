using FluffyDuck.UI;
using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using FluffyDuck.Util;
using Cysharp.Text;
using System.Collections;

public class SkillLevelPopup : PopupBase
{
    [SerializeField, Tooltip("탭 컨트롤러")]
    TabController TabCtrl = null;

    [SerializeField, Tooltip("스킬 탭")]
    List<SkillInfoTab> Skill_Tab_Ui = null;

    [SerializeField, Tooltip("스킬 이름")]
    TMP_Text Skill_Name = null;

    [SerializeField, Tooltip("레벨업 이전과 이후의 스킬 정보")]
    SkillInfo[] skillInfos = null;

    [SerializeField, Tooltip("경험치 텍스트\'0/100\'")]
    TMP_Text Exp_Text = null;

    [SerializeField, Tooltip("경험치 바")]
    Slider Exp_Bar = null;

    [SerializeField, Tooltip("시뮬레이션 경험치 바")]
    Slider Result_Exp_Bar;

    [SerializeField, Tooltip("경험치 아이템")]
    
    List<UsableItemCard> Usable_Items;

    [SerializeField, Tooltip("사용 골드 수치 텍스트")]
    TMP_Text Need_Gold_Text = null;

    [SerializeField, Tooltip("자동선택 버튼")]
    UIButtonBase AutoSelect_Button = null;

    [SerializeField, Tooltip("레벨업 버튼")]
    UIButtonBase Up_Button = null;

    /// <summary>
    /// 스킬 데이터
    /// </summary>
    List<BattlePcSkillGroup> Battle_Skill_Groups = new List<BattlePcSkillGroup>();

    /// <summary>
    /// 사용 가능한 아이템 선택 정보
    /// </summary>
    List<USABLE_ITEM_DATA> Use_Exp_Items = new List<USABLE_ITEM_DATA>();

    //  시뮬레이션 결과
    EXP_SIMULATE_RESULT_DATA? Simulate_Result = null;

    /// <summary>
    /// 시뮬레이션 경험치 게이지
    /// </summary>
    Coroutine Simulate_Coroutine;
    /// <summary>
    /// 실제 경험치 게이지
    /// </summary>
    Coroutine Level_Up_Coroutine;


    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1 || data[0] is not IReadOnlyList<UserHeroSkillData>)
        {
            return false;
        }
        Simulate_Result = null;
        var user_skills = ((IReadOnlyList<UserHeroSkillData>)data[0]).ToList();
        Battle_Skill_Groups.Clear();
        List<SKILL_TYPE> skill_order = new List<SKILL_TYPE>();
        skill_order.Add(SKILL_TYPE.SPECIAL_SKILL);
        skill_order.Add(SKILL_TYPE.SKILL_01);
        skill_order.Add(SKILL_TYPE.SKILL_02);

        for (int i = 0; i < skill_order.Count; i++)
        {
            SKILL_TYPE stype = skill_order[i];
            var user_skill = user_skills.Find(x => x.GetSkillType() == stype);
            if (user_skill != null)
            {
                var battle_skill = new BattlePcSkillGroup(user_skill);
                battle_skill.SetSkillGroupID(user_skill.GetSkillGroupID());
                Battle_Skill_Groups.Add(battle_skill);
            }
        }
        SetUsableItemsCallback();

        UpdateTabUI();
        if (Battle_Skill_Groups.Exists(x => x.GetSkillType() == SKILL_TYPE.SPECIAL_SKILL))
        {
            TabCtrl.GetTab(0).SetBlockTab(false);
            TabCtrl.Select(0);
            
        }
        else
        {
            TabCtrl.GetTab(0).SetBlockTab(true);
            TabCtrl.Select(1);
        }
        
        return true;
    }

    void UpdateTabUI()
    {
        if (Battle_Skill_Groups.Count == 0)
        {
            return;
        }
        for (int i = 0; i < Battle_Skill_Groups.Count; i++)
        {
            var skill = Battle_Skill_Groups[i];
            var find_tab = Skill_Tab_Ui.Find(x => x.GetSkillType() == skill.GetSkillType());
            if (find_tab != null)
            {
                find_tab.SetBattlePcSkillGroup(skill);
            }
        }
    }

    void SetUsableItemsCallback()
    {
        var item_data_list = MasterDataManager.Instance.Get_ItemDataListByItemType(ITEM_TYPE_V2.EXP_SKILL);
        for (int i = 0; i < item_data_list.Count; i++)
        {
            var item_data = item_data_list[i];
            if (i < Usable_Items.Count)
            {
                Usable_Items[i].SetUserItemData(item_data.item_type, item_data.item_id, OnChangeUsableItemCount);
            }
        }
    }
    /// <summary>
    /// 경험치 아이템 추가 및 제거시 호출 함수
    /// </summary>
    /// <param name="is_add"></param>
    /// <param name="result_cb"></param>
    void OnChangeUsableItemCount(bool is_add, System.Action<bool> result_cb)
    {
        var skill = GetSelectedSkillGroup();
        if (skill == null)
        {
            result_cb?.Invoke(false);
            return;
        }

        if (skill.GetUserHeroSkillData().IsMaxLevel())
        {
            result_cb?.Invoke(false);
            ShowNoticePopup("최대 레벨에 도달했습니다.", 1.5f);
            return;
        }

        if (is_add)
        {
            //  더하기만 체크. 이전 시뮬레이션 정보에서 이미 최대레벨에 도달한 경우, 아이템 추가하지 않음.
            if (Simulate_Result != null && Simulate_Result.Value.Result_Lv >= skill.GetUserHeroSkillData().GetMaxLevel())
            {
                result_cb?.Invoke(false);
                return;
            }
        }

        RESPONSE_TYPE result = RESPONSE_TYPE.NOT_WORK;
        Use_Exp_Items.Clear();
        for (int i = 0; i < Usable_Items.Count; i++)
        {
            Use_Exp_Items.Add(Usable_Items[i].GetUsableItemData());
        }

        if (Use_Exp_Items.Count > 0)
        {
            var simulate = skill.GetUserHeroSkillData().GetCalcSimulateExp(Use_Exp_Items);
            result = simulate.Code;
            if (simulate.Code == RESPONSE_TYPE.SUCCESS || simulate.Code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
            {
                //  cur exp bar (시뮬레이션 결과가 현재 레벨보다 클 경우 현재 경험치 바는 숨겨준다)
                Exp_Bar.gameObject.SetActive(skill.GetUserHeroSkillData().GetLevel() == simulate.Result_Lv);

                //  레벨 상승이 있으면, 상승 레벨 결과를 보여준다.
                if (skill.GetSkillLevel() == simulate.Result_Lv)
                {
                    skillInfos[0].SetBattlePcSkillGroup(skill);
                    skillInfos[1].SetBattlePcSkillGroup(skill);
                }
                else
                {
                    if (Simulate_Result != null)
                    {
                        skillInfos[0].SetBattlePcSkillGroup(skill);
                        if (Simulate_Result.Value.Result_Lv < simulate.Result_Lv)
                        {
                            var clone = skill.GetCloneSimulateLevelUpData(Simulate_Result.Value.Result_Lv);
                            skillInfos[1].SetBattlePcSkillGroup(clone);
                        }
                        else
                        {
                            var clone = skill.GetCloneSimulateLevelUpData(simulate.Result_Lv);
                            skillInfos[1].SetBattlePcSkillGroup(clone);
                        }
                    }
                    else
                    {
                        skillInfos[0].SetBattlePcSkillGroup(skill);
                        skillInfos[1].SetBattlePcSkillGroup(skill);
                    }
                    
                }

                //  exp count
                var lv_data = MasterDataManager.Instance.Get_PlayerCharacterSkillLevelData(simulate.Result_Lv);
                double need_exp = lv_data.need_exp;
                double lv_exp = simulate.Result_Accum_Exp - lv_data.accum_exp;
                Exp_Text.text = ZString.Format("{0:N0} / {1:N0}", lv_exp, need_exp);

                //  필요 골드가 부족하면 빨간색
                if (GameData.Instance.GetUserGoodsDataManager().IsUsableGoodsCount(GOODS_TYPE.GOLD, simulate.Need_Gold))
                {
                    Need_Gold_Text.text = simulate.Need_Gold.ToString("N0");
                }
                else
                {
                    Need_Gold_Text.text = ZString.Format("<color=#ff0000>{0:N0}</color>", simulate.Need_Gold);
                }

                //  경험치가 최대 레벨이상으로 초과될 경우 알림을 해준다.
                if (simulate.Over_Exp > 0)
                {
                    string msg = $"경험치가 <color=#ff0000>{simulate.Over_Exp.ToString("N0")}</color> 초과되었습니다.";
                    ShowNoticePopup(msg, 1.5f);
                }

                if (is_add)
                {
                    if (Simulate_Coroutine != null)
                    {
                        StopCoroutine(Simulate_Coroutine);
                    }
                    EXP_SIMULATE_RESULT_DATA? before = Simulate_Result;
                    Simulate_Result = simulate;
                    //  coroutine
                    Simulate_Coroutine = StartCoroutine(StartSimulateLevelExpGaugeAnim(before, simulate));
                }
                else
                {
                    if (Simulate_Coroutine != null)
                    {
                        StopCoroutine(Simulate_Coroutine);
                    }
                    Simulate_Coroutine = null;
                    Simulate_Result = simulate;

                    //  exp bar
                    float per = (float)(lv_exp / need_exp);
                    Result_Exp_Bar.value = per;
                }

            }
            else
            {
                UpdatePopup();
            }
        }
        result_cb?.Invoke(true);
    }
    /// <summary>
    /// 경험치 아이템 사용 시뮬레이션 결과 보여주기
    /// </summary>
    /// <param name="before_simulate"></param>
    /// <param name="after_simulate"></param>
    /// <returns></returns>
    IEnumerator StartSimulateLevelExpGaugeAnim(EXP_SIMULATE_RESULT_DATA? before_simulate, EXP_SIMULATE_RESULT_DATA? after_simulate)
    {
        var m = MasterDataManager.Instance;
        var skill = GetSelectedSkillGroup();
        int before_lv = skill.GetSkillLevel();
        float before_exp_per = skill.GetUserHeroSkillData().GetExpPercentage();
        if (before_simulate != null)
        {
            before_lv = before_simulate.Value.Result_Lv;
            var lv_data = m.Get_PlayerCharacterSkillLevelData(before_lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = before_simulate.Value.Result_Accum_Exp - lv_data.accum_exp;
            before_exp_per = (float)(lv_exp / need_exp);
        }
        //  시작전 게이지바
        Result_Exp_Bar.value = before_exp_per;

        int gauge_full_count = after_simulate.Value.Result_Lv - before_lv;
        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  게이지 풀 횟수
        while (loop_count < gauge_full_count)
        {
            delta += Time.deltaTime * 4f;

            Result_Exp_Bar.value = Mathf.Clamp01(delta);
            if (delta >= duration)
            {
                delta = 0f;
                Result_Exp_Bar.value = 0f;
                ++loop_count;
                int lv = before_lv + loop_count;
                var clone = skill.GetCloneSimulateLevelUpData(lv);
                skillInfos[1].SetBattlePcSkillGroup(clone);
                if (loop_count >= gauge_full_count)
                {
                    break;
                }
            }
            else
            {
                yield return wait;
            }
        }

        //  남은 경험치 게이지 이동
        delta = 0f;
        float after_exp_per = 0f;
        {
            var lv_data = m.Get_PlayerCharacterSkillLevelData(after_simulate.Value.Result_Lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = after_simulate.Value.Result_Accum_Exp - lv_data.accum_exp;
            after_exp_per = (float)(lv_exp / need_exp);
        }

        duration = 1f;
        if (after_exp_per > 0f)
        {
            while (true)
            {
                delta += Time.deltaTime;
                Result_Exp_Bar.value = Mathf.MoveTowards(Result_Exp_Bar.value, after_exp_per, duration * Time.deltaTime);
                if (delta >= duration)
                {
                    Result_Exp_Bar.value = after_exp_per;
                    break;
                }
                else 
                {
                    yield return wait;
                }
            }
        }
        Simulate_Coroutine = null;
    }
    /// <summary>
    /// 경험치 아이템 사용 결과 보여주기
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    IEnumerator StartLevelUpExpGaugeAnim(USE_EXP_ITEM_RESULT_DATA result)
    {
        var m = MasterDataManager.Instance;
        var skill = GetSelectedSkillGroup();
        UpdateExpItemButtons();

        Exp_Bar.gameObject.SetActive(true);
        Result_Exp_Bar.value = 0f;

        int before_lv = result.Before_Lv;
        float before_exp_per = 0f;
        {
            var lv_data = m.Get_PlayerCharacterSkillLevelData(before_lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = result.Before_Accum_Exp - lv_data.accum_exp;
            before_exp_per = (float)(lv_exp / need_exp);
        }
        //  강화전 초기값
        {
            var clone = skill.GetCloneSimulateLevelUpData(before_lv);
            skillInfos[1].SetBattlePcSkillGroup(clone);
        }
        Exp_Bar.value = before_exp_per;

        int gauge_full_count = result.Result_Lv - before_lv;
        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  게이지 풀 횟수
        while (loop_count < gauge_full_count)
        {
            delta += Time.deltaTime * 4f;
            Exp_Bar.value = Mathf.Clamp01(delta);
            if (delta >= duration)
            {
                delta = 0f;
                Exp_Bar.value = 0f;
                ++loop_count;
                int lv = before_lv + loop_count;
                var clone = skill.GetCloneSimulateLevelUpData(lv);
                skillInfos[1].SetBattlePcSkillGroup(clone);
                if (loop_count >= gauge_full_count)
                {
                    break;
                }
            }
            else
            {
                yield return wait;
            }
        }

        //  남은 경험치 게이지 이동
        delta = 0f;
        float after_exp_per = 0f;
        {
            var lv_data = m.Get_PlayerCharacterSkillLevelData(result.Result_Lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = result.Result_Accum_Exp - lv_data.accum_exp;
            after_exp_per = (float)(lv_exp / need_exp);
        }
        duration = 1f;
        if (after_exp_per > 0f)
        {
            while (true)
            {
                delta += Time.deltaTime;
                Exp_Bar.value = Mathf.MoveTowards(Exp_Bar.value, after_exp_per, duration * Time.deltaTime);
                if (delta >= duration)
                {
                    Exp_Bar.value = after_exp_per;
                    break;
                }
                else
                {
                    yield return wait;
                }
            }
        }

        //  레벨업 팝업
        if (result.Code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
        }
        Level_Up_Coroutine = null;
        Simulate_Result = null;
        UpdatePopup();
    }

    void UpdateExpItemButtons()
    {
        if (Battle_Skill_Groups.Count == 0)
        {
            return;
        }
        int cnt = Usable_Items.Count;
        for (int i = 0; i < cnt; i++)
        {
            Usable_Items[i].ResetUsableCount();
        }
    }

    protected override void FixedUpdatePopup()
    {
        if (Battle_Skill_Groups.Count == 0)
        {
            return;
        }
        if (!Battle_Skill_Groups.Exists(x => x.GetSkillType() == SKILL_TYPE.SPECIAL_SKILL))
        {
            TabCtrl.GetTab(0).SetBlockTab(true);
        }
        TabCtrl.GetTab(3).SetBlockTab(true);

        //for (int i = 0; i < Battle_Skill_Groups.Count; i++)
        //{
        //    var skill = Battle_Skill_Groups[i];
        //    var find_tab = Skill_Tab_Ui.Find(x => x.GetSkillType() == skill.GetSkillType());
        //    if (find_tab != null)
        //    {
        //        find_tab.SetBattlePcSkillGroup(skill);
        //    }
        //}
        
    }

    BattlePcSkillGroup GetSelectedSkillGroup()
    {
        int selected_idx = TabCtrl.GetSelectedIndex();
        var tab = Skill_Tab_Ui[selected_idx];
        var skill = Battle_Skill_Groups.Find(x => x.GetSkillType() == tab.GetSkillType());
        return skill;
    }

    public override void UpdatePopup()
    {
        if (Battle_Skill_Groups.Count == 0)
        {
            return;
        }

        var skill = GetSelectedSkillGroup();
        if (skill == null)
        {
            Exp_Text.text = "0/0";
            Exp_Bar.value = 0;
            Result_Exp_Bar.value = 0;
            for (int i = 0; i < skillInfos.Length; i++)
            {
                skillInfos[i].SetBattlePcSkillGroup(null);
            }
            return;
        }

        //  skill name
        Skill_Name.text = skill.GetSkillActionName();

        //  current skill info
        skillInfos[0].SetBattlePcSkillGroup(skill);
        //  next skill info
        if (Simulate_Result != null)
        {
            var clone = skill.GetCloneSimulateLevelUpData(Simulate_Result.Value.Result_Lv);
            skillInfos[1].SetBattlePcSkillGroup(clone);

            if (GameData.Instance.GetUserGoodsDataManager().IsUsableGoodsCount(GOODS_TYPE.GOLD, Simulate_Result.Value.Need_Gold))
            {
                Need_Gold_Text.text = Simulate_Result.Value.Need_Gold.ToString("N0");
            }
            else
            {
                Need_Gold_Text.text = ZString.Format("<color=#ff0000>{0:N0}</color>", Simulate_Result.Value.Need_Gold);
            }
        }
        else
        {
            skillInfos[1].SetBattlePcSkillGroup(skill);

            Need_Gold_Text.text = "0";
        }

        //  skill exp & gauge
        double cur_exp = skill.GetUserHeroSkillData().GetLevelExp();
        double next_exp = skill.GetUserHeroSkillData().GetNextExp();
        float per = skill.GetUserHeroSkillData().GetExpPercentage();

        Exp_Text.text = ZString.Format("{0:N0}/{1:N0}", cur_exp, next_exp);
        Exp_Bar.value = per;
        Result_Exp_Bar.value = per;


        
    }

    

    public void OnSelected(Tab tab)
    {
        Use_Exp_Items.Clear();
        Simulate_Result = null;
        FixedUpdatePopup();
        UpdatePopup();
        UpdateExpItemButtons();
    }

    public void OnClickAutoSelect()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");

        var skill = GetSelectedSkillGroup();
        if (skill != null)
        {
            var result = skill.GetUserHeroSkillData().GetAutoSimulateExp();
            for (int i = 0; i < result.Auto_Item_Data_List.Count; i++)
            {
                var data = result.Auto_Item_Data_List[i];
                var slot = Usable_Items.Find(x => x.GetItemType() == data.Item_Type && x.GetItemID() == data.Item_ID);
                if (slot != null)
                {
                    slot.SetUsableCount(data.Use_Count);
                }
            }

            UpdateAutoSimulateResult(result);
        }
    }

    void UpdateAutoSimulateResult(EXP_SIMULATE_RESULT_DATA simulate)
    {
        if (simulate.Code == RESPONSE_TYPE.SUCCESS || simulate.Code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
        {
            var skill = GetSelectedSkillGroup();
            //  cur exp bar (시뮬레이션 결과가 현재 레벨보다 클 경우 현재 경험치 바는 숨겨준다)
            Exp_Bar.gameObject.SetActive(skill.GetUserHeroSkillData().GetLevel() == simulate.Result_Lv);

            //  레벨 상승이 있으면, 상승 레벨 결과를 보여준다.
            if (skill.GetSkillLevel() == simulate.Result_Lv)
            {
                skillInfos[0].SetBattlePcSkillGroup(skill);
                skillInfos[1].SetBattlePcSkillGroup(skill);
            }
            else
            {
                if (Simulate_Result != null)
                {
                    skillInfos[0].SetBattlePcSkillGroup(skill);
                    if (Simulate_Result.Value.Result_Lv < simulate.Result_Lv)
                    {
                        var clone = skill.GetCloneSimulateLevelUpData(Simulate_Result.Value.Result_Lv);
                        skillInfos[1].SetBattlePcSkillGroup(clone);
                    }
                    else
                    {
                        var clone = skill.GetCloneSimulateLevelUpData(simulate.Result_Lv);
                        skillInfos[1].SetBattlePcSkillGroup(clone);
                    }
                }
                else
                {
                    var clone = skill.GetCloneSimulateLevelUpData(simulate.Result_Lv);
                    skillInfos[1].SetBattlePcSkillGroup(clone);
                }

            }

            //  exp count
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterSkillLevelData(simulate.Result_Lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = simulate.Result_Accum_Exp - lv_data.accum_exp;
            Exp_Text.text = ZString.Format("{0:N0} / {1:N0}", lv_exp, need_exp);

            //  필요 골드가 부족하면 빨간색
            if (GameData.Instance.GetUserGoodsDataManager().IsUsableGoodsCount(GOODS_TYPE.GOLD, simulate.Need_Gold))
            {
                Need_Gold_Text.text = simulate.Need_Gold.ToString("N0");
            }
            else
            {
                Need_Gold_Text.text = ZString.Format("<color=#ff0000>{0:N0}</color>", simulate.Need_Gold);
            }

            //  경험치가 최대 레벨이상으로 초과될 경우 알림을 해준다.
            if (simulate.Over_Exp > 0)
            {
                string msg = $"경험치가 <color=#ff0000>{simulate.Over_Exp.ToString("N0")}</color> 초과되었습니다.";
                ShowNoticePopup(msg, 1.5f);
            }

            if (Simulate_Coroutine != null)
            {
                StopCoroutine(Simulate_Coroutine);
            }
            Simulate_Coroutine = null;
            Simulate_Result = simulate;

            //  exp bar
            float per = (float)(lv_exp / need_exp);
            Result_Exp_Bar.value = per;


        }
        else
        {
            UpdatePopup();
        }
    }

    public void OnClickLevelUp()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Use_Exp_Items.Clear();
        for (int i = 0; i < Usable_Items.Count; i++)
        {
            Use_Exp_Items.Add(Usable_Items[i].GetUsableItemData());
        }
        int sum = Use_Exp_Items.Sum(x => x.Use_Count);
        if (sum == 0)
        {
            ShowNoticePopup("경험치 아이템을 선택해 주세요.", 1.5f);
            return;
        }
        var skill = GetSelectedSkillGroup();
        if (skill != null)
        {
            skill.GetUserHeroSkillData().AddExpUseItem(OnResponseLevelUp, Use_Exp_Items);
        }
    }
    /// <summary>
    /// 스킬 레벨업 결과
    /// </summary>
    /// <param name="result"></param>
    void OnResponseLevelUp(USE_EXP_ITEM_RESULT_DATA result)
    {
        if (result.Code != RESPONSE_TYPE.SUCCESS && result.Code != RESPONSE_TYPE.LEVEL_UP_SUCCESS)
        {
            if (result.Code == RESPONSE_TYPE.NOT_ENOUGH_GOLD)
            {
                ShowNoticePopup("골드가 부족합니다.", 1.5f);
            }
            else if (result.Code == RESPONSE_TYPE.NOT_ENOUGH_ITEM)
            {
                ShowNoticePopup("경험치 아이템이 부족합니다.", 1.5f);
            }
            return;
        }

        var gd = GameData.Instance;
        gd.GetUserHeroSkillDataManager().Save();
        gd.GetUserGoodsDataManager().Save();
        gd.GetUserItemDataManager().Save();
        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_GOLD);

        Use_Exp_Items.Clear();
        if (Level_Up_Coroutine != null)
        {
            StopCoroutine(Level_Up_Coroutine);
        }
        if (Simulate_Coroutine != null)
        {
            StopCoroutine(Simulate_Coroutine);
        }
        Simulate_Coroutine = null;
        Level_Up_Coroutine = StartCoroutine(StartLevelUpExpGaugeAnim(result));
    }


    void ShowNoticePopup(string msg, float duration)
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(duration, msg);
        });

    }

}

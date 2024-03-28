using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoBoxLevelUp : MonoBehaviour
{
    [SerializeField]
    TMP_Text Level_Text = null;

    [SerializeField, Tooltip("Result Level Box")]
    RectTransform Result_Level_Box;

    [SerializeField]
    TMP_Text Result_Level_Text = null;

    [SerializeField]
    Slider Current_Exp_Bar = null;

    [SerializeField, Tooltip("Result Exp Bar")]
    Slider Result_Exp_Bar;

    [SerializeField, Tooltip("Exp Count")]
    TMP_Text Exp_Count;

    [SerializeField, Tooltip("영웅 스탯 정보")]
    List<HeroStatusInfoNode> Status_Info_Nodes;

    [SerializeField, Tooltip("사용 가능한 아이템(경험치 아이템)")]
    List<UsableItemCard> Usable_Items;

    [SerializeField, Tooltip("사용 골드 수치 텍스트")]
    TMP_Text Need_Gold_Text = null;

    [SerializeField, Tooltip("자동선택 버튼")]
    UIButtonBase AutoSelect_Button = null;

    [SerializeField, Tooltip("레벨업 버튼")]
    UIButtonBase Up_Button = null;

    BattlePcData Unit_Data = null;
    List<USABLE_ITEM_DATA> Use_Exp_Items = new List<USABLE_ITEM_DATA>();

    EXP_SIMULATE_RESULT_DATA? Simulate_Result = null;

    Coroutine Simulate_Coroutine;
    Coroutine Level_Up_Coroutine;


    public void SetHeroData(BattlePcData data)
    {
        Unit_Data = data;
        Simulate_Result = null;
        FixedUpdatePopup();
    }

    public void FixedUpdatePopup()
    {
        var item_data_list = MasterDataManager.Instance.Get_ItemDataListByItemType(ITEM_TYPE_V2.EXP_POTION_C);
        for (int i = 0; i < item_data_list.Count; i++)
        {
            var item_data = item_data_list[i];
            if (i < Usable_Items.Count)
            {
                Usable_Items[i].SetUserItemData(item_data.item_type, item_data.item_id, OnChangeUsableItemCount);
            }
        }

        Refresh();
    }

    void Refresh()
    {
        if (Unit_Data == null)
        {
            return;
        }

        Simulate_Result = null;
        Use_Exp_Items.Clear();
        UpdateExpItemButtons();
        UpdateHeroStatusInfoNodes(Unit_Data.GetLevel());
        UpdateUI();
    }

    void UpdateUI()
    {
        Level_Text.text = Unit_Data.GetLevel().ToString();

        //  result level
        Result_Level_Box.gameObject.SetActive(false);

        //  exp bar
        float per = Unit_Data.User_Data.GetExpPercetage();

        Current_Exp_Bar.value = per;
        Result_Exp_Bar.value = per;

        //  exp count
        double cur_exp = Unit_Data.User_Data.GetLevelExp();
        double next_exp = Unit_Data.User_Data.GetNextExp();
        Exp_Count.text = ZString.Format("{0:N0} / {1:N0}", cur_exp, next_exp);

        //  need gold init
        Need_Gold_Text.text = "0";
    }

    /// <summary>
    /// 경험치 아이템 버튼 업데이트<br />
    /// 경험치 아이템 버튼을 눌렀을때 업데이트 시켜주면 순환 호출을 하기 때문에<br />
    /// UpdateUI에서는 별도로 분리
    /// </summary>
    void UpdateExpItemButtons()
    {
        int cnt = Usable_Items.Count;
        for (int i = 0; i < cnt; i++)
        {
            Usable_Items[i].ResetUsableCount();
        }
    }

    void UpdateHeroStatusInfoNodes(int next_lv)
    {
        int cnt = Status_Info_Nodes.Count;
        BattlePcData clone_unit_data = clone_unit_data = Unit_Data.GetSimulateLevelUpData(next_lv);
        
        for (int i = 0; i < cnt; i++)
        {
            Status_Info_Nodes[i].SetBattleUnitData(Unit_Data, clone_unit_data);
        }
    }


    public void OnSelectedTab(Gpm.Ui.Tab tab)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Refresh();
    }
    public void OnClickAutoSelect()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        var result = Unit_Data.User_Data.GetAutoSimulateExp();
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

        Unit_Data.User_Data.AddExpUseItem(OnResponseLevelup, Use_Exp_Items);
    }
    /// <summary>
    /// 캐릭터 레벨업 결과
    /// </summary>
    /// <param name="result"></param>
    void OnResponseLevelup(USE_EXP_ITEM_RESULT_DATA result)
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


        GameData.Instance.GetUserHeroDataManager().Save();
        GameData.Instance.GetUserGoodsDataManager().Save();
        GameData.Instance.GetUserItemDataManager().Save();
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

    void OnChangeUsableItemCount(bool is_add, System.Action<bool> result_cb)
    {
        if (Unit_Data.User_Data.IsMaxLevel())
        {
            result_cb?.Invoke(false);
            ShowNoticePopup("최대 레벨에 도달했습니다.", 1.5f);
            return;
        }
        if (is_add)
        {
            //  더하기만 체크. 이전 시뮬레이션 정보에서 이미 최대레벨에 도달한 경우, 아이템 추가하지 않음
            if (Simulate_Result != null && Simulate_Result.Value.Result_Lv >= Unit_Data.User_Data.GetMaxLevel())
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
            var simulate = Unit_Data.User_Data.GetCalcSimulateExp(Use_Exp_Items);
            result = simulate.Code;
            if (simulate.Code == RESPONSE_TYPE.SUCCESS || simulate.Code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
            {
                //  current exp bar(시뮬레이션 결과가 현재 레벨보다 클 경우 현재 경험치 바는 숨겨준다)
                Current_Exp_Bar.gameObject.SetActive(Unit_Data.User_Data.GetLevel() == simulate.Result_Lv);
                //  레벨 상승이 있으면, 상승 결과 레벨을 보여준다.
                Result_Level_Box.gameObject.SetActive(Unit_Data.User_Data.GetLevel() != simulate.Result_Lv);
                //  next level
                //Result_Level_Text.text = Simulate_Result.Value.Result_Lv.ToString();
                //  exp bar
                var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(simulate.Result_Lv);
                double need_exp = lv_data.need_exp;
                double lv_exp = simulate.Result_Accum_Exp - lv_data.accum_exp;
                //float per = (float)(lv_exp / need_exp);
                //Result_Exp_Bar.value = per;
                //  exp count
                Exp_Count.text = ZString.Format("{0:N0} / {1:N0}", lv_exp, need_exp);

                //  need gold (골드가 부족하면 빨간색)
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
                    string msg = ZString.Format(GameDefine.GetLocalizeString("system_alert_exp_over"), simulate.Over_Exp.ToString("N0"));
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
                    //  next level
                    Result_Level_Text.text = Simulate_Result.Value.Result_Lv.ToString();
                    //  exp bar
                    float per = (float)(lv_exp / need_exp);
                    Result_Exp_Bar.value = per;

                }

            }
            else
            {
                UpdateUI();
            }
        }
        
        result_cb?.Invoke(true);
    }

    void UpdateAutoSimulateResult(EXP_SIMULATE_RESULT_DATA simulate)
    {
        if (simulate.Code == RESPONSE_TYPE.SUCCESS || simulate.Code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
        {
            //  current exp bar(시뮬레이션 결과가 현재 레벨보다 클 경우 현재 경험치 바는 숨겨준다)
            Current_Exp_Bar.gameObject.SetActive(Unit_Data.User_Data.GetLevel() == simulate.Result_Lv);
            //  레벨 상승이 있으면, 상승 결과 레벨을 보여준다.
            Result_Level_Box.gameObject.SetActive(Unit_Data.User_Data.GetLevel() != simulate.Result_Lv);
            //  exp bar
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(simulate.Result_Lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = simulate.Result_Accum_Exp - lv_data.accum_exp - simulate.Over_Exp;
            //  exp count
            Exp_Count.text = ZString.Format("{0:N0} / {1:N0}", lv_exp, need_exp);

            //  need gold (골드가 부족하면 빨간색)
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
                string msg = ZString.Format(GameDefine.GetLocalizeString("system_alert_exp_over"), simulate.Over_Exp.ToString("N0"));
                ShowNoticePopup(msg, 1.5f);
            }

            if (Simulate_Coroutine != null)
            {
                StopCoroutine(Simulate_Coroutine);
            }
            Simulate_Coroutine = null;
            Simulate_Result = simulate;
            //  next level
            Result_Level_Text.text = Simulate_Result.Value.Result_Lv.ToString();
            //  exp bar
            float per = (float)(lv_exp / need_exp);
            Result_Exp_Bar.value = per;
        }
        else
        {
            UpdateUI();
        }
    }

    IEnumerator StartSimulateLevelExpGaugeAnim(EXP_SIMULATE_RESULT_DATA? before_simulate, EXP_SIMULATE_RESULT_DATA? after_simulate)
    {
        int before_lv = Unit_Data.User_Data.GetLevel();
        float before_exp_per = Unit_Data.User_Data.GetExpPercetage();
        
        if (before_simulate != null)
        {
            before_lv = before_simulate.Value.Result_Lv;
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(before_lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = before_simulate.Value.Result_Accum_Exp - lv_data.accum_exp;
            before_exp_per = (float)(lv_exp / need_exp);
        }
        
        Result_Level_Text.text = before_lv.ToString();
        Result_Exp_Bar.value = before_exp_per;

        int gauge_full_count = after_simulate.Value.Result_Lv - before_lv;
        if (gauge_full_count > 1)
        {
            gauge_full_count = 1;
        }
        float speed = 3f;
        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  게이지 풀 횟수
        while (loop_count < gauge_full_count)
        {
            delta += Time.deltaTime * 10f;

            Result_Exp_Bar.value = Mathf.Clamp01(delta);
            if (delta >= duration)
            {
                delta = 0f;
                Result_Exp_Bar.value = 0f;
                ++loop_count;
                //int lv = before_lv + loop_count;
                //Result_Level_Text.text = lv.ToString();
                //UpdateHeroStatusInfoNodes(lv);

                Result_Level_Text.text = after_simulate.Value.Result_Lv.ToString();
                UpdateHeroStatusInfoNodes(after_simulate.Value.Result_Lv);
                
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
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(after_simulate.Value.Result_Lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = after_simulate.Value.Result_Accum_Exp - lv_data.accum_exp;
            after_exp_per = (float)(lv_exp / need_exp);
        }

        duration = 1f;
        if (after_exp_per > 0f)
        {
            while (true)
            {
                delta += Time.deltaTime * 5f;
                Result_Exp_Bar.value = Mathf.MoveTowards(Result_Exp_Bar.value, after_exp_per, duration * Time.deltaTime * speed);
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
        UpdateExpItemButtons();

        Current_Exp_Bar.gameObject.SetActive(true);
        Result_Exp_Bar.value = 0f;

        int before_lv = result.Before_Lv;
        float before_exp_per = 0f;
        {
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(result.Before_Lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = result.Before_Accum_Exp - lv_data.accum_exp;
            before_exp_per = (float)(lv_exp / need_exp);
        }

        Result_Level_Text.text = before_lv.ToString();
        Current_Exp_Bar.value = before_exp_per;

        int gauge_full_count = result.Result_Lv - before_lv;
        if (gauge_full_count > 1)
        {
            gauge_full_count = 1;
        }
        float speed = 1f;
        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  게이지 풀 횟수
        while (loop_count < gauge_full_count)
        {
            delta += Time.deltaTime * 10f;

            Current_Exp_Bar.value = Mathf.Clamp01(delta);

            if (delta >= duration)
            {
                delta = 0f;
                Current_Exp_Bar.value = 0f;
                ++loop_count;
                Result_Level_Text.text = result.Result_Lv.ToString();
                UpdateHeroStatusInfoNodes(result.Result_Lv);
                
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
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(result.Result_Lv);
            double need_exp = lv_data.need_exp;
            double lv_exp = result.Result_Accum_Exp - lv_data.accum_exp;
            after_exp_per = (float)(lv_exp / need_exp);
        }
        duration = 1f;
        if (after_exp_per > 0f)
        {
            while (true)
            {
                delta += Time.deltaTime * 5f;
                Current_Exp_Bar.value = Mathf.MoveTowards(Current_Exp_Bar.value, after_exp_per, duration * Time.deltaTime * speed);
                if (delta >= duration)
                {
                    Current_Exp_Bar.value = after_exp_per;
                    break;
                }
                else
                {
                    yield return wait;
                }
            }
        }
        
        UpdateUI();

        if (result.Code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
        }
        
        Level_Up_Coroutine = null;
        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_HERO_DETAIL_INFO);
    }
    

    void ShowNoticePopup(string msg, float duration)
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(duration, msg);
        });

    }

}

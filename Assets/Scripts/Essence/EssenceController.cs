using Cysharp.Text;
using Cysharp.Threading.Tasks;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZeroOne.Input;

public class EssenceController : SceneControllerBase
{
    [SerializeField, Tooltip("애니메이션 재생시에 안보이게 가릴 UI")]
    CanvasGroup HideableUI = null;

    [SerializeField, Tooltip("성공 파티클")]
    ParticleSystem Success_Effect = null;

    [SerializeField, Tooltip("절정 파티클")]
    ParticleSystem Climax_Effect = null;

    [SerializeField, Tooltip("대사창")]
    SerifuBox Serifu_Box = null;

    [SerializeField, Tooltip("게이지 바")]
    Slider Essence_Charge = null;

    [SerializeField, Tooltip("하단 팁 내용")]
    GameObject Tip_BG = null;

    [SerializeField, Tooltip("게이지 플러스 텍스트")]
    TMP_Text Essence_Charge_Plus_Text = null;

    [SerializeField, Tooltip("오늘 시도 가능한 횟수")]
    TMP_Text Essence_Chance_Count = null;

    [SerializeField, Tooltip("사각형?")]
    Animator Square = null;

    List<UserL2dData> L2d_List = null;
    Producer pd = null;

    BattlePcData Battle_Pc_Data = null;
    LOVE_LEVEL_TYPE Selected_Relationship;
    int Remain_Count = 0;
    private CancellationTokenSource Token_MoveToTargetAlpha = null;

    // TODO: 하드코딩 근원전달 강제 플로우
    Queue<TOUCH_BODY_TYPE>[] Essence_Force_Flow = new Queue<TOUCH_BODY_TYPE>[]
    {
        new Queue<TOUCH_BODY_TYPE>(new List<TOUCH_BODY_TYPE> // 처음
        {
            TOUCH_BODY_TYPE.PART2, // 얼굴
            TOUCH_BODY_TYPE.PART4, // 사타구니
            TOUCH_BODY_TYPE.PART3, // 가슴
        }),
        new Queue<TOUCH_BODY_TYPE>(new List<TOUCH_BODY_TYPE> // 두번째
        {
            TOUCH_BODY_TYPE.PART1, // 머리
            TOUCH_BODY_TYPE.PART3, // 가슴
            TOUCH_BODY_TYPE.PART4, // 사타구니
        })
    };

    int Essence_Force_Flow_Index { get { if (Remain_Count == 2) return 0; else if (Remain_Count == 1) return 1; return -1; }}

    protected override void Initialize()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup");
        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);

        Screen.orientation = ScreenOrientation.Portrait;
        Essence_Charge_Plus_Text.alpha = 0;

        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/success");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/DM-CGS-26");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/DM-CGS-45");

        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        L2d_List = GameData.I.GetUserL2DDataManager().GetUserL2dDataListByChoice();

        if (L2d_List.Count == 0)
        {
            string msg = "정상적으로 데이터가 설정되지 않았습니다.\nPersistent 데이터를 지워주세요";
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
            {
                popup.ShowPopup(3f, msg);
            });

            SCManager.Instance.SetCurrent(this);
            return;
        }
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt == total_cnt)
        {
            SCManager.I.SetCurrent(this, "OnReceiveData");
            ((PopupContainer)PopupManager.I.Container).SetEtcCanvasScaler(0);
            TouchCanvas.Instance.EnableTouchEffect(false);
            return;
        }
    }

    async UniTask _UpdateSlider(Slider slider, TMP_Text text, float value, float duration = 0.5f)
    {
        float elapsed_time = 0f;
        float origin = slider.value;
        int gap = (int)((value * 100) - (origin * 100));
        if (gap > 0) AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/DM-CGS-26");

        text.text = ZString.Format(GameDefine.GetLocalizeString("system_plus_format"), gap.ToString("N0"));
        text.alpha = 0;
        float text_origin_y = text.rectTransform.anchoredPosition.y;
        float text_dest_y = text.rectTransform.anchoredPosition.y + 50;

        while (elapsed_time < duration)
        {
            await UniTask.Yield(); 
            elapsed_time += Time.deltaTime;
            float multiple = elapsed_time / duration;
            slider.value = Mathf.Lerp(origin, value, multiple);
            if (gap > 0)
            {
                text.alpha = (multiple > 0.5f) ? (1 - multiple) * 2.0f : multiple * 2.0f;
                text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, Mathf.Lerp(text.rectTransform.anchoredPosition.y, text_dest_y, multiple));
            }
        }

        text.alpha = 0;
        text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, text_origin_y);
        slider.value = value;
    }

    bool OnReceiveData(BattlePcData data, int index, int remain_count_of_chance_send_essence)
    {
        Battle_Pc_Data = data;
        Selected_Relationship = (LOVE_LEVEL_TYPE)(index + 1);// 인덱스 + 1 = 실제 호감도
        Remain_Count = remain_count_of_chance_send_essence;

        pd = Factory.Instantiate<Producer>(Battle_Pc_Data.Data.essence_id, Selected_Relationship, SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE);
        pd.OnResultTransferEssence += OnResultTransferEssence;
        pd.OnCompleteTransferEssence += OnCompleteTransferEssence;
        pd.OnSendActorMessage += Serifu_Box.OnReceiveSpineMessage;

        UpdateUI();

        //TODO:하드코딩된 부분은 나중에 제외하기
        /*
        if (Remain_Count > 0)
        {
            pd.SetEssenceBodyPart();
        }
        */

        GestureManager.Instance.Enable = true;

        return true;
    }

    void UpdateUI()
    {
        Essence_Chance_Count.text = $"{GameDefine.SENDING_ESSENCE_CHANCE_COUNT_OF_DATE - Battle_Pc_Data.User_Data.Essence_Sended_Count_Of_Date}/{GameDefine.SENDING_ESSENCE_CHANCE_COUNT_OF_DATE}";
    }

    public void OnResultTransferEssence(bool is_success, TOUCH_BODY_TYPE type)
    {
        //TODO: 원래라면 실패라서 아무것도 안하지만 M2용 근원전달 플로우를 위해서 존재하는 코드
        if (Remain_Count > 0)
        {
            if (Essence_Force_Flow[Essence_Force_Flow_Index].Peek().Equals(type))
            {
                Essence_Force_Flow[Essence_Force_Flow_Index].Dequeue();
                if (!is_success)
                {
                    _ = _UpdateSlider(Essence_Charge, Essence_Charge_Plus_Text, (3f - Essence_Force_Flow[Essence_Force_Flow_Index].Count) / 3);
                }

                TouchCanvas.Instance.SetTouchEffectPrefabPath(TouchCanvas.Effect_Pink_Path);
            }
            else
            {
                TouchCanvas.Instance.SetTouchEffectPrefabPath(TouchCanvas.Effect_Blue_Path);
            }

            if (!is_success && Essence_Force_Flow[Essence_Force_Flow_Index].Count > 0)
            {
                if (Essence_Force_Flow[Essence_Force_Flow_Index].Count == 1)
                {
                    pd.SetEssenceBodyPart(Essence_Force_Flow[Essence_Force_Flow_Index].Peek());
                }
            }
        }
        else
        {
            TouchCanvas.Instance.SetTouchEffectPrefabPath(TouchCanvas.Effect_Blue_Path);
        }

        if (!is_success)
        {
            return;
        }

        Token_MoveToTargetAlpha?.Cancel();
        Token_MoveToTargetAlpha = new CancellationTokenSource();

        MoveToTargetAlpha(Token_MoveToTargetAlpha.Token, HideableUI, 0).Forget();

        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/success");

        Success_Effect.Play(true);
        Battle_Pc_Data.User_Data.SetDataSendedEssence(type);

        var charge_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);
        charge_item.UseChargeItem(1);

        GameData.Instance.GetUserChargeItemDataManager().Save();
        GameData.Instance.GetUserHeroDataManager().Save();

        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ESSESNCE);
        UpdateUI();
    }

    public void OnCompleteTransferEssence()
    {
        Climax_Effect.Play(true);
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/DM-CGS-45");

        if (Remain_Count-1 > 0)
        {
            _ = _UpdateSlider(Essence_Charge, Essence_Charge_Plus_Text, (3f - Essence_Force_Flow[Essence_Force_Flow_Index].Count) / 3, 1.0f);
            
        }
        else
        {
            Essence_Charge.transform.parent.gameObject.SetActive(false);
            Tip_BG.SetActive(false);
        }

        Remain_Count--;
        MoveToTargetAlpha(Token_MoveToTargetAlpha.Token, HideableUI, 1).Forget();
        _ = BlockInputWhenEndingClimaxEffect();

        Token_MoveToTargetAlpha?.Cancel();
        Token_MoveToTargetAlpha = new CancellationTokenSource();
    }

    async UniTask BlockInputWhenEndingClimaxEffect()
    {
        GestureManager.Instance.Enable = false;
        ScreenEffectManager.I.SetBlockInputUI(true);
        await UniTask.WaitUntil(() => !Climax_Effect.isPlaying);
        GestureManager.Instance.Enable = true;
        ScreenEffectManager.I.SetBlockInputUI(false);

        var charge_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);
        
        /*
        if (Remain_Count == 0)
        {
            // 오늘 이 캐릭터에게 전달할수 있는 찬스 전부 소진
            GestureManager.Instance.Enable = false;
            var select_popup = (SelectPopup)PopupManager.I.AddFromResources("Prefabs/Popup/Modal/SelectPopup");
            select_popup.SetVerticalScreen();
            select_popup.ShowPopup(GameDefine.GetLocalizeString("system_information"), GameDefine.GetLocalizeString("system_notice_complete_sending_essence"), GameDefine.GetLocalizeString("system_answer_ok"), (Action<int>)OnClickNoticePopup);
        }
        else if (charge_item.GetCount() == 0)
        {
            // 남은 근원전달 재화가 0
            GestureManager.Instance.Enable = false;
            var select_popup = (SelectPopup)PopupManager.I.AddFromResources("Prefabs/Popup/Modal/SelectPopup");
            select_popup.SetVerticalScreen();
            select_popup.ShowPopup(GameDefine.GetLocalizeString("system_information"), GameDefine.GetLocalizeString("system_notice_quit_essence_not_enough_point"), GameDefine.GetLocalizeString("system_answer_ok"), (Action<int>)OnClickNoticePopup);
        }
        */

        if (Remain_Count > 0 && charge_item.GetCount() > 0)
        {
            _ = _UpdateSlider(Essence_Charge, Essence_Charge_Plus_Text, 0);
        }
    }

    void OnClickNoticePopup(int btn_index)
    {
        ChangeScene(SceneName.home, Battle_Pc_Data);
    }

    //static int index = 0;

    async UniTaskVoid MoveToTargetAlpha(CancellationToken token, CanvasGroup canvas_group, float target_alpha, float duraion = 0.3f)
    {
        //int local_index = index;
        //index++;
        //Debug.Log($"MoveToTargetAlpha{local_index} Start : {target_alpha}".WithColorTag(Color.white));
        float origin_alpha = canvas_group.alpha;
        float elapsed_time = 0.0f;
        float gap = target_alpha - origin_alpha;
        while (elapsed_time < duraion)
        {
            await UniTask.Yield();
            if (token.IsCancellationRequested)
            {
                break;
            }

            elapsed_time += Time.deltaTime;
            canvas_group.alpha = origin_alpha + gap * (elapsed_time / duraion);
            //Debug.Log($"MoveToTargetAlpha{local_index} Time {elapsed_time} SetAlpha {canvas_group.alpha}".WithColorTag(Color.white));
        }
        canvas_group.alpha = target_alpha;
        //Debug.Log($"MoveToTargetAlpha{local_index} End".WithColorTag(Color.white));
    }

    public override void OnClick(UIButtonBase button)
    {
        base.OnClick(button);

        switch (button.name)
        {
            case "HomeBtn":
                ChangeScene(SceneName.home);
                break;
            case "BackBtn":
                ChangeScene(SceneName.home, Battle_Pc_Data);
                break;
        }

    }

    void ChangeScene(SceneName name, BattlePcData data = null)
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.orientation = ScreenOrientation.AutoRotation;

        TouchCanvas.Instance.SetTouchEffectPrefabPath(TouchCanvas.Effect_Blue_Path);
        TouchCanvas.Instance.EnableTouchEffect(true);

        if (data == null)
        {
            SCManager.I.ChangeScene(name);
        }
        else
        {
            SCManager.I.ChangeScene(name, data);
        }
    }
}

using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ZeroOne.Input;

public class EssenceController : SceneControllerBase
{
    [SerializeField]
    RawImage Chara_Image = null;

    [SerializeField]
    GameObject Effect_Node = null;

    [SerializeField]
    ParticleSystem Success_Effect = null;

    [SerializeField]
    ParticleSystem Climax_Effect = null;

    [SerializeField]
    SerifuBox Serifu_Box = null;

    List<UserL2dData> L2d_List = null;
    Producer pd = null;

    BattlePcData Battle_Pc_Data = null;
    LOVE_LEVEL_TYPE Selected_Relationship;
    int Remain_Count = 0;

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

        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/success");
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
            return;
        }
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


    public void OnResultTransferEssence(bool is_success, TOUCH_BODY_TYPE type)
    {
        Debug.Log($"OnResultTransferEssence : {is_success} : {type}");
        //TODO: 원래라면 실패라서 아무것도 안하지만 M2용 근원전달 플로우를 위해서 존재하는 코드
        if (Essence_Force_Flow[Essence_Force_Flow_Index].Peek().Equals(type))
        {
            Essence_Force_Flow[Essence_Force_Flow_Index].Dequeue();
        }
        if (!is_success && Essence_Force_Flow[Essence_Force_Flow_Index].Count > 0)
        {
            if (Essence_Force_Flow[Essence_Force_Flow_Index].Count == 1)
            {
                pd.SetEssenceBodyPart(Essence_Force_Flow[Essence_Force_Flow_Index].Peek());
            }
            return;
        }

        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/success");

        Success_Effect.Play(true);
        Battle_Pc_Data.User_Data.SetDataSendedEssence(type);

        var charge_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);
        charge_item.UseChargeItem(1);

        GameData.Instance.GetUserHeroDataManager().Save();

        UpdateEventDispatcher.Instance.AddEvent(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ESSESNCE);
    }

    public void OnCompleteTransferEssence()
    {
        // TODO: M2 근원전달 플로우를 위해서는 실패처리
        if (Essence_Force_Flow[Essence_Force_Flow_Index].Count > 0)
        {
            Debug.Assert(Essence_Force_Flow[Essence_Force_Flow_Index].Count > 0, "근원전달 플로우가 더 이상 남아있지 않는 상황 자체가 에러");
            pd.SetEssenceBodyPart(Essence_Force_Flow[Essence_Force_Flow_Index].Dequeue());
        }
        else
        {
            Climax_Effect.Play(true);

            Remain_Count--;
            if (Remain_Count > 0)
            {
                //pd.SetEssenceBodyPart();
                pd.SetEssenceBodyPart(Essence_Force_Flow[Essence_Force_Flow_Index].Dequeue());
            }
        }
    }

    public override void OnClick(UIButtonBase button)
    {
        base.OnClick(button);

        switch (button.name)
        {
            case "HomeBtn":
                SCManager.I.ChangeScene(SceneName.home);
                break;
            case "BackBtn":
                SCManager.I.ChangeScene(SceneName.home, Battle_Pc_Data);
                break;
        }

        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.orientation = ScreenOrientation.AutoRotation;

        TouchCanvas.Instance.EnableTouchEffect(true);
    }
}

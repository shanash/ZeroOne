using Cinemachine;
using Cysharp.Threading.Tasks;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using ZeroOne.Input;
using UnityEngine.UI;

public class WholeBodyController : SceneControllerBase
{
    [SerializeField, Tooltip("Memorial Parent")]
    Transform Memorial_Parent;

    [SerializeField, Tooltip("Memorial Vcam")]
    CinemachineVirtualCamera Memorial_Camera;

    [SerializeField]
    RawImage Hero_Stand;

    [SerializeField, Tooltip("Left UI")]
    GameObject Left_UI;

    [SerializeField, Tooltip("Right UI")]
    GameObject Right_UI;

    [Header("Infobox")]
    [SerializeField, Tooltip("Level Text")]
    TMP_Text Level_Text;

    [SerializeField, Tooltip("Name Text")]
    TMP_Text Name_Text;

    [SerializeField, Tooltip("Tribe Image")]
    protected Image Tribe_Image;

    [SerializeField, Tooltip("Star Images")]
    List<GameObject> Star_Images;

    [Header("Position")]
    [SerializeField, Tooltip("Position Tag")]
    Image Position_Tag;
    [SerializeField, Tooltip("Position Tag Sprites")]
    List<Sprite> Position_Tag_Sprites;

    //[SerializeField, Tooltip("Tribe Box View")]
    //Image Tribe_Box;
    //[SerializeField, Tooltip("Tribe Box Sprites Resources")]
    //List<Sprite> Tribe_Box_Sprites;

    //[SerializeField, Tooltip("Tribe Tag View")]
    //Image Tribe_Tag;
    //[SerializeField, Tooltip("Tribe Tag Sprites Resources")]
    //List<Sprite> Tribe_Tag_Sprites;
    //[SerializeField, Tooltip("Tribe Text")]
    //TMP_Text Tribe_Text;

    [Header("Attribute")]
    [SerializeField, Tooltip("Attribute Tag View")]
    Image Attr_Tag;
    [SerializeField, Tooltip("Attribute Text")]
    TMP_Text Attr_Text;
    [SerializeField, Tooltip("Attribute Tag Sprites Resources")]
    List<Sprite> Attr_Tag_Sprites;

    [Header("Role")]
    [SerializeField, Tooltip("Role Icon View")]
    Image Role_Icon;
    [SerializeField, Tooltip("Role Text")]
    TMP_Text Role_Text;
    [SerializeField, Tooltip("Role Icon Sprites Resources")]
    List<Sprite> Role_Icon_Sprites;

    [Header("Reserved")]
    [SerializeField, Tooltip("애니메이션 재생시에 안보이게 가릴 UI")]
    CanvasGroup HideableUI = null;

    [SerializeField, Tooltip("성공 파티클")]
    ParticleSystem Success_Effect = null;

    [SerializeField, Tooltip("절정 파티클")]
    ParticleSystem Climax_Effect = null;

    //[SerializeField, Tooltip("대사창")]
    //SerifuBox Serifu_Box = null;

    //[SerializeField, Tooltip("게이지 바")]
    //Slider Essence_Charge = null;

    [SerializeField, Tooltip("하단 팁 내용")]
    GameObject Tip_BG = null;

    //[SerializeField, Tooltip("게이지 플러스 텍스트")]
    //TMP_Text Essence_Charge_Plus_Text = null;

    //[SerializeField, Tooltip("오늘 시도 가능한 횟수")]
    //TMP_Text Essence_Chance_Count = null;

    [SerializeField, Tooltip("사각형?")]
    Animator Square = null;

    List<UserL2dData> L2d_List = null;
    Producer pd = null;

    BattlePcData Battle_Pc_Data = null;
    HeroData Hero_Data = null;

    int Current_L2d_Index = -1;
    UserL2dData Current_L2d_Data => L2d_List[Current_L2d_Index];

    LOVE_LEVEL_TYPE Selected_Relationship;
    int Remain_Count = 0;
    private CancellationTokenSource Token_MoveToTargetAlpha = null;

    RenderTexture Chara_Texture = null;

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

    int Essence_Force_Flow_Index { get { if (Remain_Count == 2) return 0; else if (Remain_Count == 1) return 1; return -1; } }

    protected override void Initialize()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup");
        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);

        Screen.orientation = ScreenOrientation.Portrait;
        //Essence_Charge_Plus_Text.alpha = 0;

        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/success");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/DM-CGS-26");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/DM-CGS-45");

        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        //L2d_List = GameData.I.GetUserL2DDataManager().GetUserL2dDataListByChoice();

        HeroDataManager.I.UpdateHeroL2dData();

        L2d_List = HeroDataManager.I.GetHeroL2dDataList();

        if (L2d_List.Count == 0)
        {
            string msg = GameDefine.GetLocalizeString("system_warning_not_exist_user_data");
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

    /// <summary>
    /// SCManager 의 ChangeSceneAsync() 에서 callback 으로 불러움.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    /// <param name="remain_count_of_chance_send_essence"></param>
    /// <returns></returns>
    bool OnReceiveData(HeroData data, int index, int remain_count_of_chance_send_essence)
    {
        //var l2d_list = GameData.I.GetUserL2DDataManager().GetUserL2dDataListByChoice();
        HeroDataManager.I.GetHeroL2dDataList();
        var l2d_list = HeroDataManager.I.GetHeroL2dDataList();

        //Battle_Pc_Data = data;
        Hero_Data = data;
        Selected_Relationship = (LOVE_LEVEL_TYPE)(index + 1);// 인덱스 + 1 = 실제 호감도
        Remain_Count = remain_count_of_chance_send_essence;


        GestureManager.Instance.Enable = false;
        //InitCameraForL2dChar(Memorial_Camera);
        SetLobbyChar(l2d_list);

        Current_L2d_Index = 0;
        UpdateLobbyChar();

        //pd = Factory.Instantiate<Producer>(Hero_Data.Data.essence_id, Selected_Relationship, SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE, Memorial_Parent);

        //pd = Factory.Instantiate<Producer>(Battle_Pc_Data.Data.essence_id, Selected_Relationship, SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE);
        //pd.OnResultTransferEssence += OnResultTransferEssence;
        //pd.OnCompleteTransferEssence += OnCompleteTransferEssence;
        //pd.OnSendActorMessage += Serifu_Box.OnReceiveSpineMessage;

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

    void InitCameraForL2dChar(CinemachineVirtualCamera camera)
    {
        var before_fov = camera.m_Lens.FieldOfView;
        var fov = CalculateAdjustedFOVForAspectRatio(before_fov, GameDefine.RESOLUTION_SCREEN_WIDTH, GameDefine.RESOLUTION_SCREEN_HEIGHT);

        var top = CalculateTop(camera.transform.position, before_fov, Vector3.zero);
        var m_top = CalculateTop(camera.transform.position, fov, Vector3.zero);

        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y - (m_top - top), camera.transform.position.z);
        camera.m_Lens.FieldOfView = fov;
    }

    /// <summary>
    /// 새로운 화면 비율에 따라 수직 FOV를 계산하여 반환
    /// </summary>
    /// <param name="v_cam"></param>
    public float CalculateAdjustedFOVForAspectRatio(float fov, float base_width, float base_height)
    {
        float originalAspectRatio = base_width / base_height;
        float newAspectRatio = (float)Screen.width / (float)Screen.height;

        // 원래의 수평 FOV 계산
        float originalHorizontalFOV = 2f * Mathf.Atan(Mathf.Tan(fov * Mathf.Deg2Rad / 2f) * originalAspectRatio);

        // 새로운 수직 FOV 계산
        float newVerticalFOV = 2f * Mathf.Atan(Mathf.Tan(originalHorizontalFOV / 2f) / newAspectRatio) * Mathf.Rad2Deg;

        return newVerticalFOV;
    }

    float CalculateTop(Vector3 cam_pos, float fov, Vector3 target)
    {
        var distanceToTarget = Vector3.Distance(cam_pos, target);

        // 수직 FOV의 절반을 라디안으로 변환
        float verticalFOVInRadians = fov * Mathf.Deg2Rad;
        // 카메라로부터 대상까지의 거리를 이용하여 시야 상단의 높이 계산
        float topHeightAtDistance = distanceToTarget * Mathf.Tan(verticalFOVInRadians / 2);

        // 카메라의 현재 y 위치에 계산된 높이를 추가하여 상단의 y값 계산
        return cam_pos.y + topHeightAtDistance;
    }

    void UpdateUI()
    {
        //Essence_Chance_Count.text = $"{GameDefine.SENDING_ESSENCE_CHANCE_COUNT_OF_DATE - Battle_Pc_Data.User_Data.Essence_Sended_Count_Of_Date}/{GameDefine.SENDING_ESSENCE_CHANCE_COUNT_OF_DATE}";
    }

    void UpdateLobbyChar()
    {
        ReleaseProducer();

        pd = Factory.Instantiate<Producer>(Current_L2d_Data.Skin_Id, LOVE_LEVEL_TYPE.NORMAL, SPINE_CHARA_LOCATION_TYPE.LOBBY_EXPECT);
        //pd = Factory.Instantiate<Producer>(Current_L2d_Data.Skin_Id, LOVE_LEVEL_TYPE.NORMAL, SPINE_CHARA_LOCATION_TYPE.HERO_INFO, Memorial_Parent);
        //pd = Factory.Instantiate<Producer>(Hero_Data.Data.essence_id, Selected_Relationship, SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE, Memorial_Parent);

        //SetRenderTextureAndCamera();
    }

    void ReleaseProducer()
    {
        if (pd != null)
        {
            pd.Release();
            pd = null;
        }
    }

    void SetRenderTextureAndCamera()
    {
        Chara_Texture = new RenderTexture(GameDefine.RESOLUTION_SCREEN_WIDTH, (int)(Screen.height * ((float)GameDefine.RESOLUTION_SCREEN_WIDTH / (float)Screen.width)), 16);
        var over_cam = GameObject.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.fieldOfView = Camera.main.fieldOfView;
        over_cam.targetTexture = Chara_Texture;

        Hero_Stand.enabled = true;
        Hero_Stand.texture = Chara_Texture;
    }

    void SetLobbyChar(List<UserL2dData> l2d_list)
    {
        L2d_List = l2d_list;
        Left_UI.SetActive(L2d_List.Count > 1);
        Right_UI.SetActive(L2d_List.Count > 1);
        Current_L2d_Index = 0;
        //UpdateLobbyChar();
    }

    async UniTask BlockInputWhenEndingClimaxEffect()
    {
        GestureManager.Instance.Enable = false;
        ScreenEffectManager.I.SetBlockInputUI(true);
        await UniTask.WaitUntil(() => !Climax_Effect.isPlaying);
        GestureManager.Instance.Enable = true;
        ScreenEffectManager.I.SetBlockInputUI(false);

/*        var charge_item = GameData.Instance.GetUserChargeItemDataManager().FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);

        if (Remain_Count > 0 && charge_item.GetCount() > 0)
        {
            _ = _UpdateSlider(Essence_Charge, Essence_Charge_Plus_Text, 0);
        }*/
    }

    void OnClickNoticePopup(int btn_index)
    {
        ChangeScene(SceneName.home, Battle_Pc_Data);
    }

    async UniTaskVoid MoveToTargetAlpha(CancellationToken token, CanvasGroup canvas_group, float target_alpha, float duraion = 0.3f)
    {
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
        }
        canvas_group.alpha = target_alpha;
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

            case "LeftMemorialBtn":
                Current_L2d_Index--;
                if (Current_L2d_Index < 0)
                {
                    Current_L2d_Index = L2d_List.Count - 1;
                }
                UpdateLobbyChar();
                break;

            case "RightMemorialBtn":
                Current_L2d_Index++;
                if (Current_L2d_Index >= L2d_List.Count)
                {
                    Current_L2d_Index = 0;
                }
                UpdateLobbyChar();
                break;
            case "VoiceBtn":
                // todo : 로비터치 캐릭터 사운드 출력
                Debug.Log("Here are 로비터치 캐릭터 사운드 출력");

                AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
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

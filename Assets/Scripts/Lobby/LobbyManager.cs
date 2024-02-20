using Cinemachine;
using Cysharp.Threading.Tasks;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZeroOne.Input;


public class LobbyManager : SceneControllerBase
{
    [SerializeField, Tooltip("Lobby Anim")]
    Animator Lobby_Anim;

    [SerializeField, Tooltip("Memorial Parent")]
    Transform Memorial_Parent;

    [SerializeField, Tooltip("Fade in Box")]
    RectTransform Fade_In_Box;

    [SerializeField, Tooltip("Memorial Vcam")]
    CinemachineVirtualCamera Memorial_Camera;

    Producer pd = null;

    protected override void Initialize()
    {
        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");

        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        var pmng = PopupManager.Instance;
        pmng.SetRootOnEnter(() => pd.Resume());
        pmng.SetRootOnExit(() => pd.Pause());

        pd = Factory.Instantiate<Producer>(1010101, MEMORIAL_TYPE.MAIN_LOBBY, Memorial_Parent);
        GestureManager.Instance.Enable = false;

        InitCameraForL2dChar(Memorial_Camera);

        _ = InitializeAsync();
    }

    async UniTask InitializeAsync()
    {
        await UniTask.WaitUntil(() => pd.Is_Init);

        var board = BlackBoard.Instance;
        int open_dungeon_id = board.GetBlackBoardData<int>(BLACK_BOARD_KEY.OPEN_STORY_STAGE_DUNGEON_ID, 0);
        if (open_dungeon_id > 0)
        {
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/MissionGateUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
            {
                popup.ShowPopup();
            });
        }

        SCManager.Instance.SetCurrent(this);
    }

    void InitCameraForL2dChar(CinemachineVirtualCamera camera)
    {
        var before_fov = camera.m_Lens.FieldOfView;
        var fov = CalculateAdjustedFOVForAspectRatio(before_fov, GameDefine.SCREEN_UI_BASE_WIDTH, GameDefine.SCREEN_UI_BASE_HEIGHT);

        var top = CalculateTop(camera.transform.position, before_fov, Vector3.zero);
        var m_top = CalculateTop(camera.transform.position, fov, Vector3.zero);

        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y - (m_top - top), camera.transform.position.z);
        camera.m_Lens.FieldOfView = fov;
    }

    public override void OnClick(UIButtonBase button)
    {
        base.OnClick(button);

        switch (button.name)
        {
            case "ChageCharacterBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Lobby/SelectLobbyCharacterPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
                {
                    popup.ShowPopup();
                });
                break;
            case "CharacterBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Hero/HeroListUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
                {
                    popup.ShowPopup();
                });
                break;
            case "LeftMemorialBtn":
                CommonUtils.ShowToast("Left 메모리얼", TOAST_BOX_LENGTH.SHORT);
                break;
            case "RightMemorialBtn":
                CommonUtils.ShowToast("Right 메모리얼", TOAST_BOX_LENGTH.SHORT);
                break;
            case "PlayBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Mission/MissionGateUI", POPUP_TYPE.FULLPAGE_TYPE, (popup) =>
                {
                    popup.ShowPopup();
                });
                break;
            case "UIHideBtn":
                Lobby_Anim.SetTrigger("fadeout");
                break;
            case "UIShowBtn":
                Lobby_Anim.SetTrigger("fadein");
                break;
            case "PartyBtn":
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/PartySettingPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
                {
                    popup.ShowPopup(GAME_TYPE.NONE, 0);
                });
                break;
            case "HouseBtn":
            case "MissionBtn":
            case "ShopBtn":
            case "SearchBtn":
                ShowNotYetNoti();
                break;
        }
    }

    void ShowNotYetNoti()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
        {
            popup.ShowPopup(3f, ConstString.Message.NOT_YET);
        });
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

    // 카메라의 up 벡터, lookAt 벡터, 위치, 그리고 FOV를 기반으로 상단 면의 평면을 계산합니다.
    public static Plane CalculateTopPlane(Vector3 position, Vector3 lookAt, Vector3 up, float fov)
    {
        // right 벡터 계산
        Vector3 right = Vector3.Cross(lookAt, up);
        right = Vector3.Normalize(right); // right 벡터 정규화

        // 수정된 up 벡터 계산 (정확한 상단 방향)
        Vector3 trueUp = Vector3.Cross(right, lookAt);
        trueUp = Vector3.Normalize(trueUp); // trueUp 벡터 정규화

        // 상단 면의 방향 벡터 계산
        float radians = fov * Mathf.Deg2Rad / 2.0f; // FOV를 라디안으로 변환
        Vector3 topDirection = lookAt * Mathf.Cos(radians) + trueUp * Mathf.Sin(radians);
        topDirection = Vector3.Normalize(topDirection); // topDirection 벡터 정규화

        // 상단 면의 평면 방정식 계산
        Plane topPlane = new Plane(topDirection, Vector3.Dot(topDirection, position));

        return topPlane;
    }

    public static Plane CalculateTopPlane_(Vector3 cameraPosition, Vector3 targetPosition, Vector3 up,  float fov)
    {
        // 카메라에서 대상까지의 방향 벡터 계산
        Vector3 directionToTarget = (targetPosition - cameraPosition).normalized;

        // right 벡터 계산
        Vector3 right = Vector3.Cross(directionToTarget, up);

        // 수정된 up 벡터 계산
        Vector3 trueUp = Vector3.Cross(right, directionToTarget);

        // 상단 면의 방향 벡터 계산
        float radians = fov * Mathf.Deg2Rad / 2; // FOV를 라디안으로 변환
        Vector3 topDirection = directionToTarget * Mathf.Cos(radians) + trueUp * Mathf.Sin(radians);

        // 상단 면의 평면 방정식 계산
        Plane topPlane = new Plane(Vector3.Normalize(topDirection), cameraPosition);
        return topPlane;
    }

    #region UI Animation Events
    public void OnUIHideComplete()
    {
        Fade_In_Box.gameObject.SetActive(true);
        GestureManager.Instance.Enable = true;
    }

    public void OnUIShowBegin()
    {
        Fade_In_Box.gameObject.SetActive(false);
        GestureManager.Instance.Enable = false;
    }
    #endregion
}

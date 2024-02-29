using Cinemachine;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZeroOne.Input;

public class EssenceTransferPopup : PopupBase
{
    const float FOV = 90;

    [SerializeField, Tooltip("화면을 가로로 돌리는 버튼")]
    GameObject ToHorizontalButton = null;

    [SerializeField, Tooltip("화면을 세로로 돌리는 버튼")]
    GameObject ToVerticalButton = null;

    [SerializeField]
    RawImage Chara_Image = null;

    BattlePcData Battle_Pc_Data = null;
    LOVE_LEVEL_TYPE Selected_Relationship;

    RenderTexture Chara_Texture = null;
    Producer Producer = null;

    Vector3 local_origin_pos = Vector3.zero;
    float fov = 0.0f;


    public override void Spawned()
    {
        base.Spawned();

        Chara_Texture = new RenderTexture((int)(Screen.height * ((float)GameDefine.SCREEN_UI_BASE_WIDTH / (float)Screen.width)), GameDefine.SCREEN_UI_BASE_WIDTH, 16);
        var over_cam = Camera.main.transform.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.targetTexture = Chara_Texture;
        Chara_Image.texture = Chara_Texture;
        InputCanvas.Instance.RenderImage = Chara_Image;
        InputCanvas.Instance.RenderCamera = over_cam;
        Screen.orientation = ScreenOrientation.Portrait;

        //TODO: 일단 임시로 카메라로 위치를 세팅
        local_origin_pos = over_cam.transform.localPosition;
        over_cam.transform.localPosition = new Vector3(0, 0, 0);

        over_cam.fieldOfView = FOV;
        var brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        var vCam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        fov = vCam.m_Lens.FieldOfView;
        vCam.m_Lens.FieldOfView = FOV;

        TouchCanvas.Instance.EnableTouchEffect(false);
    }

    public override void Despawned()
    {
        try
        {
            GestureManager.Instance.Enable = false;
            base.Despawned();

            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        if (Producer != null)
        {
            Producer.Release();
            Producer = null;
        }

        //TODO: 일단 임시로 카메라로 위치를 세팅
        var over_cam = Camera.main.transform.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.transform.localPosition = local_origin_pos;

        over_cam.fieldOfView = fov;
        var brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        var vCam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        vCam.m_Lens.FieldOfView = fov;

        TouchCanvas.Instance.EnableTouchEffect(true);

    }

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2 || data[0] is not BattlePcData || data[1] is not int)
        {
            return false;
        }

        Battle_Pc_Data = data[0] as BattlePcData;
        Selected_Relationship = (LOVE_LEVEL_TYPE)((int)data[1] + 1);// 인덱스 + 1 = 실제 호감도

        Producer = Factory.Instantiate<Producer>(Battle_Pc_Data.Data.essence_id, Selected_Relationship, SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE);
        Producer.OnSuccessTransferEssence += OnSuccessTransfer;

        GestureManager.Instance.Enable = true;

        return true;
    }

    public void OnSuccessTransfer(TOUCH_BODY_TYPE type)
    {
        Battle_Pc_Data.User_Data.SetDataSendedEssence(type);
        GameData.Instance.GetUserHeroDataManager().Save();
    }

    public void OnClickBackButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Closed_Delegate?.Invoke();

        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.DIALOG_TYPE);
    }

    public void OnClickHomeButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.CloseAll();
    }

    // TODO: 현재는 사용하지 않는 버튼입니다. 오래 방치되면 지워줄것
    public void OnClickToVerticalButton()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        ToVerticalButton.SetActive(false);
        ToHorizontalButton.SetActive(true);
    }

    // TODO: 현재는 사용하지 않는 버튼입니다. 오래 방치되면 지워줄것
    public void OnClickToHorizontalButton()
    {
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }
        ToVerticalButton.SetActive(true);
        ToHorizontalButton.SetActive(false);
    }
}

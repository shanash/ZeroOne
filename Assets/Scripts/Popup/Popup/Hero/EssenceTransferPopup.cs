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
    const float ADDED_Y = -3.8f;

    [SerializeField, Tooltip("화면을 가로로 돌리는 버튼")]
    GameObject ToHorizontalButton = null;

    [SerializeField, Tooltip("화면을 세로로 돌리는 버튼")]
    GameObject ToVerticalButton = null;

    [SerializeField]
    RawImage Chara_Image = null;

    BattlePcData Battle_Pc_Data = null;
    RenderTexture Chara_Texture = null;
    Producer pd = null;

    Vector3 local_origin_pos = Vector3.zero;
    float fov = 0.0f;



    public override void Spawned()
    {
        base.Spawned();

        Screen.orientation = ScreenOrientation.Portrait;

        Chara_Texture = new RenderTexture((int)(Screen.height * ((float)GameDefine.SCREEN_UI_BASE_WIDTH / (float)Screen.width)), GameDefine.SCREEN_UI_BASE_WIDTH, 16);
        var over_cam = Camera.main.transform.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.targetTexture = Chara_Texture;
        Chara_Image.texture = Chara_Texture;
        InputCanvas.Instance.RenderImage = Chara_Image;
        InputCanvas.Instance.RenderCamera = over_cam;

        //TODO: 일단 임시로 카메라로 위치를 세팅
        local_origin_pos = over_cam.transform.localPosition;
        over_cam.transform.localPosition = new Vector3(0, 0, 0);

        over_cam.fieldOfView = FOV;
        var brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        var vCam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        fov = vCam.m_Lens.FieldOfView;
        vCam.transform.localPosition = new Vector3(0, ADDED_Y, -10);
        vCam.m_Lens.FieldOfView = FOV;

        //pd = Factory.Instantiate<Producer>(1010101, MEMORIAL_TYPE.MEMORIAL);
        //GestureManager.Instance.Enable = true;
    }

    public override void Despawned()
    {
        try
        {
            GestureManager.Instance.Enable = false;
            base.Despawned();

            Screen.orientation = ScreenOrientation.LandscapeRight;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        if (pd != null)
        {
            pd.Release();
            pd = null;
        }

        //TODO: 일단 임시로 카메라로 위치를 세팅
        var over_cam = Camera.main.transform.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.transform.localPosition = local_origin_pos;

        over_cam.fieldOfView = fov;
        var brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        var vCam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        vCam.transform.localPosition = new Vector3(0, 0, -10);
        vCam.m_Lens.FieldOfView = fov;
    }

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1 || data[0] is not BattlePcData)
        {
            return false;
        }

        Battle_Pc_Data = data[0] as BattlePcData;
        pd = Factory.Instantiate<Producer>(Battle_Pc_Data.Data.essence_id, MEMORIAL_TYPE.MEMORIAL);

        GestureManager.Instance.Enable = true;

        OnClickToVerticalButton();
        return true;
    }

    public void OnClickBackButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.DIALOG_TYPE);
    }

    public void OnClickHomeButton()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.CloseAll();
    }

    public void OnClickToVerticalButton()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        ToVerticalButton.SetActive(false);
        ToHorizontalButton.SetActive(true);
    }

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

    protected override void OnUpdatePopup()
    {
        base.OnUpdatePopup();

        switch (Screen.orientation)
        {
            case ScreenOrientation.LandscapeLeft:
                if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
                {
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                }
                break;
            case ScreenOrientation.LandscapeRight:
                if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
                {
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                }
                break;
        }
    }
}

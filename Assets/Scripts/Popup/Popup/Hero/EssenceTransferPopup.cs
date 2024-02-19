using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZeroOne.Input;

public class EssenceTransferPopup : PopupBase
{
    [SerializeField, Tooltip("화면을 가로로 돌리는 버튼")]
    GameObject ToHorizontalButton = null;

    [SerializeField, Tooltip("화면을 세로로 돌리는 버튼")]
    GameObject ToVerticalButton = null;

    [SerializeField]
    RawImage Chara_Image = null;

    RenderTexture Chara_Texture = null;
    Producer pd = null;

    public override void Spawned()
    {
        base.Spawned();

        Chara_Texture = new RenderTexture(1920, GameDefine.SCREEN_BASE_HEIGHT, 16);
        var over_cam = Camera.main.transform.Find("RenderTexture Camera").GetComponent<Camera>();
        over_cam.targetTexture = Chara_Texture;
        Chara_Image.texture = Chara_Texture;
        InputCanvas.Instance.RenderImage = Chara_Image;
        InputCanvas.Instance.RenderCamera = over_cam;

        pd = Factory.Instantiate<Producer>(1010001, MEMORIAL_TYPE.MEMORIAL);
        GestureManager.Instance.Enable = true;
    }

    public override void Despawned()
    {
        try
        {
            GestureManager.Instance.Enable = false;
            base.Despawned();

            pd.Release();

            Screen.orientation = ScreenOrientation.AutoRotation;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
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

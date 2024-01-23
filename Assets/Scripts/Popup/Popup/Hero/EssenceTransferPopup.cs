using FluffyDuck.UI;
using UnityEngine;

public class EssenceTransferPopup : PopupBase
{
    [SerializeField, Tooltip("화면을 가로로 돌리는 버튼")]
    GameObject ToHorizontalButton;

    [SerializeField, Tooltip("화면을 세로로 돌리는 버튼")]
    GameObject ToVerticalButton;

    protected override void Initialize()
    {
        base.Initialize();
    }

    public void OnClickBackButton()
    {
        Initialize();
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.FULLPAGE_TYPE);
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

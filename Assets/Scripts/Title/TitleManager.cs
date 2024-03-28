using Cysharp.Threading.Tasks;
using FluffyDuck.Addressable;
using FluffyDuck.Util;
using System.Diagnostics;

public class TitleManager : SceneControllerBase
{
    bool Is_Enable_Touch;

    AddressableContentDownloader _Downloader = new AddressableContentDownloader();

    public void TitleAnimationComplete()
    {
        Is_Enable_Touch = true;
    }

    public override void OnClick(UIButtonBase button)
    {
        if (Is_Enable_Touch)
        {
            //base.OnClick(button);

            switch (button.name)
            {
                case "Background_Touch_Btn":
                    Is_Enable_Touch = false;
                    CheckAndDownloadAssets(() => SCManager.Instance.ChangeScene(SceneName.load)).Forget();
                    break;
                case "MenuBtn":
                    CommonUtils.ShowToast(GameDefine.GetLocalizeString("system_alert_preparing"), TOAST_BOX_LENGTH.SHORT);
                    break;
            }
        }
    }

    async UniTaskVoid CheckAndDownloadAssets(System.Action callback)
    {
        if (!await _Downloader.Init())
        {
            UnityEngine.Debug.LogError("초기화 실패");
            return;
        }

        if (_Downloader.HasNewDataVersion())
        {
            //TODO: UI 표시

            _Downloader.AddDownloadProgressHandler(OnProgressDownload);
            await _Downloader.Download();
            await UniTask.Delay(500); // 짧은 지연 후 로그 추가
        }

        // 다운로드 완료
        callback();
    }

    void OnProgressDownload(string group_key, float progress, double size)
    {
        string log = $"{group_key} : {progress * size:F2} / {size:F2}";
        UnityEngine.Debug.Log(log);
    }
}

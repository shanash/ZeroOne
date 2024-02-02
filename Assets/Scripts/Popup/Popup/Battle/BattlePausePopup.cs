using FluffyDuck.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePausePopup : PopupBase
{
    /// <summary>
    /// 이어 하기
    /// </summary>
    public void OnClickContinue()
    {
        HidePopup();
    }

    /// <summary>
    /// 설정 팝업 
    /// </summary>
    public void OnClickSettings()
    {
        Debug.Log("Show Setting Popup");
    }

    /// <summary>
    /// 전투 종료. 홈으로 이동
    /// </summary>
    public void OnClickExit()
    {
        SceneManager.LoadScene("home");
    }
}

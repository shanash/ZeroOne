using FluffyDuck.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    bool Is_Enable_Touch;

    public void TitleAnimationComplete()
    {
        Is_Enable_Touch = true;
    }

    public void OnClickTouchEvent()
    {
        if (Is_Enable_Touch)
        {
            SceneManager.LoadScene(GameDefine.SCENE_LOAD);
        }

    }
    public void OnClickTitleMenu()
    {
        if (Is_Enable_Touch)
        {
            CommonUtils.ShowToast(ConstString.Message.NOT_YET, TOAST_BOX_LENGTH.SHORT);
        }
    }

}

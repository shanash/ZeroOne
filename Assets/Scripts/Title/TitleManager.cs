using FluffyDuck.Util;

public class TitleManager : SceneControllerBase
{
    bool Is_Enable_Touch;

    public void TitleAnimationComplete()
    {
        Is_Enable_Touch = true;
    }

    public override void OnClick(UIButtonBase button)
    {
        if (Is_Enable_Touch)
        {
            //base.OnClick(button_name);

            switch (button.name)
            {
                case "Background_Touch_Btn":
                    SCManager.Instance.ChangeScene(SceneName.load);
                    break;
                case "MenuBtn":
                    CommonUtils.ShowToast(ConstString.Message.NOT_YET, TOAST_BOX_LENGTH.SHORT);
                    break;
            }
        }
    }
}

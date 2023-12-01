using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLosePopup : PopupBase
{
    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);
    }

    public void OnClickGoHome()
    {
        SceneManager.LoadScene("home");
    }
}

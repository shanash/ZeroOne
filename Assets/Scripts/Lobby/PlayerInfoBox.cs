using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBox : MonoBehaviour
{
    [SerializeField, Tooltip("BG")]
    Image Player_Info_BG;

    [SerializeField, Tooltip("Level Text")]
    TMP_Text Player_Level_Text;

    [SerializeField, Tooltip("Nickname Text")]
    TMP_Text Player_Nickname_Text;


    [SerializeField, Tooltip("Exp Bar")]
    Image Exp_Bar;

    [SerializeField, Tooltip("Exp Current Text. Current Color #80BCFF")]
    TMP_Text Exp_Point;

    public void OnClickTestBattleResult()
    {
        //bool test_win = false;
        //if (test_win)
        //{
        //    PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultWinPopup", (popup) =>
        //    {
        //        popup.ShowPopup();
        //    });
        //}
        //else
        //{
        //    PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultLosePopup", (popup) =>
        //    {
        //        popup.ShowPopup();
        //    });
        //}

        
    }

}

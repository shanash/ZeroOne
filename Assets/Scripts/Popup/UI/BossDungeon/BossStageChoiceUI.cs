using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStageChoiceUI : PopupBase
{
    [Header("Left Box")]
    [SerializeField, Tooltip("Blur")]
    Image Back_Blur;
    [SerializeField, Tooltip("Boss Image")]
    Image Boss_Image;

    [SerializeField, Tooltip("Boss Nick")]
    TMP_Text Boss_Nick;
    [SerializeField, Tooltip("Boss Name")]
    TMP_Text Boss_Name;

    [SerializeField, Tooltip("Extra Rank Title")]
    TMP_Text Extra_Rank_Title;
    [SerializeField, Tooltip("Extra Stage Btn")]
    UIButtonBase Extra_Stage_Btn;
    [SerializeField, Tooltip("Extra Stage Lock")]
    Image Extra_Stage_Lock;

    [SerializeField, Tooltip("Boss Info List View")]
    ScrollRect Boss_Info_List_View;

    [Space()]
    [Header("Right Box")]
    [SerializeField, Tooltip("Entrance Count")]
    TMP_Text Entrance_Count;

    [SerializeField, Tooltip("Stage List View")]
    ScrollRect Stage_List_View;

    protected override bool Initialize(object[] data)
    {
        return true;
    }


}

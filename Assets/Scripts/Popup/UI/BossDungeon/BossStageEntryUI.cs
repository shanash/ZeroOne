using FluffyDuck.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStageEntryUI : PopupBase
{
    [SerializeField, Tooltip("Boss List View")]
    ScrollRect Boss_List_View;

    [SerializeField, Tooltip("Entrance Count Title")]
    TMP_Text Entrance_Count_Title;
    [SerializeField, Tooltip("Entrance Count")]
    TMP_Text Entrance_Count;



    protected override bool Initialize(object[] data)
    {
        return true;
    }

    protected override void FixedUpdatePopup()
    {

    }
}

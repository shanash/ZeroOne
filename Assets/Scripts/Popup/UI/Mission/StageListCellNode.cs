using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageListCellNode : InfiniteScrollItem
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Stage Number")]
    TMP_Text Stage_Number;

    [SerializeField, Tooltip("Star List")]
    List<Image> Stage_Star_Points;

    [SerializeField, Tooltip("Stage Name")]
    TMP_Text Stage_Name;

    [SerializeField, Tooltip("Entrance Btn")]
    UIButtonBase Entrance_Btn;

    [SerializeField, Tooltip("Lock Cover")]
    RectTransform Lock_Cover;

}

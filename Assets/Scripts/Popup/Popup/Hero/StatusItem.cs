
using Gpm.Ui;
using TMPro;
using UnityEngine;

public class StatusItem : InfiniteScrollItem
{
    [SerializeField]
    TMP_Text Subject_UI;

    [SerializeField]
    TMP_Text Value_UI;

    public override void UpdateData(InfiniteScrollData scroll_data)
    {
        base.UpdateData(scroll_data);

        var data = (StatusItemData)scroll_data;

        Subject_UI.text = data.Subject;
        Value_UI.text = data.Value;
    }
}
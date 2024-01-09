using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterCell : InfiniteScrollItem
{
    [SerializeField, Tooltip("Cell Items")]
    List<LobbyCharacterCellItem> Cell_Items;


    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        var data = (LobbyCharacterListData)scrollData;

        var memorial_list = data.GetUserMemorialDataList();
        int cnt = Cell_Items.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Cell_Items[i];
            slot.SetClickMemorialCallback(data.Click_Memorial_Callback);
            if (i < memorial_list.Count)
            {
                slot.SetUserMemorialData(memorial_list[i]);
            }
            else
            {
                slot.SetUserMemorialData(null);
            }
        }
    }


}

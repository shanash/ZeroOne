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

        var memorial_list = data.GetUserL2dDataList();
        int cnt = Cell_Items.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Cell_Items[i];
            slot.SetClickMemorialCallback(data.Click_Char_Callback);
            if (i < memorial_list.Count)
            {
                slot.SetUserL2dData(memorial_list[i]);
            }
            else
            {
                slot.SetUserL2dData(null);
            }
        }
    }


}

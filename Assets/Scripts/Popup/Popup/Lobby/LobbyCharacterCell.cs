using Gpm.Ui;
using System.Collections;
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
        var hero_list = data.GetUserHeroDataList();

        int cnt = Cell_Items.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Cell_Items[i];
            slot.SetClickCallback(data.Click_Callback);
            if (i < hero_list.Count)
            {
                slot.SetUserHeroData(hero_list[i]);
            }
            else
            {
                slot.SetUserHeroData(null);
            }
        }
    }

    public void SetChoiceItemCallback(System.Action<UserHeroData> cb)
    {
        Cell_Items.ForEach(x => x.SetClickCallback(cb));
    }
}

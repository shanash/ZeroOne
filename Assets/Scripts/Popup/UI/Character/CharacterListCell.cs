using UnityEngine;
using System.Collections.Generic;
using Gpm.Ui;

public class CharacterListCell : InfiniteScrollItem
{
    [SerializeField, Tooltip("Card Item List")]
    List<CharacterListItem> Card_Item_List;

    public override void UpdateData(InfiniteScrollData scroll_data)
    {
        base.UpdateData(scroll_data);

        var data = (CharacterListData)scroll_data;

        var hero_list = data.GetUserHeroDataList();
        int cnt = Card_Item_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Card_Item_List[i];
            slot.SetClickHeroCallback(data.Click_Hero_Callback);
            if (i < hero_list.Count)
            {
                slot.SetUserHeroData(hero_list[i], data.Filter_Type);
            }
            else
            {
                slot.SetUserHeroData(null);
            }
        }
    }
}

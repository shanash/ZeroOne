using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;

public class PartyCharacterListCell : InfiniteScrollItem
{
    [SerializeField, Tooltip("Card Item List")]
    List<PartyCharacterListItem> Card_Item_List;

    public override void UpdateData(InfiniteScrollData scroll_data)
    {
        base.UpdateData(scroll_data);

        var data = (PartyCharacterListData)scroll_data;

        var hero_list = data.GetBattleHeroDataList();
        int cnt = Card_Item_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var slot = Card_Item_List[i];
            //slot.Click_Hero_Callback.RemoveAllListeners();
            slot.Click_Hero_Callback.RemoveListener(data.Click_Hero_Callback);
            slot.Click_Hero_Callback.AddListener(data.Click_Hero_Callback);
            if (i < hero_list.Count)
            {
                slot.SetBattleUnitData(hero_list[i], data.Game_Type, data.Filter_Type);
            }
            else
            {
                slot.SetBattleUnitData(null, data.Game_Type, data.Filter_Type);
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using Gpm.Ui;

public class HeroListCell : InfiniteScrollItem
{
    [SerializeField, Tooltip("Card Item List")]
    List<HeroListItem> Card_Item_List;

    public override void UpdateData(InfiniteScrollData scroll_data)
    {
        base.UpdateData(scroll_data);

        var data = (HeroListData)scroll_data;

        if( data.ExpectHero)
        {
            // 미보유 Hero
            var hero_list = data.GetHeroDataList();
            int cnt = Card_Item_List.Count;
            for (int i = 0; i < cnt; i++)
            {
                var slot = Card_Item_List[i];
                slot.SetClickHeroCallback(data.Click_Hero_Callback);
                if (i < hero_list.Count)
                {
                    slot.SetHeroData(hero_list[i], data.Filter_Type);
                }
                else
                {
                    slot.SetHeroData(null);
                }
            }
        }
        else
        {
            // 보유 Hero
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
}

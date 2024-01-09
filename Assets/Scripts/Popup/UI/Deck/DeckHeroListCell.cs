using System.Collections.Generic;
using UnityEngine;

public class DeckHeroListCell : UIBase
{
    [SerializeField, Tooltip("Hero Card List")]
    List<DeckHeroListCardNode> Hero_Cards;

    public void SetUserHeroDataList(List<UserHeroData> list)
    {
        int cnt = Hero_Cards.Count;
        for (int i = 0; i < cnt; i++)
        {
            var card = Hero_Cards[i];
            if (i < list.Count)
            {
                card.SetUserHeroData(list[i]);
            }
            else
            {
                card.SetUserHeroData(null);
            }
        }
    }

    public void SetDeckHeroListCardChoiceCallback(System.Action<DeckHeroListCardNode> cb)
    {
        int cnt = Hero_Cards.Count;
        for (int i = 0; i < cnt; i++)
        {
            Hero_Cards[i].SetClickCallback(cb);
        }
    }

    public void UpdateHeroListCardNodes()
    {
        int cnt = Hero_Cards.Count;
        for (int i = 0; i < cnt; i++)
        {
            Hero_Cards[i].UpdateCardNode();
        }
    }

}

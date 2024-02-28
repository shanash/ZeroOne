using Gpm.Ui;
using System.Collections.Generic;

public class HeroListData : InfiniteScrollData
{
    List<BattlePcData> User_Hero_Data_List = new List<BattlePcData>();

    public System.Action<HeroListItem> Click_Hero_Callback;

    public CHARACTER_SORT Filter_Type { get; set; }

    public void SetUserHeroDataList(List<BattlePcData> list)
    {
        User_Hero_Data_List.Clear();
        if (list != null)
        {
            User_Hero_Data_List.AddRange(list);
        }
    }

    public List<BattlePcData> GetUserHeroDataList()
    {
        return User_Hero_Data_List;
    }
}

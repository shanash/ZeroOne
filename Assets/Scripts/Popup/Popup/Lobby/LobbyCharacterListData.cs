

using Gpm.Ui;
using System.Collections.Generic;

public class LobbyCharacterListData : InfiniteScrollData
{
    List<UserHeroData> Hero_List = new List<UserHeroData>();

    public System.Action<UserHeroData> Click_Callback;

    public void SetUserHeroDataList(List<UserHeroData> list)
    {
        Hero_List.Clear();
        if (list != null)
        {
            Hero_List.AddRange(list);
        }
    }

    public List<UserHeroData> GetUserHeroDataList()
    {
        return Hero_List;
    }

    public void SetClickCallback(System.Action<UserHeroData> cb)
    {
        Click_Callback = cb;
    }
    
}

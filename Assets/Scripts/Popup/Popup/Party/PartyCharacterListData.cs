using Gpm.Ui;
using System.Collections.Generic;

public class PartyCharacterListData : InfiniteScrollData
{
    List<UserHeroData> User_Hero_Data_List = new List<UserHeroData>();

    public System.Action<PartyCharacterListItem> Click_Hero_Callback;

    public GAME_TYPE Game_Type { get; protected set; } = GAME_TYPE.NONE;

    public void SetUserHeroDataList(List<UserHeroData> list)
    {
        User_Hero_Data_List.Clear();
        if (list != null)
        {
            User_Hero_Data_List.AddRange(list);
        }
    }

    public void SetGameType(GAME_TYPE gtype)
    {
        Game_Type = gtype;
    }

    public List<UserHeroData> GetUserHeroDataList()
    {
        return User_Hero_Data_List;
    }
}

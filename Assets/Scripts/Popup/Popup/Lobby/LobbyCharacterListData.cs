using Gpm.Ui;
using System.Collections.Generic;

public class LobbyCharacterListData : InfiniteScrollData
{
    List<UserL2dData> L2d_List = new List<UserL2dData>();

    public System.Action<UserL2dData> Click_Char_Callback;

    public void SetUserL2dDataList(List<UserL2dData> list)
    {
        L2d_List.Clear();
        if (list != null)
        {
            L2d_List.AddRange(list);
        }
    }

    public List<UserL2dData> GetUserL2dDataList()
    {
        return L2d_List;
    }

}

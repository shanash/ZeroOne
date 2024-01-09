using Gpm.Ui;
using System.Collections.Generic;

public class LobbyCharacterListData : InfiniteScrollData
{
    List<UserMemorialData> Memorial_List = new List<UserMemorialData>();

    public System.Action<UserMemorialData> Click_Memorial_Callback;

    public void SetUserMemorialDataList(List<UserMemorialData> list)
    {
        Memorial_List.Clear();
        if (list != null)
        {
            Memorial_List.AddRange(list);
        }
    }
    public List<UserMemorialData> GetUserMemorialDataList()
    {
        return Memorial_List;
    }

}

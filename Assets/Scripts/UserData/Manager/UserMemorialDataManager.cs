


using System.Collections.Generic;

public class UserMemorialDataManager : ManagerBase
{
    List<UserMemorialData> User_Memorial_Data_List = new List<UserMemorialData>();

    public UserMemorialDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype) { }

    protected override void Destroy()
    {
        int cnt = User_Memorial_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Memorial_Data_List[i].Dispose();
        }
        User_Memorial_Data_List.Clear();
    }

    public override void InitDataManager()
    {
        if (User_Memorial_Data_List.Count > 0)
        {
            return;
        }
        DummyDataSetting();
    }
    void DummyDataSetting()
    {
        var m = MasterDataManager.Instance;
    }
}

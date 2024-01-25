using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class UserItemDataManager : ManagerBase
{
    List<UserItemData> User_Item_Data_List = new List<UserItemData>();

    public UserItemDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }

    protected override void Destroy()
    {
        int cnt = User_Item_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Item_Data_List[i].Dispose();
        }
        User_Item_Data_List.Clear();
    }

    public override void InitDataManager()
    {
        if (User_Item_Data_List.Count > 0)
        {
            return;
        }
        DefaultDataSetting();
    }

    void DefaultDataSetting() 
    {
        var m = MasterDataManager.Instance;
        var item_list = m.Get_ItemDataListByItemType(ITEM_TYPE_V2.EXP_SKILL);
        int cnt = item_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = item_list[i];
        }
    }

    public UserItemData FindUserItem(ITEM_TYPE_V2 itype, int item_id)
    {
        return User_Item_Data_List.Find(x => x.Item_Type == itype && x.Item_ID == item_id);
    }

    public override JsonData Serialized()
    {
        return null;
    }

    public override bool Deserialized(JsonData json)
    {
        return false;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_ITEM_DATA_LIST = "ilist";


}

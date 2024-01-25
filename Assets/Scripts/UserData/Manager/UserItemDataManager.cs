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
            AddUserItemData(item.item_type, item.item_id);
        }
        Save();
    }

    public UserItemData FindUserItem(ITEM_TYPE_V2 itype, int item_id)
    {
        return User_Item_Data_List.Find(x => x.Item_Type == itype && x.Item_ID == item_id);
    }

    UserItemData AddUserItemData(ITEM_TYPE_V2 itype, int item_id)
    {
        var item = FindUserItem(itype, item_id);
        if (item == null)
        {
            item = new UserItemData();
            item.SetItemType(itype, item_id);
            User_Item_Data_List.Add(item);
        }
        return item;
    }
    /// <summary>
    /// 아이템 추가. <br/>
    /// 아이템을 찾아서 직접 추가해줘도 되지만, 가끔 메니저에서 바로 입력이 가능한 기능도 필요.
    /// </summary>
    /// <param name="itype"></param>
    /// <param name="item_id"></param>
    /// <param name="add_cnt"></param>
    /// <returns></returns>
    public ERROR_CODE AddUserItemCount(ITEM_TYPE_V2 itype, int item_id, double add_cnt)
    {
        var item = FindUserItem(itype, item_id);
        if (item == null)
        {
            item = AddUserItemData(itype, item_id);
        }
        return item.AddItemCount(add_cnt);
    }
    /// <summary>
    /// 아이템 사용<br/>
    /// </summary>
    /// <param name="itype"></param>
    /// <param name="item_id"></param>
    /// <param name="use_cnt"></param>
    /// <returns></returns>
    public ERROR_CODE UseItemCount(ITEM_TYPE_V2 itype, int item_id, double use_cnt)
    {
        var item = FindUserItem(itype, item_id);
        if (item == null)
        {
            item = AddUserItemData(itype, item_id);
        }
        return item.UseItemCount(use_cnt);
    }
    /// <summary>
    /// 아이템 사용 가능 여부 체크
    /// </summary>
    /// <param name="itype"></param>
    /// <param name="item_id"></param>
    /// <param name="use_cnt"></param>
    /// <returns></returns>
    public bool IsUsableItemCount(ITEM_TYPE_V2 itype, int item_id, double use_cnt)
    {
        var item = FindUserItem(itype, item_id);
        if (item == null)
        {
            item = AddUserItemData(itype, item_id);
        }
        return item.IsUsableItemCount(use_cnt);
    }


    public override JsonData Serialized()
    {
        var json = new JsonData();

        var arr = new JsonData();
        int cnt = User_Item_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Item_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_ITEM_DATA_LIST] = arr;
        }
        if (json.Keys.Count > 0)
        {
            return json;
        }

        return null;
    }

    public override bool Deserialized(JsonData json)
    {
        if (json == null) return false;

        if (json.ContainsKey(NODE_ITEM_DATA_LIST))
        {
            var arr = json[NODE_ITEM_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];
                    int item_type = 0;
                    int item_id = 0;
                    if (int.TryParse(jdata[NODE_ITEM_TYPE].ToString(), out item_type) && int.TryParse(jdata[NODE_ITEM_ID].ToString(), out item_id))
                    {
                        UserItemData item = FindUserItem((ITEM_TYPE_V2)item_type, item_id);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddUserItemData((ITEM_TYPE_V2)item_type, item_id);
                            item.Deserialized(jdata);
                        }
                    }
                }
            }
        }
        InitUpdateData();

        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_ITEM_DATA_LIST = "ilist";

    protected const string NODE_ITEM_TYPE = "itype";
    protected const string NODE_ITEM_ID = "iid";



}

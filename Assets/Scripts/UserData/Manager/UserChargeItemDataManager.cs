using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserChargeItemDataManager : ManagerBase
{
    List<UserChargeItemData> User_Charge_Item_Data_List= new List<UserChargeItemData>();
    public UserChargeItemDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }

    protected override void Destroy()
    {
        int cnt = User_Charge_Item_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Charge_Item_Data_List[i].Dispose();
        }
        User_Charge_Item_Data_List.Clear();
    }

    public override void InitDataManager()
    {
        if (User_Charge_Item_Data_List.Count > 0)
        {
            return;
        }

        var charge_data_list = MasterDataManager.Instance.Get_ChargeValueDataList();
        int cnt = charge_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var charge_item = charge_data_list[i];
            AddUserChargeItemData(charge_item.reward_type);
        }
        Save();
    }

    public UserChargeItemData FindUserChargeItemData(REWARD_TYPE rtype)
    {
        return User_Charge_Item_Data_List.Find(x => x.Charge_Item_Type == rtype);
    }

    UserChargeItemData AddUserChargeItemData(REWARD_TYPE rtype)
    {
        var item = FindUserChargeItemData(rtype);
        if (item == null)
        {
            item = new UserChargeItemData();
            item.SetRewardType(rtype);
            User_Charge_Item_Data_List.Add(item);
            Is_Update_Data = true;
        }
        return item;
    }

    /// <summary>
    /// 해당 아이템 사용 가능 여부
    /// </summary>
    /// <param name="rtype"></param>
    /// <param name="cnt"></param>
    /// <returns></returns>
    public bool IsUsableChargeItemCount(REWARD_TYPE rtype, int cnt)
    {
        var item = FindUserChargeItemData(rtype);
        if (item != null)
        {
            return item.IsUsableChargeItemCount(cnt);
        }
        return false;
    }


    public override JsonData Serialized()
    {
        var json = base.Serialized();

        var arr = new JsonData();

        int cnt = User_Charge_Item_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Charge_Item_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_CHARGE_ITEM_DATA_LIST] = arr;
        }
        if (json.Keys.Count > 0)
        {
            return json;
        }

        return null;
    }
    public override bool Deserialized(JsonData json)
    {
        if (!base.Deserialized(json))
        {
            return false;
        }

        if (json.ContainsKey(NODE_CHARGE_ITEM_DATA_LIST))
        {
            var arr = json[NODE_CHARGE_ITEM_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];

                    int charge_item_type = 0;
                    if (int.TryParse(jdata[NODE_CHARGE_ITEM_TYPE].ToString(), out charge_item_type))
                    {
                        UserChargeItemData item = FindUserChargeItemData((REWARD_TYPE)charge_item_type);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddUserChargeItemData((REWARD_TYPE)charge_item_type);
                            item?.Deserialized(jdata);
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
    protected const string NODE_CHARGE_ITEM_DATA_LIST = "clist";

    protected const string NODE_CHARGE_ITEM_TYPE = "ctype";
}

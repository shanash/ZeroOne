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
        }
        return item;
    }


    public override JsonData Serialized()
    {
        return base.Serialized();
    }
    public override bool Deserialized(JsonData json)
    {
        return base.Deserialized(json);
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_CHARGE_ITEM_DATA_LIST = "clist";

    protected const string NODE_CHARGE_ITEM_TYPE = "ctype";
}

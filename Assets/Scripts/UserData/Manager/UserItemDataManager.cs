using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class UserItemDataManager : ManagerBase
{
    public UserItemDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }

    protected override void Destroy()
    {

    }

    public override void InitDataManager()
    {

    }

    void DummyDataSetting() { }

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

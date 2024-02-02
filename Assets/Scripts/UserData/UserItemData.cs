using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItemData : UserDataBase
{
    public ITEM_TYPE_V2 Item_Type { get; private set; } = ITEM_TYPE_V2.NONE;
    public int Item_ID { get; private set; } = 0;
    protected SecureVar<double> Count = null;


    public UserItemData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Item_Type = ITEM_TYPE_V2.NONE;
        Item_ID = 0;
    }

    protected override void InitSecureVars()
    {
        if (Count == null)
        {
            Count = new SecureVar<double>();
        }
    }

    public void SetItemType(ITEM_TYPE_V2 itype, int item_id)
    {
        Item_Type = itype;
        Item_ID = item_id;
        InitMasterData();
    }

    

    /// <summary>
    /// 각 아이템 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetItemData() {  return null; }

    public double GetCount()
    {
        return Count.Get();
    }

    public ERROR_CODE AddItemCount(double add_cnt)
    {
        ERROR_CODE code = ERROR_CODE.FAILED;
        if (add_cnt < 0)
        {
            return code;
        }
        double cnt = GetCount();
        cnt += add_cnt;
        Count.Set(cnt);
        Is_Update_Data = true;
        return ERROR_CODE.SUCCESS;
    }
    public ERROR_CODE UseItemCount(double use_cnt)
    {

        if (!IsUsableItemCount(use_cnt))
        {
            return ERROR_CODE.NOT_ENOUGH_ITEM;
        }
        if (use_cnt >= 0)
        {
            double cnt = GetCount();
            cnt -= use_cnt;
            Count.Set(cnt);
            Is_Update_Data = true;
            return ERROR_CODE.SUCCESS;
        }

        return ERROR_CODE.FAILED;
    }
    public bool IsUsableItemCount(double cnt)
    {
        if (cnt < 0)
        {
            return false;
        }
        return GetCount() >= cnt;
    }

    public override JsonData Serialized()
    {
        //if (!IsUpdateData())
        //{
        //    return null;
        //}
        var json = new JsonData();

        json[NODE_ITEM_TYPE] = (int)Item_Type;
        json[NODE_ITEM_ID] = Item_ID;
        json[NODE_COUNT] = GetCount();

        return json;
    }
    public override bool Deserialized(JsonData json)
    {
        if (json == null) return false;

        InitSecureVars();

        if (json.ContainsKey(NODE_ITEM_TYPE))
        {
            Item_Type = (ITEM_TYPE_V2)ParseInt(json, NODE_ITEM_TYPE);
        }
        if (json.ContainsKey(NODE_ITEM_ID))
        {
            Item_ID = ParseInt(json, NODE_ITEM_ID);
        }
        if (json.ContainsKey(NODE_COUNT))
        {
            Count.Set(ParseDouble(json, NODE_COUNT));
        }

        InitMasterData();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_ITEM_TYPE = "itype";
    protected const string NODE_ITEM_ID = "iid";
    protected const string NODE_COUNT = "cnt";
}

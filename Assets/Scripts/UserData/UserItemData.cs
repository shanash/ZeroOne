using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItemData : UserDataBase
{
    public ITEM_TYPE_V2 Item_Type { get; private set; } = ITEM_TYPE_V2.NONE;
    public int Item_ID { get; private set; } = 0;
    SecureVar<double> Count = null;

    
    Item_Data Data = null;

    public UserItemData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Item_Type = ITEM_TYPE_V2.NONE;
        Item_ID = 0;
        //Count.Set(0);
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

    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        
        switch (Item_Type)
        {
            case ITEM_TYPE_V2.EXP_POTION_P:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.EXP_POTION_C:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.STA_POTION:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.FAVORITE_ITEM:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.EXP_SKILL:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.STAGE_SKIP:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.TICKET_DUNGEON:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.EQ_GROWUP:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            case ITEM_TYPE_V2.TICKET_REWARD_SELECT:
                Debug.Assert(false);
                break;
            case ITEM_TYPE_V2.TICKET_REWARD_RANDOM:
                Debug.Assert(false);
                break;
            case ITEM_TYPE_V2.TICKET_REWARD_ALL:
                Debug.Assert(false);
                break;
            case ITEM_TYPE_V2.EQUIPMENT:
                Debug.Assert(false);
                break;
            case ITEM_TYPE_V2.CHARACTER:
                Debug.Assert(false);
                break;
            case ITEM_TYPE_V2.PIECE_EQUIPMENT:
                Debug.Assert(false);
                break;
            case ITEM_TYPE_V2.PIECE_CHARACTER:
                Debug.Assert(false);
                break;
            case ITEM_TYPE_V2.PIECE_ITEM:
                Data = m.Get_ItemData(Item_Type, Item_ID);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

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
        ERROR_CODE code = ERROR_CODE.FAILED;

        if (!IsUsableItemCount(use_cnt))
        {
            return ERROR_CODE.NOT_ENOUGH_ITEM;
        }
        if (use_cnt > 0)
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

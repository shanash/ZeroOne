
using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGoodsData : UserDataBase
{
    public GOODS_TYPE Goods_Type { get; private set; } = GOODS_TYPE.NONE;
    Goods_Data Data = null;

    SecureVar<double> Count = null;

    public UserGoodsData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Goods_Type = GOODS_TYPE.NONE;
        //Count.Set(0);
    }

    protected override void InitSecureVars()
    {
        if (Count == null)
        {
            Count = new SecureVar<double>();
        }
    }

    public void SetGoodsType(GOODS_TYPE gtype)
    {
        Goods_Type = gtype;
        InitMasterData();
        Is_Update_Data = true;
    }

    protected override void InitMasterData()
    {
        Data = MasterDataManager.Instance.Get_GoodsData(Goods_Type);
    }


    public double GetMaxBound()
    {
        if (Data != null)
        {
            return Data.max_bound;
        }
        return 0;
    }
    public double GetCount()
    {
        return Count.Get();
    }

    /// <summary>
    /// 최대값을 초과하는지 여부 반환
    /// </summary>
    /// <param name="cnt"></param>
    /// <returns></returns>
    public bool IsEnableAddGoodsCount(double cnt)
    {
        bool is_enable = true;
        if (GetMaxBound() > 0)
        {
            double result_cnt = GetCount() + cnt;
            if (result_cnt > GetMaxBound())
            {
                is_enable = false;
            }
        }
        return is_enable;
    }

    public bool IsUsableGoodsCount(double cnt)
    {
        if (cnt < 0)
        {
            return false;
        }
        return GetCount() >= cnt;
    }
    /// <summary>
    /// 재화 개수 추가
    /// </summary>
    /// <param name="cnt"></param>
    /// <param name="over_apply">max bound를 무시하고 추가 가능한 경우 추가</param>
    /// <returns></returns>
    public ERROR_CODE AddGoodsCount(double cnt, bool over_apply = true)
    {
        ERROR_CODE code = ERROR_CODE.SUCCESS;
        if (cnt >= 0)
        {
            if (!over_apply)
            {
                if (!IsEnableAddGoodsCount(cnt))
                {
                    return ERROR_CODE.OVER_MAX_ITEM_BOUND;
                }
            }

            double g_count = GetCount();
            g_count += cnt;
            if (!over_apply)
            {
                double max_bound = GetMaxBound();
                if (max_bound > 0)
                {
                    if (g_count > max_bound)
                    {
                        g_count = max_bound;
                    }
                }
            }
            
            Count.Set(g_count);
            Is_Update_Data = true;
            
        }

        return code;
    }

    public ERROR_CODE UseGoods(double cnt)
    {
        if (!IsUsableGoodsCount(cnt))
        {
            return ERROR_CODE.NOT_ENOUGH_ITEM;
        }
        if (cnt > 0)
        {
            double g_count = GetCount();
            g_count -= cnt;
            Count.Set(g_count);
            Is_Update_Data = true;
            return ERROR_CODE.SUCCESS;
        }

        return ERROR_CODE.FAILED;
    }

    public override JsonData Serialized()
    {
        var json = new JsonData();
        json[NODE_GOODS_TYPE] = (int)Goods_Type;
        json[NODE_COUNT] = GetCount();
        return json;
    }

    public override bool Deserialized(JsonData json)
    {
        if(json == null) {  return false; }

        InitSecureVars();

        if (json.ContainsKey(NODE_GOODS_TYPE))
        {
            Goods_Type = (GOODS_TYPE)ParseInt(json, NODE_GOODS_TYPE);
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
    protected const string NODE_GOODS_TYPE = "gtype";
    protected const string NODE_COUNT = "cnt";
}

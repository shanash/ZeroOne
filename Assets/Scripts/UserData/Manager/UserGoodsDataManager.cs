using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGoodsDataManager : ManagerBase
{
    List<UserGoodsData> User_Goods_Data_List = new List<UserGoodsData>();
    public UserGoodsDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }

    protected override void Destroy()
    {
        
    }

    public override void InitDataManager()
    {
        
    }

    void DummyDataSetting() { }

    public UserGoodsData FindUserGoods(GOODS_TYPE gtype)
    {
        return User_Goods_Data_List.Find(x => x.Goods_Type == gtype);
    }

    public bool IsUsableGoodsCount(GOODS_TYPE gtype, int cnt)
    {
        var goods = FindUserGoods(gtype);
        if (goods != null)
        {
            return goods.IsUsableGoodsCount(cnt);
        }
        return false;
    }

    public UserGoodsData AddUserGoodsCount(GOODS_TYPE gtype, int cnt)
    {
        UserGoodsData goods = FindUserGoods(gtype);
        if (goods != null)
        {
            goods.AddGoodsCount(cnt);
        }
        else
        {
            goods = new UserGoodsData();
            goods.SetGoodsType(gtype);
            goods.AddGoodsCount(cnt);
            User_Goods_Data_List.Add(goods);
        }
        return goods;
    }

    public ERROR_CODE UseGoodsCount(GOODS_TYPE gtype, int cnt)
    {
        if (cnt < 0)
        {
            return ERROR_CODE.FAILED;
        }

        var goods = FindUserGoods(gtype);
        if (goods != null)
        {
            return goods.UseGoods(cnt);
        }
        return ERROR_CODE.FAILED;
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
    protected const string NODE_GOODS_DATA_LIST = "glist";


}

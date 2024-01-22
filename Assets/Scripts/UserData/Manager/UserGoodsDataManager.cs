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
        int cnt = User_Goods_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Goods_Data_List[i].Dispose();
        }
        User_Goods_Data_List.Clear();
    }

    public override void InitDataManager()
    {
        if (User_Goods_Data_List.Count > 0)
        {
            return;
        }
        DummyDataSetting();
    }

    void DummyDataSetting() 
    {
        //  gold
        {
            AddUserGoodsCount(GOODS_TYPE.GOLD, 1000);
        }
        //  dia
        {
            AddUserGoodsCount(GOODS_TYPE.DIA, 100);
        }

    }

    public UserGoodsData FindUserGoods(GOODS_TYPE gtype)
    {
        return User_Goods_Data_List.Find(x => x.Goods_Type == gtype);
    }

    public double GetGoodsCount(GOODS_TYPE gtype)
    {
        var goods = FindUserGoods(gtype);
        if (goods != null)
        {
            return goods.GetCount();
        }
        return 0;
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

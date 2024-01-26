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
            var gold = AddUserGoodsData(GOODS_TYPE.GOLD);
            if (gold != null)
            {
                gold.AddGoodsCount(1000);
            }
        }
        //  dia
        {
            var dia = AddUserGoodsData(GOODS_TYPE.DIA);
            if (dia != null)
            {
                dia.AddGoodsCount(100);
            }
        }
        Save();

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

    public bool IsUsableGoodsCount(GOODS_TYPE gtype, double cnt)
    {
        var goods = FindUserGoods(gtype);
        if (goods != null)
        {
            return goods.IsUsableGoodsCount(cnt);
        }
        return false;
    }

    UserGoodsData AddUserGoodsData(GOODS_TYPE gtype)
    {
        UserGoodsData goods = FindUserGoods(gtype);
        if (goods == null)
        {
            goods = new UserGoodsData();
            goods.SetGoodsType(gtype);
            User_Goods_Data_List.Add(goods);
        }
        return goods;
    }

    public ERROR_CODE AddUserGoodsCount(GOODS_TYPE gtype, double cnt)
    {
        UserGoodsData goods = FindUserGoods(gtype);
        if (goods == null)
        {
            goods = AddUserGoodsData(gtype);
        }
        
        return goods.AddGoodsCount(cnt);
    }

    public ERROR_CODE UseGoodsCount(GOODS_TYPE gtype, double cnt)
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
        var json = new JsonData();

        var arr = new JsonData();

        int cnt = User_Goods_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Goods_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_GOODS_DATA_LIST] = arr;
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

        if (json.ContainsKey(NODE_GOODS_DATA_LIST))
        {
            var arr = json[NODE_GOODS_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];
                    int goods_type = 0;
                    if (int.TryParse(jdata[NODE_GOODS_TYPE].ToString(), out goods_type))
                    {
                        UserGoodsData item = FindUserGoods((GOODS_TYPE) goods_type);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddUserGoodsData((GOODS_TYPE)goods_type);
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
    protected const string NODE_GOODS_DATA_LIST = "glist";

    protected const string NODE_GOODS_TYPE = "gtype";
}

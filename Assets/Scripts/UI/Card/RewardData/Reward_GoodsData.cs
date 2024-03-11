using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_GoodsData : RewardDataBase
{
    protected GoodsDataBase Data;

    public override string RewardItemName => (Data != null) ?
        GameDefine.GetLocalizeString(Data.GetGoodsData().name_id)
        : string.Empty;

    public override string RewardItemDesc => (Data != null) ?
    GameDefine.GetLocalizeString(Data.GetGoodsData().desc_id)
    : string.Empty;

    protected override void InitData()
    {
        Data = new GoodsDataBase();
        if (Reward.reward_type == REWARD_TYPE.GOLD)
        {
            Data.SetGoods(GOODS_TYPE.GOLD);
        }
        else if (Reward.reward_type == REWARD_TYPE.DIA)
        {
            Data.SetGoods(GOODS_TYPE.DIA);
        }
        else
        {
            Debug.Assert(false);
        }
    }

    public override object GetRewardItemData()
    {
        return Data;
    }

    public override string GetRewardItemIconPath()
    {
        if (Data != null)
        {
            return Data.GetGoodsIconPath();
        }
        return null;
    }
}

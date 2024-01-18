using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_GoodsData : RewardDataBase
{
    protected GoodsDataBase Data;


    protected override void InitData()
    {
        
    }

    public override object GetRewardItemData()
    {
        return Data;
    }

    public override string GetRewardItemIconPath()
    {
        return Data.GetGoodsIconPath();
    }
}

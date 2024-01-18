
public class Reward_ItemData : RewardDataBase
{
    protected ItemDataBase Data;


    protected override void InitData()
    {

    }

    public override object GetRewardItemData()
    {
        return Data;
    }

    public override string GetRewardItemIconPath()
    {
        return Data.GetItemIconPath();
    }
}

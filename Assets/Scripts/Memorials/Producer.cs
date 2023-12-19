using UnityEngine;

public class Producer : FactoryProductBase
{
    public static Producer Create(int memorial_id, MEMORIAL_TYPE type, Transform contents_parent)
    {
        return Factory.Create<Producer>(memorial_id, type, contents_parent);
    }

    ActressBase Actress = null;
    MemorialBackground Background = null;
    MEMORIAL_TYPE Type = MEMORIAL_TYPE.NONE;


    Producer() { }

    protected override bool Initialize(params object[] args)
    {
        if (args.Length != 3 || args[0] is not int || args[1] is not MEMORIAL_TYPE || args[2] is not Transform)
        {
            return false;
        }

        int memorial_id = (int)args[0];
        Type = (MEMORIAL_TYPE)args[1];
        Transform tf = (Transform)args[2];

        var data = MasterDataManager.Instance.Get_MemorialData(memorial_id);

        Actress = ActressBase.Create(this, data, tf);
        Background = MemorialBackground.Create(this, data, tf);

        return true;
    }
}

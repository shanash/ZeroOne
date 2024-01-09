using FluffyDuck.Util;
using System.Threading.Tasks;
using UnityEngine;

public class Producer : FluffyDuck.Util.Factory.IProduct
{
    StageBase Stage = null;
    Background Background = null;
    ActorBase Actor = null;

    MEMORIAL_TYPE Type = MEMORIAL_TYPE.NONE;

    Producer() { }

    bool Initialize(int memorial_id, MEMORIAL_TYPE type, Transform parent)
    {
        Type = type;
        var data = MasterDataManager.Instance.Get_MemorialData(memorial_id);
        _ = InitializeAsync(data, Type, parent);

        return true;
    }

    async Task InitializeAsync(Me_Resource_Data data, MEMORIAL_TYPE type, Transform parent)
    {
        Stage = await MonoFactory.CreateAsync<StageBase>("Assets/AssetResources/Prefabs/Memorial/StageBase", parent);
        Actor = await MonoFactory.CreateAsync<ActorBase>(data.actor_prefab_key, Stage.Actor_Parent, this, data);
        Background = await MonoFactory.CreateAsync<Background>(data.background_prefab_key, Stage.Background_Parent, data);
    }

    public void Pause()
    {
        Actor.Pause(true);
    }

    public void Resume()
    {
        Actor.Pause(false);
    }
}

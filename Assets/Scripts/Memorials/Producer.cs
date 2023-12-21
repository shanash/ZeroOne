using System.Threading.Tasks;
using UnityEngine;

namespace FluffyDuck.Memorial
{
    public class Producer
    {
        StageBase Stage = null;
        Background Background = null;
        ActorBase Actor = null;

        MEMORIAL_TYPE Type = MEMORIAL_TYPE.NONE;

        Producer() { }

        bool Initialize(int memorial_id, MEMORIAL_TYPE type, Transform tf)
        {
            Type = type;
            var data = MasterDataManager.Instance.Get_MemorialData(memorial_id);
            _ = InitializeAsync(data, Type, tf);

            return true;
        }

        async Task InitializeAsync(Me_Resource_Data data, MEMORIAL_TYPE type, Transform tf)
        {
            Stage = await MBFactory.CreateAsync<StageBase>("Assets/AssetResources/Prefabs/Memorial/StageBase", tf);
            Actor = await MBFactory.CreateAsync<ActorBase>(data.actor_prefab_key, Stage.Actor_Parent, this, data);
            Background = await MBFactory.CreateAsync<Background>(data.background_prefab_key, Stage.Background_Parent, data);
        }
    }
}

using FluffyDuck.Util;
using System.Threading.Tasks;
using UnityEngine;

public class Producer : FluffyDuck.Util.Factory.IProduct
{
    StageBase Stage = null;
    Background Background = null;
    ActorBase Actor = null;

    MEMORIAL_TYPE Type = MEMORIAL_TYPE.NONE;

    public bool Is_Init => (Stage != null && Actor != null);

    Producer() { }

    bool Initialize(int memorial_id, MEMORIAL_TYPE type, Transform parent)
    {
        Type = type;
        
        var data = MasterDataManager.Instance.Get_L2DCharSkinData(memorial_id);
        _ = InitializeAsync(data, Type, parent);

        return true;
    }

    bool Initialize(int memorial_id, MEMORIAL_TYPE type)
    {
        return Initialize(memorial_id, type, null);
    }

    async Task InitializeAsync(L2d_Char_Skin_Data data, MEMORIAL_TYPE type, Transform parent)
    {
        Stage = await MonoFactory.CreateAsync<StageBase>("Assets/AssetResources/Prefabs/Memorial/StageBase", parent);
        Actor = await MonoFactory.CreateAsync<ActorBase>(data.l2d_skin_path, Stage.Actor_Parent, this, data, LOVE_LEVEL_TYPE.NORMAL);
        /*
        if (!string.IsNullOrEmpty(data.l2d_bg_path))
        {
            Background = await MonoFactory.CreateAsync<Background>(data.l2d_bg_path, Stage.Background_Parent, data);
        }
        */

        if (type == MEMORIAL_TYPE.MEMORIAL)
        {
            GameObjectUtils.ChangeLayersRecursively(Stage.transform, "OverlayObj");
        }
    }

    public void Pause()
    {
        Actor.Pause(true);
    }

    public void Resume()
    {
        Actor.Pause(false);
    }

    public void Release()
    {
        GameObject.Destroy(Actor.gameObject);
        if (Background != null)
        {
            GameObject.Destroy(Background.gameObject);
        }
        GameObject.Destroy(Stage.gameObject);
    }
}

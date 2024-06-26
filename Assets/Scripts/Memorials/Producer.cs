using FluffyDuck.Util;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class Producer : FluffyDuck.Util.Factory.IProduct
{
    static int Index = 0;

    StageBase Stage = null;
    ActorBase Actor = null;

    GameObject root = null;

    SPINE_CHARA_LOCATION_TYPE Type = SPINE_CHARA_LOCATION_TYPE.NONE;

    public bool Is_Init => (Stage != null && Actor != null);
    public string Name => root != null ? root.name : Index.ToString();

    public delegate void TransferEssenceHandler(bool is_success, TOUCH_BODY_TYPE type);

    public event Action<string, float, bool> OnSendActorMessage { add => Actor.OnSendMessage += value; remove => Actor.OnSendMessage -= value; }
    public event Action<bool, TOUCH_BODY_TYPE> OnResultTransferEssence { add => Actor.OnResultTransferEssence += value; remove => Actor.OnResultTransferEssence -= value; }
    public event Action OnCompleteTransferEssence { add => Actor.OnCompleteTransferEssence += value; remove => Actor.OnCompleteTransferEssence -= value; }

    Producer() { }

    bool Initialize(int skin_id, LOVE_LEVEL_TYPE selected_relationship, SPINE_CHARA_LOCATION_TYPE type, Transform parent)
    {
        Type = type;
        
        var data = MasterDataManager.Instance.Get_L2DCharSkinData(skin_id);
        _ = InitializeAsync(data, selected_relationship, Type, parent);

        return true;
    }

    bool Initialize(int memorial_id, LOVE_LEVEL_TYPE selected_relationship, SPINE_CHARA_LOCATION_TYPE type)
    {
        return Initialize(memorial_id, selected_relationship, type, null);
    }

    async Task InitializeAsync(L2d_Char_Skin_Data data, LOVE_LEVEL_TYPE selected_relationship, SPINE_CHARA_LOCATION_TYPE type, Transform parent)
    {
        ScreenEffectManager.I.SetBlockInputUI(true);
        Stage = await MonoFactory.CreateAsync<StageBase>("Assets/AssetResources/Prefabs/Memorial/StageBase", parent);
        Stage.gameObject.name = $"{Stage.gameObject.name}_{Index}_{data.l2d_id}";
        Index++;
        root = Stage.gameObject;

        Actor = await MonoFactory.CreateAsync<ActorBase>(data.l2d_skin_path, Stage.Actor_Parent, this, data, selected_relationship, type);

        if (type == SPINE_CHARA_LOCATION_TYPE.HERO_INFO || type == SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE)
        {
            GameObjectUtils.ChangeLayersRecursively(Stage.transform, "OverlayObj");
        }
        else if( type == SPINE_CHARA_LOCATION_TYPE.LOBBY_EXPECT )
        {
            Stage.transform.localPosition  = new Vector3(0, 1, 0);
            GameObjectUtils.ChangeLayersRecursively(Stage.transform, "OverlayObj");
        }

        ScreenEffectManager.I.SetBlockInputUI(false);
    }

    public void SetEssenceBodyPart(TOUCH_BODY_TYPE type = TOUCH_BODY_TYPE.NONE)
    {
        Actor.SetEssenceBodyPart(type);
    }

    public void SetActive(bool value)
    {
        root.SetActive(value);
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
        GameObject.Destroy(root);
    }
}

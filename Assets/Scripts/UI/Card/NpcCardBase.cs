using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcCardBase : UIBase
{
    [SerializeField, Tooltip("BG Box")]
    protected Image BG_Box;

    [SerializeField, Tooltip("Npc Icon")]
    protected Image Npc_Icon;

    [SerializeField, Tooltip("Boss Tag")]
    protected Image Boss_Tag;

    [SerializeField, Tooltip("Warning Tag")]
    protected Image Warning_Tag;

    protected Npc_Data Data;
    protected bool Is_Boss;

    Vector2 Init_Scale = new Vector2(0.66f, 0.66f);

    public void SetNpcID(int npc_id)
    {
        Data = MasterDataManager.Instance.Get_NpcData(npc_id);

    }
    public void SetBoss(bool boss)
    {
        Is_Boss = boss;
        Boss_Tag.gameObject.SetActive(Is_Boss);
    }

    protected void UpdateNpcIcon()
    {
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.icon_path, (spr) =>
        {
            Npc_Icon.sprite = spr;
        });
    }

    public int GetNpcID()
    {
        if (Data != null)
        {
            return Data.npc_data_id;
        }
        return 0;
    }
    public override void Spawned()
    {
        base.Spawned();
        this.transform.localScale = Init_Scale;
    }
    public override void Despawned()
    {
        base.Despawned();
        SetBoss(false);
    }

}

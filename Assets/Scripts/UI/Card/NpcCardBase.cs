using FluffyDuck.Util;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [SerializeField, Tooltip("Button")]
    public UIInteractiveButton TooltipButton;

    protected Npc_Data Data;
    protected bool Is_Boss;

    Vector2 Init_Scale = new Vector2(0.66f, 0.66f);

    public Action<Rect, Npc_Data> OnStartLongPress;
    public Action OnFinishLongPress;

    public void SetNpcID(int npc_id)
    {
        SetNpcData(MasterDataManager.Instance.Get_NpcData(npc_id));
    }

    public void SetNpcData(Npc_Data data)
    {
        Data = data;
        UpdateNpcIcon();
        if (TooltipButton != null)
        {
            TooltipButton.Tooltip_Data = Data;
        }
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

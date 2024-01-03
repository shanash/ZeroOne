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

    public void SetNpcID(int npc_id)
    {
        Data = MasterDataManager.Instance.Get_NpcData(npc_id);

    }
    public void SetBoss(bool boss)
    {
        Is_Boss = boss;
    }

    protected void UpdateNpcIcon()
    {
        CommonUtils.GetResourceFromAddressableAsset<Sprite>("", (obj) =>
        {

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

}

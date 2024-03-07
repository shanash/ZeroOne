using UnityEngine;

public class TooltipMonster : Tooltip
{
    [SerializeField]
    NpcCardBase Npc_Card = null;

    public void Initialize(Rect hole, Npc_Data npc_data)
    {
        Initialize(hole, GameDefine.GetLocalizeString(npc_data.name_id), GameDefine.GetLocalizeString(npc_data.desc_id));
        Npc_Card.SetNpcData(npc_data);
    }
}

using UnityEngine;

public class MonsterTooltip : TooltipBase
{
    [SerializeField]
    NpcCardBase Npc_Card = null;

    public void Initialize(Rect hole, Npc_Data npc_data)
    {
        Initialize(
            hole,
            (npc_data != null) ? GameDefine.GetLocalizeString(npc_data.name_id) : "EMPTY",
            (npc_data != null) ? GameDefine.GetLocalizeString(npc_data.desc_id) : "EMPTY");

        Npc_Card.SetNpcData(npc_data);
    }
}

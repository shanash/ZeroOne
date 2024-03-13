using UnityEngine;

public class MonsterTooltip : TooltipBase
{
    [SerializeField]
    NpcCardBase Npc_Card = null;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2 || data[0] is not Rect)
        {
            return false;
        }

        Npc_Data npc_data = null;
        if (data[1] != null && data[1] is Npc_Data)
        {
            npc_data = data[1] as Npc_Data;
        }

        Initialize((Rect)data[0], npc_data);

        return true;
    }

    public void Initialize(Rect hole, Npc_Data npc_data)
    {
        Initialize(
            hole,
            (npc_data != null) ? GameDefine.GetLocalizeString(npc_data.name_id) : "EMPTY",
            (npc_data != null) ? GameDefine.GetLocalizeString(npc_data.desc_id) : "EMPTY");

        Npc_Card.SetNpcData(npc_data);
    }
}

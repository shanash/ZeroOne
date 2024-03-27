using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextNode : DamageTextBase
{
    [SerializeField, Tooltip("Ease Random Mover")]
    DamageTextMover Mover;

    public override void ShowText(BATTLE_SEND_DATA send_data, DAMAGE_TYPE dtype, Vector3 target_pos, DAMAGE_DIRECTION dir)
    {
        base.ShowText(send_data, dtype, target_pos, dir);
        MoverStart(UIEaseBase.MOVE_TYPE.MOVE_OUT, dtype, dir);
    }

    public override void ShowText(Total_Damage_Data total, Vector3 target_pos)
    {
        base.ShowText(total, target_pos);
    }

    void MoverStart(UIEaseBase.MOVE_TYPE mtype, DAMAGE_TYPE dtype, DAMAGE_DIRECTION dir)
    {
        if (mtype == UIEaseBase.MOVE_TYPE.NONE)
        {
            return;
        }
        if (Mover != null)
        {
            if (dtype == DAMAGE_TYPE.WEAK)
            {
                Mover.StartMove(mtype, dir, 1.5f);
            }
            else if (dtype == DAMAGE_TYPE.CRITICAL)
            {
                Mover.StartMove(mtype, dir, 1.75f);
            }
            else if (dtype == DAMAGE_TYPE.WEAK_CRITICAL)
            {
                Mover.StartMove(mtype, dir, 2f);
            }
            else
            {
                Mover.StartMove(mtype, dir);
            }
            
        }
    }
}

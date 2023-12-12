using FluffyDuck.Util;
using Spine;
using System.Collections.Generic;
using UnityEngine;

public class ActressLucia : ActressBase
{
    const string BORED = "bored";
    int Idle_Animation_Played_Count = 0;

    protected override void SpineAnimationStart(TrackEntry entry)
    {
        base.SpineAnimationStart(entry);

        if (Current_State_Id != 50000003)
        {
            return;
        }

        if (!entry.Animation.Name.Equals(Idle_Animation) && !entry.Animation.Name.Contains(BORED))
        {
            Debug.Log("reset Idle_Animation_Played_Count");
            Idle_Animation_Played_Count = 0;
        }
    }

    protected override void SpineAnimationInterrupt(TrackEntry entry)
    {
        base.SpineAnimationInterrupt(entry);
    }

    protected override void SpineAnimationComplete(TrackEntry entry)
    {
        base.SpineAnimationComplete(entry);

        if (Current_State_Id != 50000003)
        {
            return;
        }

        if (entry.Animation.Name.Equals(Idle_Animation))
        {
            Idle_Animation_Played_Count++;
            Debug.Log($"count : {Idle_Animation_Played_Count}");

            if (Idle_Animation_Played_Count % 3 == 0)
            {
                PlayAnimationForChatMotion(1200002000 + Idle_Animation_Played_Count / 3);

                if (Idle_Animation_Played_Count == 9)
                {
                    Debug.Log("reset Idle_Animation_Played_Count");
                    Idle_Animation_Played_Count = 0;
                }
            }
        }
    }

    protected override void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        base.SpineAnimationEvent(entry, evt);
    }
}

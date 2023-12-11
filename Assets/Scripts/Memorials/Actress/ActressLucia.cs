using FluffyDuck.Util;
using Spine;
using System.Collections.Generic;
using UnityEngine;

public class ActressLucia : ActressBase
{
    const string BORED = "bored";
    int Idle_Animation_Played_Count = 0;
    int played_animation_drag_id = 0;

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

    protected override int GetInteractionIndex(SpineBoundingBox bounding_box, List<Me_Interaction_Data> interaction_datas, TOUCH_GESTURE_TYPE gesture_type, Vector2 screen_pos, int drag_id)
    {
        int index = base.GetInteractionIndex(bounding_box, interaction_datas, gesture_type, screen_pos, drag_id);

        Debug.Log($"index : {index}");

        if (index == -1)
        {
            return index;
        }

        if (gesture_type != TOUCH_GESTURE_TYPE.DRAG)
        {
            return index;
        }

        if (played_animation_drag_id == drag_id)
        {
            return -1;
        }

        // 드래그 디테일 구현
        Vector2 drag_dest = bounding_box.GetPtDirection();

        if (drag_dest.Equals(Vector2.zero))
        {
            return -1;
        }

        if (screen_pos.sqrMagnitude.Equals(0.0f))
        {
            screen_pos = drag_dest * 0.001f;
        }
        
        float close_value = // 드래그한 정도가 얼만큼 drag_dest와 일치했는가 (0~1 이외의 값은 크게 의미 없다)
            (screen_pos.sqrMagnitude / drag_dest.sqrMagnitude) // 드래그한 길이가 얼마나 대상과 비슷한가
            * CommonUtils.Math.Cos(drag_dest, screen_pos); // 드래그한 방향이 얼마나 대상과 비슷한가

        if (string.IsNullOrEmpty(interaction_datas[index].drag_animation_name))
        {
            Debug.Log("Play");
            played_animation_drag_id = drag_id;

            return index;
        }

        string drag_anim_name = interaction_datas[index].drag_animation_name;
        if (TryGetTrackNum(drag_anim_name, out int track_num))
        {
            if (close_value < 1)
            {
                var te = Skeleton.AnimationState.GetCurrent(track_num);
                if (te == null)
                {
                    te = Skeleton.AnimationState.SetAnimation(track_num, drag_anim_name, false);
                }

                if (!te.Animation.Name.Equals(drag_anim_name))
                {
                    te = Skeleton.AnimationState.SetAnimation(track_num, drag_anim_name, false);
                }

                te.TimeScale = 0.0f;
                te.TrackTime = close_value;
            }
            else
            {
                Skeleton.AnimationState.SetEmptyAnimation(track_num, 0.0f);

                played_animation_drag_id = drag_id;
                return index;
            }
        }
        return -1;
    }
}

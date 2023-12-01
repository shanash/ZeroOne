using FluffyDuck.Util;
using Spine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActressSample : ActressBase
{
    protected override void SpineAnimationComplete(TrackEntry entry)
    {
        base.SpineAnimationComplete(entry);
    }

    protected override void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        base.SpineAnimationEvent(entry, evt);
    }

    protected override int[] GetInteractionIndexes(SpineBoundingBox bounding_box, List<Me_Interaction_Data> interaction_datas, TOUCH_GESTURE_TYPE gesture_type, Vector2 screen_pos)
    {
        int[] available_indexes = base.GetInteractionIndexes(bounding_box, interaction_datas, gesture_type, screen_pos);

        if (gesture_type != TOUCH_GESTURE_TYPE.DRAG)
        {
            return available_indexes;
        }

        // 드래그 디테일 구현
        Vector2 drag_dest = bounding_box.GetPtDirection();

        if (drag_dest.Equals(Vector2.zero))
        {
            return null;
        }

        Debug.Log($"screen_pos : {screen_pos}");
        Debug.Log($"drag_dest : {drag_dest}");
        
        float close_value = // 드래그한 정도가 얼만큼 drag_dest와 일치했는가 (0~1 이외의 값은 크게 의미 없다)
            (screen_pos.sqrMagnitude / drag_dest.sqrMagnitude) // 드래그한 길이가 얼마나 대상과 비슷한가
            * CommonUtils.Math.Cos(screen_pos, drag_dest); // 드래그한 방향이 얼마나 대상과 비슷한가

        if (close_value < 1)
        {
            Skeleton.Skeleton.FindPathConstraint($"path_");
            // 낮은 수치에서는 오브젝트를 계속 움직여주겠지
            return null;
        }

        return available_indexes;
    }
}

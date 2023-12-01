using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public struct EventTriggerValue
{
    public int IntValue;
    public double DoubleValue;
    public float FloatValue;
    public string StrValue;
}

[Serializable]
public class EventTriggerBehaviour : PlayableBehaviour
{
    /// <summary>
    /// 해당 타임라인 클립
    /// </summary>
    TimelineClip Clip;
    /// <summary>
    /// 트리거 이벤트를 전달할 오브젝트
    /// </summary>
    EventTriggerObject Trigger_Obj;
    /// <summary>
    /// 트리거 값을 담고 있는 구조체. 
    /// 호출되는 시점에 필요한 값을 저장하고 전달해주는 역할
    /// </summary>
    public EventTriggerValue Trigger_Value = default(EventTriggerValue);


    /// <summary>
    /// 클립이 플레이되는 시점에 최초 1회만 호출
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        if (Trigger_Obj != null)
        {
            var evt_clip = GetTriggerClip();
            if (evt_clip != null)
            {
                Trigger_Obj.SendEventTrigger(evt_clip.GetTriggerName(), Trigger_Value);
            }
        }
    }


    public void SetTimelineClip(TimelineClip c)
    {
        Clip = c;
    }

    public EventTriggerClip GetTriggerClip()
    {
        if (Clip == null)
        {
            return null;
        }
        return Clip.asset as EventTriggerClip;
    }

    public void SetEventTriggerObject(EventTriggerObject obj)
    {
        Trigger_Obj = obj;
    }

}

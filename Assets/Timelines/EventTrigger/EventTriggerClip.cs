using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;



[Serializable]
public class EventTriggerClip : PlayableAsset, ITimelineClipAsset
{
    EventTriggerBehaviour _Template = new EventTriggerBehaviour();

    [Space()]
    [Header("Event Trigger ID")]
    [SerializeField, Tooltip("Trigger ID")]
    string Trigger_ID;

    [Space()]
    [Header("Event Values")]
    [SerializeField, Tooltip("Int Value")]
    int IntValue;
    [SerializeField, Tooltip("Double Value")]
    double DoubleValue;
    [SerializeField, Tooltip("Float Value")]
    float FloatValue;
    [SerializeField, Tooltip("String Value")]
    string StrValue;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<EventTriggerBehaviour>.Create(graph, _Template);
        EventTriggerBehaviour clone = playable.GetBehaviour();

        clone.Trigger_Value.IntValue = IntValue;
        clone.Trigger_Value.DoubleValue = DoubleValue;
        clone.Trigger_Value.FloatValue = FloatValue;
        clone.Trigger_Value.StrValue = StrValue;

        return playable;
    }



    public void SetTimelineClip(TimelineClip c)
    {
        _Template.SetTimelineClip(c);
    }

    public string GetTriggerName()
    {
        return Trigger_ID;
    }

    public void SetEventTriggerObject(EventTriggerObject trigger_obj)
    {
        _Template.SetEventTriggerObject(trigger_obj);
    }

}

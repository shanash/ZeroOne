using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ScreenFaderClip : PlayableAsset, ITimelineClipAsset
{
    [NonSerialized]
    public TimelineClip ClipPassthrough = null;

    [SerializeField]
    public ScreenFaderBehaviour _Template = new ScreenFaderBehaviour();

    public ScreenFaderBehaviour Template { get { return _Template; } private set { _Template = value; } }

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending | ClipCaps.ClipIn | ClipCaps.SpeedMultiplier; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ScreenFaderBehaviour>.Create(graph, _Template);
        return playable;
    }
}

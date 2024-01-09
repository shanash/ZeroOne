using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ShakeCameraClip : PlayableAsset, ITimelineClipAsset
{
    [NonSerialized]
    public TimelineClip ClipPassthrough = null;
    [SerializeField]
    ShakeCameraBehaviour _Template = new ShakeCameraBehaviour();

    public ShakeCameraBehaviour Template { get { return _Template; } private set { _Template = value; } }
    public ClipCaps clipCaps { get { return ClipCaps.Blending | ClipCaps.ClipIn | ClipCaps.SpeedMultiplier; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        Template.Clip = ClipPassthrough;
        var playable = ScriptPlayable<ShakeCameraBehaviour>.Create(graph, Template);
        //타임라인 클립은 이렇게 꺼내쓰면 됩니다
        //ShakeCameraBehaviour clone = playable.GetBehaviour ();
        //clone.Clip;
        return playable;
    }
}

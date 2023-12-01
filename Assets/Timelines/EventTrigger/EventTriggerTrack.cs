using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 1f, 0f)]
[TrackClipType(typeof(EventTriggerClip))]
[TrackBindingType(typeof(EventTriggerObject))]

public class EventTriggerTrack : TrackAsset
{

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // 바인딩 된 게임 오브젝트에서 트리거 오브젝트를 찾는다.
        EventTriggerObject trigger_obj = go.GetComponent<EventTriggerObject>();
        var clips = GetClips();
        foreach (var clip in clips)
        {
            var evt_clip = clip.asset as EventTriggerClip;
            //  behaviour에 타임라인 클립 전달
            evt_clip.SetTimelineClip(clip);
            
            //  behaviour 트리거 오브젝트 전달
            if (trigger_obj != null)
            {
                evt_clip.SetEventTriggerObject(trigger_obj);
            }
        }
        return ScriptPlayable<EventTriggerMixerBehaviour>.Create (graph, inputCount);
    }
}
